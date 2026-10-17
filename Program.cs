using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ExwhyzeeEDI.Web.DataServices.PaystackService;
using ExwhyzeeEDI.Web.DataServices.PaymentService;
using ExwhyzeeEDI.Web.DataServices.CourseService;
using ExwhyzeeEDI.Web.DataServices.ProgramCourseService;
using ExwhyzeeEDI.Web.DataServices.ReviewService;
using ExwhyzeeEDI.Web.DataServices.SchoolService;
using ExwhyzeeEDI.Web.DataServices.AuthorService;
using ExwhyzeeEDI.Web.DataServices.AboutImagesService;
using ExwhyzeeEDI.Web.DataServices.ImageService;

// Load .env configuration
EnvLoader.Load();

var builder = WebApplication.CreateBuilder(args);

// Configure Database Connection
var connectionString = EnvLoader.Get("ConnectionStrings__DefaultConnection",
    EnvLoader.Get("DEFAULT_CONNECTION",
    builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure Cookie Authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.LoginPath = "/Data/Login";
    options.LogoutPath = "/Data/LogOff";
    options.AccessDeniedPath = "/Data/Login";
    options.SlidingExpiration = true;
});

// Add MVC Services
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddHttpContextAccessor();

// Register Data Services
builder.Services.AddScoped<IPaystackTransactionService, PaystackTransactionService>();
builder.Services.AddScoped<PaystackTransactionService>();
builder.Services.AddScoped<IPaymentServices, PaymentServices>();
builder.Services.AddScoped<PaymentServices>();
builder.Services.AddScoped<ICourseServices, CourseServices>();
builder.Services.AddScoped<CourseServices>();
builder.Services.AddScoped<IProgramCourseServices, ProgramCourseServices>();
builder.Services.AddScoped<ProgramCourseServices>();
builder.Services.AddScoped<IReviewServices, ReviewServices>();
builder.Services.AddScoped<ReviewServices>();
builder.Services.AddScoped<ISchoolServices, SchoolServices>();
builder.Services.AddScoped<SchoolServices>();
builder.Services.AddScoped<IAuthorServices, AuthorServices>();
builder.Services.AddScoped<AuthorServices>();
builder.Services.AddScoped<IAboutImagesServices, AboutImagesServices>();
builder.Services.AddScoped<AboutImagesServices>();
builder.Services.AddScoped<IImageServices, ImageServices>();
builder.Services.AddScoped<ImageServices>();
builder.Services.AddScoped<ExwhyzeeEDI.Web.DataServices.CloudinaryService.ICloudinaryService, ExwhyzeeEDI.Web.DataServices.CloudinaryService.CloudinaryService>();
builder.Services.AddScoped<ExwhyzeeEDI.Web.DataServices.CloudinaryService.CloudinaryService>();

// Register Encoding Provider for ExcelDataReader (supports Windows-1252 codepage)
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

var app = builder.Build();

// Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Secured/Error");
    app.UseHsts();
}

// Initialize AppPaths
AppPaths.ContentRootPath = app.Environment.ContentRootPath;
AppPaths.WebRootPath = app.Environment.WebRootPath ?? Path.Combine(app.Environment.ContentRootPath, "wwwroot");

// Case-normalization middleware for static files on Linux (e.g. /content/ -> /Content/)
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;
    if (!string.IsNullOrEmpty(path))
    {
        if (path.StartsWith("/content/", StringComparison.OrdinalIgnoreCase) && !path.StartsWith("/Content/"))
        {
            context.Request.Path = "/Content" + path.Substring(8);
        }
        else if (path.StartsWith("/scripts/", StringComparison.OrdinalIgnoreCase) && !path.StartsWith("/Scripts/"))
        {
            context.Request.Path = "/Scripts" + path.Substring(8);
        }
    }
    await next();
});

// Primary static files middleware (serves wwwroot natively)
app.UseStaticFiles();

// Fallback for upload folders in content root (if any)
var extraFolders = new[] { "Uploads", "Slider", "BusinessPlan", "ExcelUpload" };
foreach (var folder in extraFolders)
{
    var folderPath = Path.Combine(app.Environment.ContentRootPath, folder);
    if (Directory.Exists(folderPath))
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(folderPath),
            RequestPath = "/" + folder
        });
    }
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Area Routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Default Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Secured}/{action=Index}/{id?}");

app.Run();
