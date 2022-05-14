using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Entities;

namespace ExwhyzeeEDI.Web.DataServices.AuthorService
{
   
    public class AuthorServices : IAuthorServices
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public async Task<string> Create(Author model)
        {
            try
            {
                model.Date = DateTime.UtcNow;
                db.Authors.Add(model);
                await db.SaveChangesAsync();
                return "OK";
            }
            catch (Exception c)
            {

            }
            return null;
        }

        public async Task<string> Delete(int? id)
        {
            try
            {

                var item = await db.Authors.FirstOrDefaultAsync(x => x.Id == id);

                db.Authors.Remove(item);
                await db.SaveChangesAsync();
                return "OK";

            }
            catch (Exception c)
            {

            }
            return null;
        }

        public async Task<string> Edit(Author models)
        {
            try
            {
                db.Entry(models).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return "OK";
            }
            catch (Exception c)
            {

            }
            return null;
        }

        public async Task<Author> Get(int? id)
        {
            var item = await db.Authors.FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<List<Author>> List(string searchString, string currentFilter, int? page)
        {
            var items = db.Authors.Where(x => x.FullName != "");
            if (!String.IsNullOrEmpty(searchString))
            {

                items = items.Where(s => s.Description.ToUpper().Contains(searchString.ToUpper()) || s.FullName.ToUpper().Contains(searchString.ToUpper()));

            }
            return await items.ToListAsync();
        }
    }
}