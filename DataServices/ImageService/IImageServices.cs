using ExwhyzeeEDI.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ExwhyzeeEDI.Web.DataServices.ImageService
{
   public interface IImageServices
    {
        Task<string> Create(ImageFile model, HttpPostedFileBase upload);
        Task<ImageFile> Get(int? id);
        Task<string> Edit(ImageFile models, HttpPostedFileBase upload);
        Task Delete(int? id);
        Task<List<ImageFile>> List(string searchString, string currentFilter, int? page);
    }
}
