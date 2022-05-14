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

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    public class UserProgramsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/UserPrograms
        public async Task<ActionResult> Index()
        {
            var userPrograms = db.UserPrograms.Include(u => u.GetSchool).Include(u => u.ProgramCourse).Include(x=>x.UserInfo);
            return View(await userPrograms.OrderByDescending(x=>x.DateRegisterd).ToListAsync());
        }
    


        // GET: Admin/UserPrograms/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProgram userProgram = await db.UserPrograms.Include(x => x.UserInfo).FirstOrDefaultAsync(x=>x.Id == id);
            if (userProgram == null)
            {
                return HttpNotFound();
            }
            return View(userProgram);
        }

        // GET: Admin/UserPrograms/Create
        public ActionResult Create()
        {
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "UserId");
            ViewBag.ProgramCourseId = new SelectList(db.ProgramCourses, "Id", "Title");
            return View();
        }

        // POST: Admin/UserPrograms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserId,ProgramCourseId,DateRegisterd,PaymentStatus,CourseStatus,SchoolId")] UserProgram userProgram)
        {
            if (ModelState.IsValid)
            {
                db.UserPrograms.Add(userProgram);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "UserId", userProgram.SchoolId);
            ViewBag.ProgramCourseId = new SelectList(db.ProgramCourses, "Id", "Title", userProgram.ProgramCourseId);
            return View(userProgram);
        }

        // GET: Admin/UserPrograms/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProgram userProgram = await db.UserPrograms.FindAsync(id);
            if (userProgram == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "UserId", userProgram.SchoolId);
            ViewBag.ProgramCourseId = new SelectList(db.ProgramCourses, "Id", "Title", userProgram.ProgramCourseId);
            return View(userProgram);
        }

        // POST: Admin/UserPrograms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserId,ProgramCourseId,DateRegisterd,PaymentStatus,CourseStatus,SchoolId")] UserProgram userProgram)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userProgram).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "UserId", userProgram.SchoolId);
            ViewBag.ProgramCourseId = new SelectList(db.ProgramCourses, "Id", "Title", userProgram.ProgramCourseId);
            return View(userProgram);
        }

        // GET: Admin/UserPrograms/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProgram userProgram = await db.UserPrograms.FindAsync(id);
            if (userProgram == null)
            {
                return HttpNotFound();
            }
            return View(userProgram);
        }

        // POST: Admin/UserPrograms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UserProgram userProgram = await db.UserPrograms.FindAsync(id);
            db.UserPrograms.Remove(userProgram);
            await db.SaveChangesAsync();
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
