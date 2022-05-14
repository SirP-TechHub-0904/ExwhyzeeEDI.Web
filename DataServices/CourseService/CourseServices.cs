using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Entities;

namespace ExwhyzeeEDI.Web.DataServices.CourseService
{
    public class CourseServices : ICourseServices
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public async Task<string> Create(Course model)
        {
            try
            {
                model.Date = DateTime.UtcNow;
                db.Courses.Add(model);
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

                var item = await db.Courses.FirstOrDefaultAsync(x => x.Id == id);
                db.Courses.Remove(item);
                await db.SaveChangesAsync();
                return "OK";

            }
            catch (Exception c)
            {

            }
            return null;
        }

        public async Task<string> Edit(Course models)
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

        public async Task<Course> Get(int? id)
        {
            var item = await db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<List<Course>> List(string searchString, string currentFilter, int? page)
        {
            var items = db.Courses.Include(x => x.GetProgramCourse).Where(x => x.Topic != "");
            if (!String.IsNullOrEmpty(searchString))
            {

                items = items.Where(s => s.Description.ToUpper().Contains(searchString.ToUpper()) || s.Topic.ToUpper().Contains(searchString.ToUpper()));

            }
            return await items.ToListAsync();
        }

        public async Task<List<Course>> ListCousesByProgram(string searchString, string currentFilter, int? page, int id)
        {
            var items = db.Courses.Include(x=>x.GetProgramCourse).Where(x => x.ProgramCourseId == id);
            if (!String.IsNullOrEmpty(searchString))
            {

                items = items.Where(s => s.Description.ToUpper().Contains(searchString.ToUpper()) || s.Topic.ToUpper().Contains(searchString.ToUpper()));

            }
            return await items.ToListAsync();
        }
    }
}