using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ExwhyzeeEDI.Web.DataServices.CloudinaryService
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file, string subfolder = "");
        Task<string> UploadImageAsync(Stream stream, string fileName, string subfolder = "");
        Task<string> UploadFileAsync(IFormFile file, string subfolder = "");
        Task<string> UploadFileAsync(Stream stream, string fileName, string subfolder = "");
        Task<bool> DeleteImageAsync(string publicId);
        bool IsConfigured();
    }
}
