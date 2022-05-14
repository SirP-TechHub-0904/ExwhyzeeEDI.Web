using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Entities;

namespace ExwhyzeeEDI.Web.DataServices.ImageService
{
    public class ImageServices : IImageServices
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<string> Create(ImageFile model, HttpPostedFileBase upload)
        {
            try
            {

                if (upload != null && upload.ContentLength > 0)
                {


                    string date1 = DateTime.UtcNow.AddHours(1).ToString("ssfff");
                    string name = date1 + "-" + upload.FileName;
                    string fileName = Path.GetFileName(name);
                    model.ImagePath = fileName;
                    fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads/"), fileName);
                    upload.SaveAs(fileName);
                    db.ImageFiles.Add(model);
                    await db.SaveChangesAsync();

                }
                return model.ImagePath;
            }
            catch (Exception c)
            {

            }


            return "404";
        }

        public async Task Delete(int? id)
        {
            var item = await db.ImageFiles.FirstOrDefaultAsync(x => x.Id == id);

            if (File.Exists(HttpContext.Current.Server.MapPath("~/Uploads/" + item.ImagePath)))
            {
                File.Delete(HttpContext.Current.Server.MapPath("~/Uploads/" + item.ImagePath));
            }
            if (item != null)
            {
                db.ImageFiles.Remove(item);
                await db.SaveChangesAsync();
            }
        }

        public async Task<string> Edit(ImageFile models, HttpPostedFileBase upload)
        {
            try
            {


                if (File.Exists(HttpContext.Current.Server.MapPath("~/Uploads/" + models.ImagePath)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath("~/Uploads/" + models.ImagePath));
                }
            }
            catch (Exception c) { }
            try
            {

                if (upload != null && upload.ContentLength > 0)
                {


                    string date1 = DateTime.UtcNow.AddHours(1).ToString("ssfff");
                    string name = date1 + "-" + upload.FileName;
                    string fileName = Path.GetFileName(name);
                    models.ImagePath = fileName;
                    fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads/"), fileName);
                    upload.SaveAs(fileName);
                    db.Entry(models).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                }
                return "OK";
            }
            catch (Exception c)
            {

            }


            return "404";
        }

        public async Task<ImageFile> Get(int? id)
        {
            var item = await db.ImageFiles.FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<List<ImageFile>> List(string searchString, string currentFilter, int? page)
        {
            var items = db.ImageFiles.Where(x => x.ImagePath != "");
            if (!String.IsNullOrEmpty(searchString))
            {

                items = items.Where(s => s.Description.ToUpper().Contains(searchString.ToUpper()));

            }
            return await items.ToListAsync();
        }
    }
}