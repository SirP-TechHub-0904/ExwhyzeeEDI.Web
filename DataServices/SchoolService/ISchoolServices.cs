using ExwhyzeeEDI.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExwhyzeeEDI.Web.DataServices.SchoolService
{
   public interface ISchoolServices 
    {
        Task<string> Create(School model);
        Task<School> Get(int? id);
        Task<string> Edit(School models);
        Task<string> Delete(int? id);
        Task<List<School>> List();
    }
}
