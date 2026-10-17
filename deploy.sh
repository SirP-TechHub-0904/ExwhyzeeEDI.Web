#!/bin/bash
set -e

APP_DIR="/var/www/exwhyzee-edi"
SERVICE_NAME="exwhyzee-edi.service"
APP_EXEC="ExwhyzeeEDI.Web"

SUDO=""
if [ "$(id -u)" -ne 0 ]; then
  SUDO="sudo"
fi

CURRENT_USER=$(whoami)

echo "=================================================="
echo ">>> Deploying Exwhyzee EDI Web Application"
echo ">>> User: $CURRENT_USER | Target Directory: $APP_DIR"
echo "=================================================="

# 1. Ensure target directory exists and has correct ownership
$SUDO mkdir -p "$APP_DIR"
$SUDO chown -R "$CURRENT_USER":"$CURRENT_USER" "$APP_DIR"

# 2. Back up existing .env if this is a subsequent deployment
PRESERVED_ENV=false
if [ -f "$APP_DIR/.env" ]; then
  echo ">>> Existing .env found - preserving current configuration..."
  cp "$APP_DIR/.env" /tmp/edi_existing.env
  PRESERVED_ENV=true
fi

# 3. Extract new release package
if [ -f "/tmp/app-package.tar.gz" ]; then
  echo ">>> Extracting new release package to $APP_DIR..."
  tar -xzf /tmp/app-package.tar.gz -C "$APP_DIR"
  rm -f /tmp/app-package.tar.gz
fi

# Make self-contained binary executable
chmod +x "$APP_DIR/$APP_EXEC" 2>/dev/null || true

# 4. Handle .env: Prefer GitHub Secret if provided, otherwise preserve existing, fallback to example
if [ -n "$DOTENV_SECRET" ]; then
  echo ">>> Applying .env configuration from GitHub Secret..."
  echo "$DOTENV_SECRET" > "$APP_DIR/.env"
elif [ "$PRESERVED_ENV" = true ]; then
  echo ">>> Restoring preserved production .env file..."
  cp /tmp/edi_existing.env "$APP_DIR/.env"
  rm -f /tmp/edi_existing.env
elif [ -f "$APP_DIR/.env.example" ]; then
  echo ">>> First deploy: Initializing .env from template..."
  cp "$APP_DIR/.env.example" "$APP_DIR/.env"
fi
chmod 600 "$APP_DIR/.env" 2>/dev/null || true

# Ensure runtime directories exist
mkdir -p "$APP_DIR/Uploads"
mkdir -p "$APP_DIR/Uploads/Gallery"
mkdir -p "$APP_DIR/ExcelUpload"
mkdir -p "$APP_DIR/BusinessPlan"
mkdir -p "$APP_DIR/Slider"
chmod -R 755 "$APP_DIR"

# 5. Stop existing service/process temporarily to verify port availability
echo ">>> Checking running services..."
if command -v systemctl >/dev/null 2>&1 && systemctl is-active --quiet "$SERVICE_NAME" 2>/dev/null; then
  echo ">>> Stopping current $SERVICE_NAME..."
  $SUDO systemctl stop "$SERVICE_NAME" || true
fi
pkill -f "$APP_EXEC" 2>/dev/null || true
sleep 2

# 6. Detect / Confirm free port (starting from 5005)
PORT=""
if [ -f "$APP_DIR/.port" ] && [ -s "$APP_DIR/.port" ]; then
  SAVED_PORT=$(cat "$APP_DIR/.port" | tr -d '[:space:]')
  if ! ss -tuln 2>/dev/null | grep -q ":$SAVED_PORT " && ! netstat -tuln 2>/dev/null | grep -q ":$SAVED_PORT "; then
    PORT="$SAVED_PORT"
    echo ">>> Reusing previously assigned port: $PORT"
  fi
fi

if [ -z "$PORT" ]; then
  echo ">>> Detecting next available port (starting from 5005)..."
  CHECK_PORT=5005
  while [ $CHECK_PORT -le 5200 ]; do
    if ! ss -tuln 2>/dev/null | grep -q ":$CHECK_PORT " && ! netstat -tuln 2>/dev/null | grep -q ":$CHECK_PORT "; then
      PORT=$CHECK_PORT
      break
    fi
    CHECK_PORT=$((CHECK_PORT + 1))
  done
fi

if [ -z "$PORT" ]; then
  echo ">>> Warning: Fallback to port 5005"
  PORT=5005
fi

echo "$PORT" > "$APP_DIR/.port"
echo ">>> Application Port selected: $PORT"

# Ensure the assigned port is reflected in .env
if [ -f "$APP_DIR/.env" ]; then
  if grep -q "^PORT=" "$APP_DIR/.env"; then
    sed -i "s|^PORT=.*|PORT=$PORT|" "$APP_DIR/.env"
  else
    echo "PORT=$PORT" >> "$APP_DIR/.env"
  fi

  if grep -q "^ASPNETCORE_URLS=" "$APP_DIR/.env"; then
    sed -i "s|^ASPNETCORE_URLS=.*|ASPNETCORE_URLS=http://0.0.0.0:$PORT|" "$APP_DIR/.env"
  else
    echo "ASPNETCORE_URLS=http://0.0.0.0:$PORT" >> "$APP_DIR/.env"
  fi
fi

# Configure Firewall if UFW is active
if command -v ufw >/dev/null 2>&1; then
  if $SUDO ufw status | grep -q "Status: active"; then
    echo ">>> Opening port $PORT in UFW..."
    $SUDO ufw allow "$PORT/tcp" || true
  fi
fi

# 7. Configure and start Systemd Service
SERVICE_FILE="/etc/systemd/system/$SERVICE_NAME"
if command -v systemctl >/dev/null 2>&1; then
  echo ">>> Configuring Systemd Service: $SERVICE_FILE..."
  $SUDO tee "$SERVICE_FILE" > /dev/null << EOF
[Unit]
Description=Exwhyzee EDI .NET 9 Web Application
After=network.target

[Service]
WorkingDirectory=$APP_DIR
ExecStart=$APP_DIR/$APP_EXEC
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=exwhyzee-edi
User=$CURRENT_USER
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=ASPNETCORE_URLS=http://0.0.0.0:$PORT
EnvironmentFile=-$APP_DIR/.env

[Install]
WantedBy=multi-user.target
EOF

  $SUDO systemctl daemon-reload
  $SUDO systemctl enable "$SERVICE_NAME"
  echo ">>> Starting $SERVICE_NAME..."
  $SUDO systemctl restart "$SERVICE_NAME"
  sleep 3

  if systemctl is-active --quiet "$SERVICE_NAME"; then
    echo ">>> SUCCESS: $SERVICE_NAME is running!"
  else
    echo ">>> WARNING: $SERVICE_NAME failed to start cleanly. Checking logs:"
    $SUDO journalctl -u "$SERVICE_NAME" -n 25 --no-pager || true
    exit 1
  fi
fi

# 8. Configure Nginx Reverse Proxy
NGINX_AVAILABLE="/etc/nginx/sites-available"
NGINX_ENABLED="/etc/nginx/sites-enabled"
if [ -d "$NGINX_AVAILABLE" ]; then
  NGINX_CONF="$NGINX_AVAILABLE/edi.exwhyzee.ng"
  echo ">>> Configuring Nginx reverse proxy ($NGINX_CONF) -> http://127.0.0.1:$PORT..."

  $SUDO tee "$NGINX_CONF" > /dev/null << EOF
server {
    listen 80;
    server_name edi.exwhyzee.ng www.edi.exwhyzee.ng;

    client_max_body_size 50M;

    location / {
        proxy_pass         http://127.0.0.1:$PORT;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade \$http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host \$host;
        proxy_cache_bypass \$http_upgrade;
        proxy_set_header   X-Forwarded-For \$proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto \$scheme;
    }
}
EOF

  $SUDO ln -sf "$NGINX_CONF" "$NGINX_ENABLED/edi.exwhyzee.ng"
  $SUDO nginx -t && $SUDO systemctl reload nginx || echo ">>> Nginx reload warning (check configuration if needed)"
fi

# 9. Health Check
echo ">>> Performing health check on http://127.0.0.1:$PORT..."
HEALTHY=false
for i in 1 2 3 4 5; do
  HTTP_CODE=$(curl -s -o /dev/null -w "%{http_code}" "http://127.0.0.1:$PORT/" || echo "000")
  echo ">>> Health check attempt $i: HTTP Status $HTTP_CODE"
  if [ "$HTTP_CODE" = "200" ] || [ "$HTTP_CODE" = "302" ]; then
    HEALTHY=true
    break
  fi
  sleep 2
done

echo "=================================================="
echo ">>> DEPLOYMENT SUCCEEDED!"
echo ">>> Application binary: $APP_DIR/$APP_EXEC"
echo ">>> Internal Port: $PORT"
echo ">>> Service: $SERVICE_NAME (running as $CURRENT_USER)"
echo "=================================================="
