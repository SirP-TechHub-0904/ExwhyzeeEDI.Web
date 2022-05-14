using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Entities;

namespace ExwhyzeeEDI.Web.DataServices.AboutImagesService
{
    public class AboutImagesServices : IAboutImagesServices
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public async Task<string> Create(AboutUsImage model, HttpPostedFileBase upload)
        {
            try
            {

                if (upload != null && upload.ContentLength > 0)
                {


                    string date1 = DateTime.UtcNow.AddHours(1).ToString("ssfff");
                    string name = date1 + "-" + upload.FileName;
                    string fileName = Path.GetFileName(name);
                    model.ImageUrl = fileName;
                    fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads/Advert/"), fileName);
                    upload.SaveAs(fileName);
                    db.AboutUsImages.Add(model);
                    await db.SaveChangesAsync();

                }
                return model.ImageUrl;
            }
            catch (Exception c)
            {

            }


            return "404";
        }

        public async Task Delete(int? id)
        {
            var item = await db.AboutUsImages.FirstOrDefaultAsync(x => x.Id == id);

            if (File.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Advert/" + item.ImageUrl)))
            {
                File.Delete(HttpContext.Current.Server.MapPath("~/Uploads/Advert/" + item.ImageUrl));
            }
            if (item != null)
            {
                db.AboutUsImages.Remove(item);
                await db.SaveChangesAsync();
            }
        }

        public async Task<string> Edit(AboutUsImage models, HttpPostedFileBase upload)
        {
            try
            {


                if (File.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Advert/" + models.ImageUrl)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath("~/Uploads/Advert/" + models.ImageUrl));
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
                    models.ImageUrl = fileName;
                    fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads/Advert/"), fileName);
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

        public async Task<AboutUsImage> Get(int? id)
        {
            var item = await db.AboutUsImages.FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<List<AboutUsImage>> List(string searchString, string currentFilter, int? page)
        {
            var items = db.AboutUsImages.Where(x => x.ImageName != "");
            if (!String.IsNullOrEmpty(searchString))
            {

                items = items.Where(s => s.ImageName.ToUpper().Contains(searchString.ToUpper()));

            }
            return await items.ToListAsync();
        }
    }
}