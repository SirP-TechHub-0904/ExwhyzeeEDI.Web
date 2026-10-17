using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ExwhyzeeEDI.Web.Models;

namespace ExwhyzeeEDI.Web.Helper
{
    public static class IdentityExtensions
    {
        public static string? GetUserId(this IIdentity? identity)
        {
            if (identity is ClaimsIdentity claimsIdentity)
            {
                return claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            return null;
        }

        public static string? GetUserId(this ClaimsPrincipal? principal)
        {
            return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string? GetUserName(this IIdentity? identity)
        {
            if (identity is ClaimsIdentity claimsIdentity)
            {
                return claimsIdentity.FindFirst(ClaimTypes.Name)?.Value ?? claimsIdentity.Name;
            }
            return identity?.Name;
        }

        public static async Task<IdentityResult> AddToRoleAsync(this UserManager<ApplicationUser> manager, string userId, string role)
        {
            var user = await manager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            return await manager.AddToRoleAsync(user, role);
        }

        public static async Task<bool> IsInRoleAsync(this UserManager<ApplicationUser> manager, string userId, string role)
        {
            var user = await manager.FindByIdAsync(userId);
            if (user == null) return false;
            return await manager.IsInRoleAsync(user, role);
        }

        public static async Task<IdentityResult> ConfirmEmailAsync(this UserManager<ApplicationUser> manager, string userId, string token)
        {
            var user = await manager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            return await manager.ConfirmEmailAsync(user, token);
        }

        public static async Task<IdentityResult> ResetPasswordAsync(this UserManager<ApplicationUser> manager, string userId, string token, string newPassword)
        {
            var user = await manager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            return await manager.ResetPasswordAsync(user, token, newPassword);
        }

        public static ApplicationUser? FindById(this UserManager<ApplicationUser> manager, string? userId)
        {
            if (string.IsNullOrEmpty(userId)) return null;
            return manager.FindByIdAsync(userId).GetAwaiter().GetResult();
        }

        public static ApplicationUser? FindByName(this UserManager<ApplicationUser> manager, string? name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            return manager.FindByNameAsync(name).GetAwaiter().GetResult();
        }

        public static async Task<string?> GetVerifiedUserIdAsync(this SignInManager<ApplicationUser> manager)
        {
            var user = await manager.GetTwoFactorAuthenticationUserAsync();
            return user?.Id;
        }

        public static Task<bool> SendTwoFactorCodeAsync(this SignInManager<ApplicationUser> manager, string provider)
        {
            return Task.FromResult(true);
        }

        public static Task<IList<string>> GetValidTwoFactorProvidersAsync(this UserManager<ApplicationUser> manager, string userId)
        {
            return Task.FromResult<IList<string>>(new List<string>());
        }

        public static Task<bool> IsEmailConfirmedAsync(this UserManager<ApplicationUser> manager, string userId)
        {
            var user = manager.FindById(userId);
            if (user == null) return Task.FromResult(false);
            return manager.IsEmailConfirmedAsync(user);
        }

        public static async Task<string?> GetPhoneNumberAsync(this UserManager<ApplicationUser> manager, string? userId)
        {
            var user = await manager.FindByIdAsync(userId ?? "");
            return user == null ? null : await manager.GetPhoneNumberAsync(user);
        }

        public static async Task<bool> GetTwoFactorEnabledAsync(this UserManager<ApplicationUser> manager, string? userId)
        {
            var user = await manager.FindByIdAsync(userId ?? "");
            return user != null && await manager.GetTwoFactorEnabledAsync(user);
        }

        public static async Task<IList<UserLoginInfo>> GetLoginsAsync(this UserManager<ApplicationUser> manager, string? userId)
        {
            var user = await manager.FindByIdAsync(userId ?? "");
            return user == null ? new List<UserLoginInfo>() : await manager.GetLoginsAsync(user);
        }

        public static async Task<IdentityResult> RemoveLoginAsync(this UserManager<ApplicationUser> manager, string? userId, UserLoginInfo login)
        {
            var user = await manager.FindByIdAsync(userId ?? "");
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            return await manager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
        }

        public static async Task<IdentityResult> SetPhoneNumberAsync(this UserManager<ApplicationUser> manager, string? userId, string? phoneNumber)
        {
            var user = await manager.FindByIdAsync(userId ?? "");
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            return await manager.SetPhoneNumberAsync(user, phoneNumber);
        }

        public static async Task<IdentityResult> SetTwoFactorEnabledAsync(this UserManager<ApplicationUser> manager, string? userId, bool enabled)
        {
            var user = await manager.FindByIdAsync(userId ?? "");
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            return await manager.SetTwoFactorEnabledAsync(user, enabled);
        }

        public static async Task<string> GenerateChangePhoneNumberTokenAsync(this UserManager<ApplicationUser> manager, string? userId, string? phoneNumber)
        {
            var user = await manager.FindByIdAsync(userId ?? "");
            if (user == null) return "";
            return await manager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber ?? "");
        }

        public static async Task<IdentityResult> ChangePhoneNumberAsync(this UserManager<ApplicationUser> manager, string? userId, string? phoneNumber, string? token)
        {
            var user = await manager.FindByIdAsync(userId ?? "");
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            return await manager.ChangePhoneNumberAsync(user, phoneNumber ?? "", token ?? "");
        }

        public static async Task<IdentityResult> ChangePasswordAsync(this UserManager<ApplicationUser> manager, string? userId, string currentPassword, string newPassword)
        {
            var user = await manager.FindByIdAsync(userId ?? "");
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            return await manager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public static async Task<IdentityResult> AddPasswordAsync(this UserManager<ApplicationUser> manager, string? userId, string password)
        {
            var user = await manager.FindByIdAsync(userId ?? "");
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            return await manager.AddPasswordAsync(user, password);
        }

        public static async Task<IdentityResult> AddLoginAsync(this UserManager<ApplicationUser> manager, string? userId, UserLoginInfo login)
        {
            var user = await manager.FindByIdAsync(userId ?? "");
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            return await manager.AddLoginAsync(user, login);
        }

        public static async Task<Microsoft.AspNetCore.Identity.SignInResult> PasswordSignInAsync(this SignInManager<ApplicationUser> manager, string userName, string password, bool isPersistent, bool shouldLockout)
        {
            return await manager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure: shouldLockout);
        }

        public static async Task<bool> HasBeenVerifiedAsync(this SignInManager<ApplicationUser> manager)
        {
            return (await manager.GetTwoFactorAuthenticationUserAsync()) != null;
        }

        public static async Task<SignInStatus> TwoFactorSignInAsync(this SignInManager<ApplicationUser> manager, string provider, string code, bool isPersistent, bool rememberBrowser)
        {
            var res = await manager.TwoFactorSignInAsync(provider, code, isPersistent, rememberBrowser);
            return res.ToStatus();
        }

        public static async Task SignInAsync(this SignInManager<ApplicationUser> manager, ApplicationUser user, bool isPersistent, bool rememberBrowser)
        {
            await manager.SignInAsync(user, isPersistent);
        }
    }
}
