using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ExwhyzeeEDI.Web.Models;

namespace ExwhyzeeEDI.Web.Helper
{
    public static class Server
    {
        public static string MapPath(string path)
        {
            return AppPaths.MapPath(path);
        }
    }

    public abstract class BaseController : Microsoft.AspNetCore.Mvc.Controller
    {
        public NotFoundResult HttpNotFound() => NotFound();
        public NotFoundResult HttpNotFound(string? message) => NotFound();
        public JsonResult Json(object? data, JsonRequestBehavior behavior) => base.Json(data);
    }
}

namespace Microsoft.AspNetCore.Mvc
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowHtmlAttribute : Attribute { }

    public class HttpUnauthorizedResult : UnauthorizedResult { }

    public class HttpStatusCodeResult : StatusCodeResult
    {
        public HttpStatusCodeResult(int statusCode) : base(statusCode) { }
        public HttpStatusCodeResult(System.Net.HttpStatusCode statusCode) : base((int)statusCode) { }
    }

    public static class ControllerCompatExtensions
    {
        public static NotFoundResult HttpNotFound(this Controller controller) => controller.NotFound();
        public static NotFoundResult HttpNotFound(this Controller controller, string message) => controller.NotFound();
    }

    public enum JsonRequestBehavior
    {
        AllowGet,
        DenyGet
    }

    public static class ControllerJsonExtensions
    {
        public static JsonResult Json(this Controller controller, object? data, JsonRequestBehavior behavior)
        {
            return controller.Json(data);
        }
    }
}

namespace Microsoft.AspNetCore.Identity
{
    public enum SignInStatus
    {
        Success,
        LockedOut,
        RequiresVerification,
        Failure
    }

    public static class SignInResultExtensions
    {
        public static SignInStatus ToStatus(this Microsoft.AspNetCore.Identity.SignInResult result)
        {
            if (result.Succeeded) return SignInStatus.Success;
            if (result.IsLockedOut) return SignInStatus.LockedOut;
            if (result.RequiresTwoFactor) return SignInStatus.RequiresVerification;
            return SignInStatus.Failure;
        }
    }
}

namespace Microsoft.AspNetCore.Http
{
    public static class OwinShimExtensions
    {
        public static OwinContextShim GetOwinContext(this HttpContext context) => new OwinContextShim(context);
    }

    public class OwinContextShim
    {
        private readonly HttpContext _context;
        public OwinContextShim(HttpContext context) { _context = context; }

        public T Get<T>() where T : class
        {
            return _context.RequestServices.GetService<T>() ?? ActivatorUtilities.CreateInstance<T>(_context.RequestServices);
        }

        public T GetUserManager<T>() where T : class
        {
            return _context.RequestServices.GetService<T>() ?? ActivatorUtilities.CreateInstance<T>(_context.RequestServices);
        }

        public IAuthenticationManagerShim Authentication => new IAuthenticationManagerShim(_context);
    }

    public class IAuthenticationManagerShim
    {
        private readonly HttpContext _context;
        public IAuthenticationManagerShim(HttpContext context) { _context = context; }

        public void SignOut(params string[] authenticationTypes)
        {
            var signInManager = _context.RequestServices.GetService<SignInManager<ApplicationUser>>();
            signInManager?.SignOutAsync().GetAwaiter().GetResult();
        }

        public void SignIn(AuthenticationProperties properties, ClaimsIdentity identity)
        {
            // Identity handled via SignInManager
        }

        public Task<ExternalLoginInfo?> GetExternalLoginInfoAsync()
        {
            var signInManager = _context.RequestServices.GetService<SignInManager<ApplicationUser>>();
            return signInManager?.GetExternalLoginInfoAsync() ?? Task.FromResult<ExternalLoginInfo?>(null);
        }

        public async Task<ExternalLoginInfoShim?> GetExternalLoginInfoAsync(string xsrfKey, string expectedValue)
        {
            var signInManager = _context.RequestServices.GetService<SignInManager<ApplicationUser>>();
            if (signInManager == null) return null;
            var info = await signInManager.GetExternalLoginInfoAsync(expectedValue);
            return info != null ? new ExternalLoginInfoShim { Login = info } : null;
        }

        public Task<bool> TwoFactorBrowserRememberedAsync(string? userId)
        {
            return Task.FromResult(false);
        }

        public void Challenge(AuthenticationProperties properties, string provider)
        {
            _context.ChallengeAsync(provider, properties).GetAwaiter().GetResult();
        }

        public IEnumerable<AuthenticationDescriptionShim> GetExternalAuthenticationTypes()
        {
            return Array.Empty<AuthenticationDescriptionShim>();
        }
    }

    public class AuthenticationDescriptionShim
    {
        public string AuthenticationType { get; set; } = "";
        public string Caption { get; set; } = "";
    }

    public class ExternalLoginInfoShim
    {
        public UserLoginInfo Login { get; set; } = null!;
    }
}

namespace System.Web
{
    public static class HttpContext
    {
        public static HttpContextCurrent Current => new HttpContextCurrent();
    }

    public class HttpContextCurrent
    {
        public HttpServerUtility Server => new HttpServerUtility();
        public HttpRequestCurrent Request => new HttpRequestCurrent();
    }

    public class HttpServerUtility
    {
        public string MapPath(string path) => ExwhyzeeEDI.Web.Helper.AppPaths.MapPath(path);
    }

    public class HttpRequestCurrent
    {
        public string PhysicalApplicationPath => ExwhyzeeEDI.Web.Helper.AppPaths.ContentRootPath;
    }
}
