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
using ExwhyzeeEDI.Web.Models.Dtos;

namespace ExwhyzeeEDI.Web.Areas.EDI.Controllers
{
    public class ProfileAccountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: EDI/ProfileAccount
        //public async Task<ActionResult> Index()
        //{

        //    return View();
        //}

        //// GET: EDI/ProfileAccount/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProfileDto profileDto = await db.ProfileDtoes.FindAsync(id);
        //    if (profileDto == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(profileDto);
        //}

        //// GET: EDI/ProfileAccount/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: EDI/ProfileAccount/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,Title,Surname,FirstName,OtherName,Gender,Dateofbirth,MaritalStatus,ModeOfIdentification,IdentificationNumber,ContactAddress,StateofOrigin,LocalGovernmentArea,CurrentOccupation,AreaOfInterest,SpecificAreaOfInterest,Username,Email,PhoneNumber")] ProfileDto profileDto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ProfileDtoes.Add(profileDto);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(profileDto);
        //}

        //// GET: EDI/ProfileAccount/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProfileDto profileDto = await db.ProfileDtoes.FindAsync(id);
        //    if (profileDto == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(profileDto);
        //}

        //// POST: EDI/ProfileAccount/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Surname,FirstName,OtherName,Gender,Dateofbirth,MaritalStatus,ModeOfIdentification,IdentificationNumber,ContactAddress,StateofOrigin,LocalGovernmentArea,CurrentOccupation,AreaOfInterest,SpecificAreaOfInterest,Username,Email,PhoneNumber")] ProfileDto profileDto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(profileDto).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(profileDto);
        //}

        //// GET: EDI/ProfileAccount/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProfileDto profileDto = await db.ProfileDtoes.FindAsync(id);
        //    if (profileDto == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(profileDto);
        //}

        //// POST: EDI/ProfileAccount/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    ProfileDto profileDto = await db.ProfileDtoes.FindAsync(id);
        //    db.ProfileDtoes.Remove(profileDto);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
