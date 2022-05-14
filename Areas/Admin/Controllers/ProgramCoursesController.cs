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
using ExwhyzeeEDI.Web.DataServices.ProgramCourseService;
using PagedList;

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    public class ProgramCoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IProgramCourseServices _programCourseServices = new ProgramCourseServices();


        public ProgramCoursesController()
        {

        }
        public ProgramCoursesController(
            ProgramCourseServices programCourseServices
            )
        {
            _programCourseServices = programCourseServices;
        }
        // GET: Admin/ProgramCourses
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
            var items = await _programCourseServices.List(searchString, currentFilter, page);

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            ViewBag.Total = items.Count();
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/ProgramCourses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var programCourse = await _programCourseServices.Get(id);
            if (programCourse == null)
            {
                return HttpNotFound();
            }
            return View(programCourse);
        }

        // GET: Admin/ProgramCourses/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FullName");
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList");
            return View();
        }

        // POST: Admin/ProgramCourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProgramCourse programCourse)
        {
            if (ModelState.IsValid)
            {
                programCourse.Date = DateTime.UtcNow;
                string output = await _programCourseServices.Create(programCourse);
                if (output.Contains("OK"))
                {
                    return RedirectToAction("Index");
                }

            }

            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FullName", programCourse.AuthorId);
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList", programCourse.SchoolId);
            return View(programCourse);
        }

        // GET: Admin/ProgramCourses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var programCourse = await _programCourseServices.Get(id);
            if (programCourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FullName", programCourse.AuthorId);
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList", programCourse.SchoolId);
            return View(programCourse);
        }

        // POST: Admin/ProgramCourses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProgramCourse programCourse)
        {
            if (ModelState.IsValid)
            {
                string output = await _programCourseServices.Edit(programCourse);
                if (output.Contains("OK"))
                {
                    return RedirectToAction("Index");
                }
            }
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FullName", programCourse.AuthorId);
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList", programCourse.SchoolId);

            return View(programCourse);
        }

        // GET: Admin/ProgramCourses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var programCourse = await _programCourseServices.Get(id);
            if (programCourse == null)
            {
                return HttpNotFound();
            }
            return View(programCourse);
        }

        // POST: Admin/ProgramCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _programCourseServices.Delete(id);
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
