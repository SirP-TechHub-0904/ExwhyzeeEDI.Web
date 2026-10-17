using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExwhyzeeEDI.Web.Controllers
{
    public class YoungMindsController : Controller
    {
        // GET: YoungMinds
        public ActionResult Index()
        {
            return View();
        }
    }
}