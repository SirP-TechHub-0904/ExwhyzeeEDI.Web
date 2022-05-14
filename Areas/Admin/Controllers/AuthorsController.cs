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
using ExwhyzeeEDI.Web.DataServices.AuthorService;
using PagedList;
using ExwhyzeeEDI.Web.DataServices.ImageService;

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    public class AuthorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IAuthorServices _authorServices = new AuthorServices();
        private IImageServices _imageServices = new ImageServices();


        public AuthorsController()
        {

        }
        public AuthorsController(
            AuthorServices authorServices,
            ImageServices imageServices
            )
        {
            _imageServices = imageServices;
            _authorServices = authorServices;
        }
        // GET: Admin/Authors
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
            var items = await _authorServices.List(searchString, currentFilter, page);

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            ViewBag.Total = items.Count();
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Authors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var author = await _authorServices.Get(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: Admin/Authors/Create
        public ActionResult Create()
        {
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList");

            return View();
        }

        // POST: Admin/Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Author author, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                ImageFile img = new ImageFile();
                img.Description = author.FullName;
                string imgpath = await _imageServices.Create(img, upload);
                if (!String.IsNullOrEmpty(imgpath))
                {
                    author.ImagePath = imgpath;
                    string output = await _authorServices.Create(author);
                    if (output.Contains("OK"))
                    {
                        return RedirectToAction("Index");
                    }

                }
            }
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList", author.SchoolId);

            return View(author);
        }

        // GET: Admin/Authors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var author = await _authorServices.Get(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList", author.SchoolId);

            return View(author);
        }

        // POST: Admin/Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Author author, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                ImageFile img = new ImageFile();
                img.Description = author.FullName;
                string imgpath = await _imageServices.Create(img, upload);
                if (!String.IsNullOrEmpty(imgpath))
                {
                    author.ImagePath = imgpath;
                    string output = await _authorServices.Edit(author);
                    if (output.Contains("OK"))
                    {
                        return RedirectToAction("Index");
                    }

                }
               
            }
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList", author.SchoolId);

            return View(author);
        }

        // GET: Admin/Authors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var author = await _authorServices.Get(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Admin/Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _authorServices.Delete(id);
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
