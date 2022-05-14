using ExwhyzeeEDI.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin,Data")]

    public class MainController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        // GET: Admin/Main
        public async Task<ActionResult> Index()
        {
           
            return View();
        }

        public ActionResult users()
        {
            var user = db.Users.ToList();
            return View(user);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.FirstOrDefault(x => x.Id == id);
           
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/RegisteredPrograms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == id);
            var payments = db.Payments.Where(x => x.UserId == id);
            foreach (var i in payments)
            {


                db.Payments.Remove(i);
            }
            var userp = db.UserPrograms.Where(x => x.UserId == id);
            foreach (var i in userp)
            {


                db.UserPrograms.Remove(i);
            }
            try
            {
 var pro = db.Profiles.FirstOrDefault(x => x.UserId == id);
            db.Profiles.Remove(pro);
            }catch(Exception c) { }
           
            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}