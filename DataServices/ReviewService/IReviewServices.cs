using ExwhyzeeEDI.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExwhyzeeEDI.Web.DataServices.ReviewService
{
   public interface IReviewServices
    {
        Task<string> Create(Review model);
        Task<Review> Get(int? id);
        Task<string> Edit(Review models);
        Task<string> Delete(int? id);
        Task<List<Review>> List(string searchString, string currentFilter, int? page);
    }
}
