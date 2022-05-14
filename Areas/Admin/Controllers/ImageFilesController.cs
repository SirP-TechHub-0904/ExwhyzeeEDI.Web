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
using ExwhyzeeEDI.Web.DataServices.ImageService;
using PagedList;

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    public class ImageFilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IImageServices _imageServices = new ImageServices();


        public ImageFilesController()
        {

        }
        public ImageFilesController(
            ImageServices imageServices
            )
        {
            _imageServices = imageServices;
        }
        // GET: Admin/ImageFiles
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
            var items = await _imageServices.List(searchString, currentFilter, page);

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            ViewBag.Total = items.Count();
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/ImageFiles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var imageFile = await _imageServices.Get(id);
            if (imageFile == null)
            {
                return HttpNotFound();
            }
            return View(imageFile);
        }

        // GET: Admin/ImageFiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ImageFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ImageFile imageFile, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string output = await _imageServices.Create(imageFile, upload);
                if (output.Contains("OK"))
                {
                    return RedirectToAction("Index");
                }
                
            }

            return View(imageFile);
        }

        // GET: Admin/ImageFiles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var imageFile = await _imageServices.Get(id);
            if (imageFile == null)
            {
                return HttpNotFound();
            }
            return View(imageFile);
        }

        // POST: Admin/ImageFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ImageFile imageFile, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string output = await _imageServices.Edit(imageFile, upload);
                return RedirectToAction("Index");
            }
            return View(imageFile);
        }

        // GET: Admin/ImageFiles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var imageFile = await _imageServices.Get(id);
            if (imageFile == null)
            {
                return HttpNotFound();
            }
            return View(imageFile);
        }

        // POST: Admin/ImageFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _imageServices.Delete(id);
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
