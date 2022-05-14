using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ExwhyzeeEDI.Web.DataServices.SchoolService
{
    public class SchoolServices : ISchoolServices
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public async Task<string> Create(School model)
        {
            try
            {
                model.DateofRegistration = DateTime.UtcNow;
                db.Schools.Add(model);
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

                var item = await db.Schools.FirstOrDefaultAsync(x => x.Id == id);

                db.Schools.Remove(item);
                await db.SaveChangesAsync();
                return "OK";

            }
            catch (Exception c)
            {

            }
            return null;
        }

        public async Task<string> Edit(School models)
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

        public async Task<School> Get(int? id)
        {
            var item = await db.Schools.FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<List<School>> List()
        {
            var items = db.Schools.ToListAsync();
            return await items;

        }
    }
}