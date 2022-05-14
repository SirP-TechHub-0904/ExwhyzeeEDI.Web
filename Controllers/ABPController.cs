using ExwhyzeeEDI.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExwhyzeeEDI.Web.Controllers
{
    public class ABPController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ABP
        public async Task<ActionResult> Verify(string xyzid)
        {
                var item = await db.ABPs.FirstOrDefaultAsync(x => x.RegNumber == xyzid);
                var data1 = Guid.NewGuid().ToString();
                var data2 = Guid.NewGuid().ToString();
                return RedirectToAction("ABPCertificate", new { id = item.Id, dataquery= data1, cbncode= data2, xyztech="Yh76879yy-fytfyttd-ytfYSTYUG-YUDGYFtd-56637hg-cjhgjh"});
           
        }
        public async Task<ActionResult> ABPCertificate(int id, string dataquery, string cbncode, string xyztech)
        {
            var item = await db.ABPs.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }
    }
}