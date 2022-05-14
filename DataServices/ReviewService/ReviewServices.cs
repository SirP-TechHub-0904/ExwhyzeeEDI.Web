using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Entities;

namespace ExwhyzeeEDI.Web.DataServices.ReviewService
{
    public class ReviewServices : IReviewServices
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public async Task<string> Create(Review model)
        {
            try
            {
                model.DateReviewed = DateTime.UtcNow;
                db.Reviews.Add(model);
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

                var item = await db.Reviews.FirstOrDefaultAsync(x => x.Id == id);

                db.Reviews.Remove(item);
                await db.SaveChangesAsync();
                return "OK";

            }
            catch (Exception c)
            {

            }
            return null;
        }

        public async Task<string> Edit(Review models)
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

        public async Task<Review> Get(int? id)
        {
            var item = await db.Reviews.FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<List<Review>> List(string searchString, string currentFilter, int? page)
        {
            var items = db.Reviews.Where(x => x.Content != "");
            if (!String.IsNullOrEmpty(searchString))
            {

                items = items.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()) || s.Content.ToUpper().Contains(searchString.ToUpper()));

            }
            return await items.ToListAsync();
        }
    }
}