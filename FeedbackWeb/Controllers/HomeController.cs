using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FeedbackWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.SpeechAPIKey = ConfigurationManager.AppSettings["SpeechAPIKey"];
            ViewBag.FeedbackAPIUrl = "https://" + ConfigurationManager.AppSettings["FunctionAppUrl"] + "/api/GatherFeedback";
            return View();
        }
    }
}