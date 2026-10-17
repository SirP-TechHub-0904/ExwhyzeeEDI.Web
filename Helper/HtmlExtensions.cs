using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Microsoft.AspNetCore.Mvc.Rendering
{
    public static class HtmlHelperCompatExtensions
    {
        public static MvcForm BeginForm(this IHtmlHelper htmlHelper, string actionName, string controllerName, object? routeValues, FormMethod method, object? htmlAttributes)
        {
            return htmlHelper.BeginForm(actionName, controllerName, routeValues, method, null, htmlAttributes);
        }

        public static IHtmlContent Label(this IHtmlHelper htmlHelper, string expression, object? htmlAttributes)
        {
            return htmlHelper.Label(expression, expression, htmlAttributes);
        }

        public static IHtmlContent Action(this IHtmlHelper htmlHelper, string action, string controller, object? routeValues = null)
        {
            return RenderActionHelper(htmlHelper, action, controller, routeValues);
        }

        public static IHtmlContent Action(this IHtmlHelper htmlHelper, string action, object? routeValues = null)
        {
            return RenderActionHelper(htmlHelper, action, null, routeValues);
        }

        private static IHtmlContent RenderActionHelper(IHtmlHelper htmlHelper, string action, string? controller, object? routeValues)
        {
            try
            {
                using var db = new ExwhyzeeEDI.Web.Models.ApplicationDbContext();

                if (string.Equals(action, "_Slider", StringComparison.OrdinalIgnoreCase))
                {
                    var slide = db.Sliders.OrderByDescending(x => x.SortOrder).ToList();
                    return htmlHelper.PartialAsync("~/Views/Secured/_Slider.cshtml", slide).GetAwaiter().GetResult();
                }

                if (string.Equals(action, "_Gallery", StringComparison.OrdinalIgnoreCase))
                {
                    var gallery = db.Galleries.OrderBy(x => x.SortOrder).ThenBy(x => x.DateUpload).ToList();
                    return htmlHelper.PartialAsync("~/Views/Secured/_Gallery.cshtml", gallery).GetAwaiter().GetResult();
                }

                if (string.Equals(action, "_LatestNews", StringComparison.OrdinalIgnoreCase))
                {
                    var post = db.Posts.OrderBy(x => x.SortOrder).Skip(2).Take(4).ToList();
                    return htmlHelper.PartialAsync("~/Views/Secured/_LatestNews.cshtml", post).GetAwaiter().GetResult();
                }

                if (string.Equals(action, "_FooterPosts", StringComparison.OrdinalIgnoreCase))
                {
                    var post = db.Posts.OrderBy(x => x.DatePosted).ToList();
                    if (post.Count > 6)
                    {
                        post = post.Skip(3).Take(3).ToList();
                    }
                    return htmlHelper.PartialAsync("~/Views/Secured/_FooterPosts.cshtml", post.Take(3).ToList()).GetAwaiter().GetResult();
                }

                if (string.Equals(action, "LayoutProfile", StringComparison.OrdinalIgnoreCase))
                {
                    var userId = htmlHelper.ViewContext.HttpContext.User?.Identity?.GetUserId();
                    var item = !string.IsNullOrEmpty(userId) ? db.Profiles.FirstOrDefault(x => x.UserId == userId) : null;
                    return htmlHelper.PartialAsync("~/Areas/Admin/Views/PartialView/LayoutProfile.cshtml", item).GetAwaiter().GetResult();
                }

                if (string.Equals(action, "LayoutDaashboardIcon", StringComparison.OrdinalIgnoreCase))
                {
                    var userId = htmlHelper.ViewContext.HttpContext.User?.Identity?.GetUserId();
                    var item = !string.IsNullOrEmpty(userId) ? db.Profiles.FirstOrDefault(x => x.UserId == userId) : null;
                    var school = item != null && item.SchoolId.HasValue ? db.Schools.FirstOrDefault(x => x.Id == item.SchoolId.Value) : null;
                    return htmlHelper.PartialAsync("~/Areas/Admin/Views/PartialView/LayoutDaashboardIcon.cshtml", school).GetAwaiter().GetResult();
                }

                if (string.Equals(action, "_AdminDashbar", StringComparison.OrdinalIgnoreCase))
                {
                    var viewData = new ViewDataDictionary(htmlHelper.ViewData)
                    {
                        ["users"] = db.Profiles.Count(),
                        ["paidUsers"] = db.UserPrograms.Count(x => x.PaymentStatus == ExwhyzeeEDI.Web.Models.Entities.Enum.UserProgramPaymentStatus.Successful),
                        ["businessPlan"] = db.BusinessPlans.Count(),
                        ["sch"] = db.Schools.Count()
                    };
                    return htmlHelper.PartialAsync("~/Areas/Admin/Views/PartialView/_AdminDashbar.cshtml", null, viewData).GetAwaiter().GetResult();
                }

                if (string.Equals(action, "_Profile", StringComparison.OrdinalIgnoreCase))
                {
                    var userId = htmlHelper.ViewContext.HttpContext.User?.Identity?.GetUserId();
                    var profile = !string.IsNullOrEmpty(userId)
                        ? Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.Include(db.Profiles, x => x.User).FirstOrDefault(x => x.UserId == userId)
                        : null;
                    return htmlHelper.PartialAsync("~/Areas/EDI/Views/Dashboard/_Profile.cshtml", profile).GetAwaiter().GetResult();
                }

                if (string.Equals(action, "_RegisteredProgram", StringComparison.OrdinalIgnoreCase) || string.Equals(action, "RegisteredProgram", StringComparison.OrdinalIgnoreCase))
                {
                    var userId = htmlHelper.ViewContext.HttpContext.User?.Identity?.GetUserId();
                    var programUser = !string.IsNullOrEmpty(userId)
                        ? Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.Include(db.UserPrograms, x => x.ProgramCourse).Where(x => x.PaymentStatus == ExwhyzeeEDI.Web.Models.Entities.Enum.UserProgramPaymentStatus.Pending && x.UserId == userId).ToList()
                        : new List<ExwhyzeeEDI.Web.Models.Entities.UserProgram>();
                    return htmlHelper.PartialAsync("~/Areas/EDI/Views/Dashboard/RegisteredProgram.cshtml", programUser).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                return new HtmlString($"<!-- Html.Action({action}) error: {System.Web.HttpUtility.HtmlEncode(ex.Message)} -->");
            }

            return HtmlString.Empty;
        }

        public static IHtmlContent EnumDropDownListFor<TModel, TEnum>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TEnum>> expression,
            string? optionLabel = null,
            object? htmlAttributes = null)
            where TEnum : struct
        {
            var selectList = htmlHelper.GetEnumSelectList<TEnum>();
            if (!string.IsNullOrEmpty(optionLabel))
            {
                return htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
            }
            return htmlHelper.DropDownListFor(expression, selectList, htmlAttributes);
        }

        public static IHtmlContent EnumDropDownListFor<TModel, TEnum>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TEnum>> expression,
            object? htmlAttributes)
            where TEnum : struct
        {
            return htmlHelper.EnumDropDownListFor(expression, null, htmlAttributes);
        }
    }
}

namespace System.Web.Optimization
{
    public static class Scripts
    {
        public static HtmlString Render(params string[] paths)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var p in paths)
            {
                var resolved = p.StartsWith("~/") ? p.Substring(1) : p;
                sb.AppendLine($"<script src=\"{resolved}\"></script>");
            }
            return new HtmlString(sb.ToString());
        }
    }

    public static class Styles
    {
        public static HtmlString Render(params string[] paths)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var p in paths)
            {
                var resolved = p.StartsWith("~/") ? p.Substring(1) : p;
                sb.AppendLine($"<link rel=\"stylesheet\" href=\"{resolved}\" />");
            }
            return new HtmlString(sb.ToString());
        }
    }
}
