using System;
using System.IO;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ExwhyzeeEDI.Web.Helper;

namespace ExwhyzeeEDI.Web.DataServices.CloudinaryService
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary? _cloudinary;
        private readonly string _rootFolder;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = EnvLoader.Get("CLOUDINARY_CLOUD_NAME",
                            configuration["Cloudinary:CloudName"] ?? "yffz4ii9");

            var apiKey = EnvLoader.Get("CLOUDINARY_API_KEY",
                         configuration["Cloudinary:ApiKey"] ?? "[REDACTED_SECRET_KEY]");

            var apiSecret = EnvLoader.Get("CLOUDINARY_API_SECRET",
                            configuration["Cloudinary:ApiSecret"] ?? "");

            _rootFolder = EnvLoader.Get("CLOUDINARY_ROOT_FOLDER",
                          configuration["Cloudinary:RootFolder"] ?? "edi");

            if (!string.IsNullOrEmpty(cloudName) && !string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(apiSecret))
            {
                var account = new Account(cloudName, apiKey, apiSecret);
                _cloudinary = new Cloudinary(account);
                _cloudinary.Api.Secure = true;
            }
        }

        public bool IsConfigured()
        {
            return _cloudinary != null;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string subfolder = "")
        {
            if (file == null || file.Length == 0) return string.Empty;
            using var stream = file.OpenReadStream();
            return await UploadImageAsync(stream, file.FileName, subfolder);
        }

        public async Task<string> UploadImageAsync(Stream stream, string fileName, string subfolder = "")
        {
            if (_cloudinary == null)
            {
                throw new InvalidOperationException("Cloudinary is not configured. Please supply the Cloudinary API Secret in .env or appsettings.json.");
            }

            var folder = string.IsNullOrWhiteSpace(subfolder)
                ? _rootFolder
                : $"{_rootFolder}/{subfolder.Trim('/')}";

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception($"Cloudinary upload failed: {uploadResult.Error.Message}");
            }

            return uploadResult.SecureUrl?.ToString() ?? uploadResult.Url?.ToString() ?? string.Empty;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string subfolder = "")
        {
            if (file == null || file.Length == 0) return string.Empty;
            using var stream = file.OpenReadStream();
            return await UploadFileAsync(stream, file.FileName, subfolder);
        }

        public async Task<string> UploadFileAsync(Stream stream, string fileName, string subfolder = "")
        {
            if (_cloudinary == null)
            {
                throw new InvalidOperationException("Cloudinary is not configured. Please supply the Cloudinary API Secret in .env or appsettings.json.");
            }

            var folder = string.IsNullOrWhiteSpace(subfolder)
                ? _rootFolder
                : $"{_rootFolder}/{subfolder.Trim('/')}";

            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            var isImage = ext is ".jpg" or ".jpeg" or ".png" or ".gif" or ".webp" or ".bmp" or ".svg";

            if (isImage)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, stream),
                    Folder = folder,
                    UseFilename = true,
                    UniqueFilename = true,
                    Overwrite = false
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                {
                    throw new Exception($"Cloudinary image upload failed: {uploadResult.Error.Message}");
                }
                return uploadResult.SecureUrl?.ToString() ?? uploadResult.Url?.ToString() ?? string.Empty;
            }
            else
            {
                var rawParams = new RawUploadParams
                {
                    File = new FileDescription(fileName, stream),
                    Folder = folder,
                    UseFilename = true,
                    UniqueFilename = true,
                    Overwrite = false
                };

                var rawResult = await _cloudinary.UploadAsync(rawParams);
                if (rawResult.Error != null)
                {
                    throw new Exception($"Cloudinary file upload failed: {rawResult.Error.Message}");
                }
                return rawResult.SecureUrl?.ToString() ?? rawResult.Url?.ToString() ?? string.Empty;
            }
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            if (_cloudinary == null || string.IsNullOrWhiteSpace(publicId)) return false;

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok";
        }
    }
}
