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
            get => _webRootPath ?? ContentRootPath;
            set => _webRootPath = value;
        }

        public static string MapPath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return ContentRootPath;

            relativePath = relativePath.TrimStart('~', '/', '\\');
            return Path.Combine(ContentRootPath, relativePath);
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
