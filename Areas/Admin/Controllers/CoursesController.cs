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
using ExwhyzeeEDI.Web.DataServices.CourseService;
using PagedList;

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private IImageServices _imageServices = new ImageServices();
        private ICourseServices _courseServices = new CourseServices();


        public CoursesController()
        {

        }
        public CoursesController(
            ImageServices imageServices,
            CourseServices courseServices
            )
        {
            _imageServices = imageServices;
            _courseServices = courseServices;
        }
        // GET: Admin/Courses
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
            var items = await _courseServices.List(searchString, currentFilter, page);

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            ViewBag.Total = items.Count();
            return View(items.ToPagedList(pageNumber, pageSize));
        }


        public async Task<ActionResult> ProgramCourseOutline(string searchString, string currentFilter, int? page, int id)
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
            var items = await _courseServices.ListCousesByProgram(searchString, currentFilter, page, id);

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            ViewBag.Total = items.Count();
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Courses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = await _courseServices.Get(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Admin/Courses/Create
        public ActionResult Create(int id)
        {
            ViewBag.programId = id;
            //ViewBag.ProgramCourseId = new SelectList(db.ProgramCourses, "Id", "Title");
            return View();
        }

        // POST: Admin/Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Course course, HttpPostedFileBase upload, int id)
        {
            course.Date = DateTime.UtcNow;
            course.ProgramCourseId = id;
            if (ModelState.IsValid)
            {
                ImageFile img = new ImageFile();
                img.Description = course.Topic;
                string imgpath = await _imageServices.Create(img, upload);
                if (!String.IsNullOrEmpty(imgpath))
                {
                    course.FilePath = imgpath;
                    string output = await _courseServices.Create(course);
                    if (output.Contains("OK"))
                    {
                        return RedirectToAction("ProgramCourseOutline", new { id = id });
                    }
                   
                }
               
            }
            ViewBag.programId = id;
            // ViewBag.ProgramCourseId = new SelectList(db.ProgramCourses, "Id", "Title", course.ProgramCourseId);
            return View(course);
        }

        // GET: Admin/Courses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = await _courseServices.Get(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProgramCourseId = new SelectList(db.ProgramCourses, "Id", "Title", course.ProgramCourseId);
            return View(course);
        }

        // POST: Admin/Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Course course, HttpPostedFileBase upload)
        {
           

            if (ModelState.IsValid)
            {
                ImageFile img = new ImageFile();
                img.Description = course.Topic;
                string imgpath = await _imageServices.Create(img, upload);
                if (!String.IsNullOrEmpty(imgpath))
                {
                    course.FilePath = imgpath;
                    string output = await _courseServices.Edit(course);
                    if (output.Contains("OK"))
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.ProgramCourseId = new SelectList(db.ProgramCourses, "Id", "Title", course.ProgramCourseId);
            return View(course);
        }

        // GET: Admin/Courses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = await _courseServices.Get(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Admin/Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _courseServices.Delete(id);
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
