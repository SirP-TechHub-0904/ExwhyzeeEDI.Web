using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class States
    {
        public int? Id { get; set; }


        public string StateName { get; set; }

        public virtual ICollection<LocalGovs> LocalGov { get; set; }


        public static List<States> GetStates()
        {
            var db = new ApplicationDbContext();
            return db.States.ToList();

        }
    }
}