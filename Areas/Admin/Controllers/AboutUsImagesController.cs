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
using ExwhyzeeEDI.Web.DataServices.AboutImagesService;
using PagedList;

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    public class AboutUsImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private IAboutImagesServices _aboutImagesServices = new AboutImagesServices();

        public AboutUsImagesController()
        {

        }
        public AboutUsImagesController(
            AboutImagesServices aboutImagesServices

            )
        {
            _aboutImagesServices = aboutImagesServices;
        }

        // GET: Admin/AboutUsImages
        public async Task<ActionResult> Index(string searchString, string currentFilter, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var items = await _aboutImagesServices.List(searchString, currentFilter, page);

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            ViewBag.Total = items.Count();
            return View(items.ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/AboutUsImages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = await _aboutImagesServices.Get(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Admin/AboutUsImages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/AboutUsImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AboutUsImage model, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {

                model.Date = DateTime.UtcNow;
                string output = await _aboutImagesServices.Create(model, upload);
                if (output.Contains("OK"))
                {
                    return RedirectToAction("Index");
                }


            }

            return View(model);

        }

        // GET: Admin/AboutUsImages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var aboutUsImage = await _aboutImagesServices.Get(id);
            if (aboutUsImage == null)
            {
                return HttpNotFound();
            }
            return View(aboutUsImage);
        }

        // POST: Admin/AboutUsImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
      

            public async Task<ActionResult> Edit(AboutUsImage model, HttpPostedFileBase upload)
            {
                if (ModelState.IsValid)
                {
              


                        string output = await _aboutImagesServices.Edit(model, upload);
                        if (output.Contains("OK"))
                        {
                            return RedirectToAction("Index");
                        }

                    

                }
                return View(model);
            }

        // GET: Admin/AboutUsImages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var aboutUsImage = await _aboutImagesServices.Get(id);
            if (aboutUsImage == null)
            {
                return HttpNotFound();
            }
            return View(aboutUsImage);
        }

        // POST: Admin/AboutUsImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _aboutImagesServices.Delete(id);
            return RedirectToAction("Index");
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
