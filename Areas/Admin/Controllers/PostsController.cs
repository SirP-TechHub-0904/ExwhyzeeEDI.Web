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

namespace ExwhyzeeEDI.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ExwhyzeeEDI.Web.DataServices.CloudinaryService.ICloudinaryService _cloudinaryService;

        public PostsController(ApplicationDbContext context, ExwhyzeeEDI.Web.DataServices.CloudinaryService.ICloudinaryService cloudinaryService)
        {
            db = context;
            _cloudinaryService = cloudinaryService;
        }

        // GET: Admin/Posts
        public async Task<ActionResult> Index()
        {
            var posts = db.Posts.Include(p => p.Category);
            return View(await posts.ToListAsync());
        }

        // GET: Admin/Posts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Admin/Posts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName");
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Post post, HttpPostedFileBase upload)
        {
            post.DatePosted = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                if (upload != null && upload.Length > 0)
                {
                    if (_cloudinaryService.IsConfigured())
                    {
                        var url = await _cloudinaryService.UploadImageAsync(upload, "posts");
                        post.ImageUrl = url;
                    }
                    else
                    {
                        string date1 = DateTime.UtcNow.AddHours(1).ToString("ssfff");
                        string name = date1 + "-" + upload.FileName;
                        string fileName = Path.GetFileName(name);
                        post.ImageUrl = fileName;
                        fileName = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                        upload.SaveAs(fileName);
                    }
                }
                
                db.Posts.Add(post);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var allErrors = ModelState.Values.SelectMany(v => v.Errors);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", post.CategoryId);
            return View(post);
        }

        // GET: Admin/Posts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", post.CategoryId);
            return View(post);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Post post, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.Length > 0)
                {
                    if (_cloudinaryService.IsConfigured())
                    {
                        var url = await _cloudinaryService.UploadImageAsync(upload, "posts");
                        post.ImageUrl = url;
                    }
                    else
                    {
                        string date1 = DateTime.UtcNow.AddHours(1).ToString("ssfff");
                        string name = date1 + "-" + upload.FileName;
                        string fileName = Path.GetFileName(name);
                        post.ImageUrl = fileName;
                        fileName = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                        upload.SaveAs(fileName);
                    }
                }
                post.DatePosted = DateTime.UtcNow;
                db.Entry(post).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", post.CategoryId);
            return View(post);
        }

        // GET: Admin/Posts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Post post = await db.Posts.FindAsync(id);
            db.Posts.Remove(post);
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
