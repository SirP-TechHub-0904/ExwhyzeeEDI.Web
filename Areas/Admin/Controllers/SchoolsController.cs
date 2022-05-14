using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Entities;
using ExwhyzeeEDI.Web.DataServices.SchoolService;

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    public class SchoolsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        private ISchoolServices _schoolServices = new SchoolServices();
        


        public SchoolsController()
        {

        }
        public SchoolsController(
          SchoolServices schoolServices
            )
        {
            _schoolServices = schoolServices;
        }
        // GET: Admin/Schools
        public async Task<ActionResult> Index()
        {
            return View(await _schoolServices.List());
        }

        // GET: Admin/Schools/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var school = await _schoolServices.Get(id);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // GET: Admin/Schools/Create
        public ActionResult CompleteAccount(string userId)
        {
            return View();
        }

        // POST: Admin/Schools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompleteAccount(School school, string userId)
        {
            if (ModelState.IsValid)
            {
                school.UserId = userId;
                school.DateofRegistration = DateTime.UtcNow;
                string output = await _schoolServices.Create(school);
                if (output.Contains("OK"))
                {
                    return RedirectToAction("Index");
                }
            }

            return View(school);
        }

        // GET: Admin/Schools/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var school = await _schoolServices.Get(id);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // POST: Admin/Schools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(School school)
        {
            if (ModelState.IsValid)
            {
                string output = await _schoolServices.Edit(school);
                if (output.Contains("OK"))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(school);
        }

        // GET: Admin/Schools/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var school = await _schoolServices.Get(id);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // POST: Admin/Schools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            string output = await _schoolServices.Delete(id);
            if (output.Contains("OK"))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
