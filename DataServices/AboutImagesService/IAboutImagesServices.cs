using ExwhyzeeEDI.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ExwhyzeeEDI.Web.DataServices.AboutImagesService
{
   public interface IAboutImagesServices
    {

        Task<string> Create(AboutUsImage model, HttpPostedFileBase upload);
        Task<AboutUsImage> Get(int? id);
        Task<string> Edit(AboutUsImage models, HttpPostedFileBase upload);
        Task Delete(int? id);
        Task<List<AboutUsImage>> List(string searchString, string currentFilter, int? page);
    }
}
