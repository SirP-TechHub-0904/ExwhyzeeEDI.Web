using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Entities;
using System.IO;
using System.Net.Sockets;

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GalleriesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ExwhyzeeEDI.Web.DataServices.CloudinaryService.ICloudinaryService _cloudinaryService;

        public GalleriesController(ApplicationDbContext context, ExwhyzeeEDI.Web.DataServices.CloudinaryService.ICloudinaryService cloudinaryService)
        {
            db = context;
            _cloudinaryService = cloudinaryService;
        }

        // GET: Admin/Galleries
        public async Task<ActionResult> Index()
        {
            
            return View(await db.Galleries.ToListAsync());
        }

        // GET: Admin/Galleries/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = await db.Galleries.FindAsync(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // GET: Admin/Galleries/Create
        public ActionResult Create()
        {
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList");
            return View();
        }

        // POST: Admin/Galleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Gallery gallery, List<HttpPostedFileBase> upload)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (upload.Count() > 0)
                    {
                        foreach (var image in upload)
                        {

                            if (image != null && image.Length > 0)
                            {
                                if (_cloudinaryService.IsConfigured())
                                {
                                    var url = await _cloudinaryService.UploadImageAsync(image, "gallery");
                                    gallery.ImageUrl = url;
                                }
                                else
                                {
                                    string date1 = DateTime.UtcNow.AddHours(1).ToString("ssfff");
                                    string name = date1 + "-" + image.FileName;
                                    string fileName = Path.GetFileName(name);
                                    gallery.ImageUrl = fileName;
                                    fileName = Path.Combine(Server.MapPath("~/Uploads/Gallery/"), fileName);
                                    image.SaveAs(fileName);
                                }
                                gallery.DateUpload = DateTime.UtcNow;
                                db.Galleries.Add(gallery);
                                await db.SaveChangesAsync();
                            }
                        }
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception c)
                {
                    TempData["error"] = c;
                }
            }
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList", gallery.SchoolId);
            return View(gallery);
        }

        // GET: Admin/Galleries/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = await db.Galleries.FindAsync(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList", gallery.SchoolId);

            return View(gallery);
        }

        // POST: Admin/Galleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,ImageUrl")] Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gallery).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "SchoolNameList", gallery.SchoolId);

            return View(gallery);
        }

        // GET: Admin/Galleries/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = await db.Galleries.FindAsync(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // POST: Admin/Galleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Gallery gallery = await db.Galleries.FindAsync(id);
                 //string[] files = Directory.GetFiles("~/Aq_Image/");
            if (System.IO.File.Exists(Server.MapPath("~/Uploads/Gallery/" + gallery.ImageUrl)))
            {
                System.IO.File.Delete(Server.MapPath("~/Uploads/Gallery/" + gallery.ImageUrl));
            }
            db.Galleries.Remove(gallery);
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
