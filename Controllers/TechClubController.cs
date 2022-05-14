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
using static ExwhyzeeEDI.Web.ApplicationSignInManager;
using Microsoft.AspNet.Identity.Owin;
using ExwhyzeeEDI.Web.Models.Dtos;
using RestSharp;
using Newtonsoft.Json;

namespace ExwhyzeeEDI.Web.Controllers
{
    public class TechClubController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationRoleManager _roleManager;

        public TechClubController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;

            RoleManager = roleManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }




        // GET: TechClub
        public async Task<ActionResult> Index()
        {
            return View(await db.YoungMinds.ToListAsync());
        }
        public async Task<ActionResult> List()
        {
            return View(await db.YoungMinds.ToListAsync());
        }

        // GET: TechClub/Details/5
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

        public async Task<ActionResult> Verify()
        {
           
            return View();
        }

       
        // GET: TechClub/Create
        public ActionResult Join()
        {
            ViewBag.StateofOrigin = new SelectList(db.States.OrderBy(x => x.StateName), "StateName", "StateName");

            return View();
        }

        // POST: TechClub/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Join(RegistrationDto data)
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser
                {
                    UserName = data.Email,
                    Email = data.Email,
                    PhoneNumber = data.Phone
                };
                var result = await UserManager.CreateAsync(user, data.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "XYZTECH");
                    YoungMind usermain = new YoungMind();

                    usermain.Title = data.Title;
                    usermain.Surname = data.Surname;
                    usermain.FirstName = data.FirstName;
                    usermain.OtherName = data.OtherName;
                    usermain.Gender = data.Gender;
                    usermain.Dateofbirth = data.Dateofbirth;
                    usermain.DateRegistered = DateTime.UtcNow.AddHours(1);
                    usermain.MaritalStatus = data.MaritalStatus;
                    usermain.Religion = data.Religion;
                    usermain.PermanentHomeAddress = data.PermanentHomeAddress;
                    usermain.ModeOfIdentification = data.ModeOfIdentification;
                    usermain.IdentificationNumber = data.IdentificationNumber;
                    usermain.ContactAddress = data.ContactAddress;
                    usermain.StateofOrigin = data.StateofOrigin;
                    usermain.LocalGovernmentArea = data.LocalGovernmentArea;

                    usermain.AreYouInAbyVocationNow = data.AreYouInAbyVocationNow;

                    usermain.Hobby = data.Hobby;
                    usermain.Skills = data.Skills;
                    usermain.UserId = user.Id;
                    usermain.WriteAboutYourself = data.WriteAboutYourself;
                    usermain.YourBiggestAchievement = data.YourBiggestAchievement;
                    usermain.WhatIsYourPassion = data.WhatIsYourPassion;
                    usermain.DefineYourPersonality = data.DefineYourPersonality;
                    usermain.HowDoYouSeeYourSelfIn10Years = data.HowDoYouSeeYourSelfIn10Years;

                    usermain.WhatAreYourLimitationsTowardsYourGoal = data.WhatAreYourLimitationsTowardsYourGoal;
                    usermain.HowDoYouThinkYouCanOvercomeYourLimitations = data.HowDoYouThinkYouCanOvercomeYourLimitations;

                    usermain.WhatDoYouWantToChangeInTheSociety = data.WhatDoYouWantToChangeInTheSociety;

                    usermain.PreviouseProjectAttemptedResearchPublicityInvention = data.PreviouseProjectAttemptedResearchPublicityInvention;

                    usermain.WhatIsYourGreatestGoal = data.WhatIsYourGreatestGoal;
                    usermain.AreYouReadyToMakeChangesToYourEnvironment = data.AreYouReadyToMakeChangesToYourEnvironment;
                    usermain.IfYes_HowCanYouMakeChanges = data.IfYes_HowCanYouMakeChanges;

                    usermain.Referee = data.Referee;
                    usermain.KnowAFriendThatYouThinkWillMakeAGreatChangeInHisEnvironment = data.KnowAFriendThatYouThinkWillMakeAGreatChangeInHisEnvironment;
                    usermain.IdeaAndSuggestion = data.IdeaAndSuggestion;
                    usermain.SocialMedia = data.SocialMedia;


                    db.YoungMinds.Add(usermain);
                    await db.SaveChangesAsync();

                    var userReg = await db.YoungMinds.FirstOrDefaultAsync(x => x.UserId == user.Id);
                    string numberid = userReg.Id.ToString("D3");
                    //staffReg.StaffRegistrationId = setting.SchoolInitials + "/STAFF/00" + staffReg.Id;
                    userReg.RegistrationNumber =  "XYZTECH/ID/" + numberid;
                    db.Entry(userReg).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Details", new { id = userReg.Id });
                   
                }
                var errors = result.Errors;
                var message = string.Join(", ", errors);

            }
            ViewBag.StateofOrigin = new SelectList(db.States.OrderBy(x => x.StateName), "StateName", "StateName");

            return View(data);
        }

        // GET: TechClub/Edit/5
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

        // POST: TechClub/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,RegistrationNumber,Title,Surname,FirstName,OtherName,Gender,Dateofbirth,DateRegistered,MaritalStatus,Religion,PermanentHomeAddress,ModeOfIdentification,IdentificationNumber,ContactAddress,StateofOrigin,LocalGovernmentArea,AreYouInAbyVocationNow,UserId,WhatAreaAreYouInterestedIn,WhatTrackAreYouInterestedIn,CanYouDoMoreWithLessResources,TellUsHowYouWouldGoAboutThis,WhatIsYourCurrentExperienceLevelInInformationTechnology,AreYouReadyToMakeChangesToYourEnvironment,IfYes_HowCanYouMakeChanges,Referee,KnowAFriendThatYouThinkWillMakeAGreatChangeInHisEnvironment,IdeaAndSuggestion,PassportUrl")] YoungMind youngMind)
        {
            if (ModelState.IsValid)
            {
                db.Entry(youngMind).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(youngMind);
        }

        // GET: TechClub/Delete/5
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

        // POST: TechClub/Delete/5
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
