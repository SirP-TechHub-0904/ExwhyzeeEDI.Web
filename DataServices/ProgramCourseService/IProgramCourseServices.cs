using ExwhyzeeEDI.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExwhyzeeEDI.Web.DataServices.ProgramCourseService
{
   public interface IProgramCourseServices
    {
        Task<string> Create(ProgramCourse model);
        Task<ProgramCourse> Get(int? id);
        Task<string> Edit(ProgramCourse models);
        Task<string> Delete(int? id);
        Task<List<ProgramCourse>> List(string searchString, string currentFilter, int? page);
    }
}
