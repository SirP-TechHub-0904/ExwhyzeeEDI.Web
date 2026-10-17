using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace ExwhyzeeEDI.Web.Helper
{
    public static class AppPaths
    {
        private static string? _webRootPath;
        private static string? _contentRootPath;

        public static string ContentRootPath
        {
            get => _contentRootPath ?? Directory.GetCurrentDirectory();
            set => _contentRootPath = value;
        }

        public static string WebRootPath
        {
            get => _webRootPath ?? Path.Combine(ContentRootPath, "wwwroot");
            set => _webRootPath = value;
        }

        public static string MapPath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return WebRootPath;

            relativePath = relativePath.TrimStart('~', '/', '\\');
            var webPath = Path.Combine(WebRootPath, relativePath);
            if (File.Exists(webPath) || Directory.Exists(webPath))
                return webPath;

            var contentPath = Path.Combine(ContentRootPath, relativePath);
            if (File.Exists(contentPath) || Directory.Exists(contentPath))
                return contentPath;

            return webPath;
        }

        public static async Task SaveAsAsync(this IFormFile file, string destinationPath)
        {
            var directory = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var stream = new FileStream(destinationPath, FileMode.Create);
            await file.CopyToAsync(stream);
        }

        public static void SaveAs(this IFormFile file, string destinationPath)
        {
            var directory = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var stream = new FileStream(destinationPath, FileMode.Create);
            file.CopyTo(stream);
        }
    }
}
