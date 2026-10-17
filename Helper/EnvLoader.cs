using System;
using System.IO;

namespace ExwhyzeeEDI.Web.Helper
{
    public static class EnvLoader
    {
        /// <summary>
        /// Loads key-value pairs from a .env file into the process environment variables if present.
        /// </summary>
        public static void Load(string? filePath = null)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    // Walk up parent directories to find .env (e.g. when launched from bin/Debug/net9.0)
                    var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
                    while (dir != null)
                    {
                        var candidate = Path.Combine(dir.FullName, ".env");
                        if (File.Exists(candidate))
                        {
                            filePath = candidate;
                            break;
                        }
                        dir = dir.Parent;
                    }

                    if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                    {
                        // Fallback: check AppDomain base directory and parents
                        dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                        while (dir != null)
                        {
                            var candidate = Path.Combine(dir.FullName, ".env");
                            if (File.Exists(candidate))
                            {
                                filePath = candidate;
                                break;
                            }
                            dir = dir.Parent;
                        }
                    }
                }

                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                    return;

                foreach (var line in File.ReadAllLines(filePath))
                {
                    var trimmed = line.Trim();
                    if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                        continue;

                    var separatorIndex = trimmed.IndexOf('=');
                    if (separatorIndex <= 0)
                        continue;

                    var key = trimmed.Substring(0, separatorIndex).Trim();
                    var value = trimmed.Substring(separatorIndex + 1).Trim();

                    // Strip wrapping double or single quotes if present
                    if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                        (value.StartsWith("'") && value.EndsWith("'")))
                    {
                        if (value.Length >= 2)
                        {
                            value = value.Substring(1, value.Length - 2);
                        }
                    }

                    if (!string.IsNullOrEmpty(key))
                    {
                        // If ASPNETCORE_URLS is already provided by Visual Studio / launchSettings, do not overwrite it
                        if (key == "ASPNETCORE_URLS" && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")))
                        {
                            continue;
                        }

                        Environment.SetEnvironmentVariable(key, value);
                    }
                }
            }
            catch
            {
                // Silently ignore if file cannot be read so normal startup is uninterrupted
            }
        }

        public static string Get(string key, string fallback = "")
        {
            var value = Environment.GetEnvironmentVariable(key);
            return string.IsNullOrEmpty(value) ? fallback : value;
        }
    }
}
