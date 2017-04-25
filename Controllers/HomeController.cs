using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CrowdHeyna.Controllers
{
    [Authorize]
   // [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["username"] = User.Identity.GetUserName();
            return View();
        }
    }
}
