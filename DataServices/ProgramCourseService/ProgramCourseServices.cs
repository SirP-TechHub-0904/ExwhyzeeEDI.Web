using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Entities;

namespace ExwhyzeeEDI.Web.DataServices.ProgramCourseService
{
    public class ProgramCourseServices : IProgramCourseServices
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<string> Create(ProgramCourse model)
        {
            try
            {
                model.Date = DateTime.UtcNow;
                db.ProgramCourses.Add(model);
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

                var item = await db.ProgramCourses.FirstOrDefaultAsync(x => x.Id == id);

                db.ProgramCourses.Remove(item);
                await db.SaveChangesAsync();
                return "OK";
                
            }
            catch (Exception c)
            {

            }
            return null;
        }

        public async Task<string> Edit(ProgramCourse models)
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

        public async Task<ProgramCourse> Get(int? id)
        {
            var item = await db.ProgramCourses.Include(x => x.GetAuthor).Include(x => x.GetCourses).FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<List<ProgramCourse>> List(string searchString, string currentFilter, int? page)
        {
            var items = db.ProgramCourses.Include(x=>x.GetAuthor).Include(x=>x.GetCourses).Where(x => x.Status == Models.Entities.Enum.ProgramStatus.Published);
            if (!String.IsNullOrEmpty(searchString))
            {

                items = items.Where(s => s.Description.ToUpper().Contains(searchString.ToUpper()) || s.Title.ToUpper().Contains(searchString.ToUpper()));

            }
            return await items.ToListAsync();
        }
    }
}