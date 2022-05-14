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
    public class YoungMindAdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/YoungMinds
        public async Task<ActionResult> Index()
        {
            return View(await db.YoungMinds.ToListAsync());
        }

        // GET: Admin/YoungMinds/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YoungMind youngMind = await db.YoungMinds.FindAsync(id);
            if (youngMind == null)
            {
                return HttpNotFound();
            }
            return View(youngMind);
        }

        // GET: Admin/YoungMinds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/YoungMinds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(YoungMind youngMind)
        {
            if (ModelState.IsValid)
            {
                db.YoungMinds.Add(youngMind);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(youngMind);
        }

        // GET: Admin/YoungMinds/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YoungMind youngMind = await db.YoungMinds.FindAsync(id);
            if (youngMind == null)
            {
                return HttpNotFound();
            }
            return View(youngMind);
        }

        // POST: Admin/YoungMinds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(YoungMind youngMind)
        {
            if (ModelState.IsValid)
            {
                db.Entry(youngMind).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(youngMind);
        }

        // GET: Admin/YoungMinds/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YoungMind youngMind = await db.YoungMinds.FindAsync(id);
            if (youngMind == null)
            {
                return HttpNotFound();
            }
            return View(youngMind);
        }

        // POST: Admin/YoungMinds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            YoungMind youngMind = await db.YoungMinds.FindAsync(id);
            db.YoungMinds.Remove(youngMind);
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
