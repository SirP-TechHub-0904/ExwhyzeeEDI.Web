using ExwhyzeeEDI.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExwhyzeeEDI.Web.DataServices.AuthorService
{
    public interface IAuthorServices
    {
        Task<string> Create(Author model);
        Task<Author> Get(int? id);
        Task<string> Edit(Author models);
        Task<string> Delete(int? id);
        Task<List<Author>> List(string searchString, string currentFilter, int? page);
    }
}
