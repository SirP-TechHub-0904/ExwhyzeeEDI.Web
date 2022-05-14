using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Dtos;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExwhyzeeEDI.Web.Controllers
{
    public class SecuredController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<ActionResult> BVNCheck(string phone)
        {
            try
            {
                string apiurl = $"https://api.flutterwave.com/v3/kyc/bvns/" + phone;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                var client = new RestClient(apiurl);
                var request = new RestRequest(Method.GET);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "FLWSECK-3ebc176be7413ec684592804c5cd98b7-X");


                IRestResponse response = client.Execute(request);
                var contents = response.Content.ToString();
                //var json = await response.Content.ReadAsStringAsync();
                string outp = "";
                var mainresponse = JsonConvert.DeserializeObject<BVN_Response>(contents);
                if (mainresponse.status == "success")
                {
                    outp = "BVN: " + mainresponse.data.bvn + "- Name: " + mainresponse.data.first_name + " " + mainresponse.data.middle_name + " " + mainresponse.data.last_name;
                }
                else
                {
                    outp = "Name not found";
                }

                return Json(outp, JsonRequestBehavior.AllowGet);

            }
            catch (Exception g)
            {
                return Json("not found", JsonRequestBehavior.AllowGet);

            }
        }
        
  public ActionResult AGSMEIS()
        {

            return View();
        }
        public ActionResult ProgramHistory()
        {

            return View();
        }


        public ActionResult JDPC()
        {

            return View();
        }
        public ActionResult About()
        {

            return View();
        }

        public ActionResult LeadershipTeam()
        {

            return View();
        }

        public ActionResult Partners()
        {

            return View();
        }

        public ActionResult Testimonies()
        {

            return View();
        }

        public ActionResult FAQs()
        {

            return View();
        }

        public ActionResult AwardAndRegonition()
        {

            return View();
        }

        public ActionResult AboutEdi()
        {

            return View();
        }


        public ActionResult Contact()
        {

            return View();
        }


        public ActionResult _LatestNews()
        {
            var post = db.Posts.OrderBy(x => x.SortOrder).Take(3).ToList();
            return PartialView(post);
        }

        public ActionResult _MorePost()
        {
            var post = db.Posts.OrderBy(x => x.SortOrder).Skip(2).Take(4).ToList();
            return PartialView(post);
        }

        public ActionResult _FooterPosts()
        {
            var post = db.Posts.OrderBy(x => x.DatePosted).ToList();
            if (post.Count() > 6)
            {
                post = post.Skip(3).Take(3).ToList();
            }


            return PartialView(post.Take(3));
        }

        public ActionResult _Gallery()
        {
            var gallery = db.Galleries.OrderBy(x => x.SortOrder).ThenBy(x => x.DateUpload).ToList();
            return PartialView(gallery);
        }

        public ActionResult _Slider()
        {
            var slide = db.Sliders.OrderByDescending(x => x.SortOrder).ToList();
            return PartialView(slide);
        }

        public ActionResult Programone()
        {
            return View();
        }
    }
}