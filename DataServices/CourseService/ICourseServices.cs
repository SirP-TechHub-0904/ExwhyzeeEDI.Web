using ExwhyzeeEDI.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExwhyzeeEDI.Web.DataServices.CourseService
{
    public interface ICourseServices
    {
        Task<string> Create(Course model);
        Task<Course> Get(int? id);
        Task<string> Edit(Course models);
        Task<string> Delete(int? id);
        Task<List<Course>> List(string searchString, string currentFilter, int? page);
        Task<List<Course>> ListCousesByProgram(string searchString, string currentFilter, int? page, int id);
    }
}
