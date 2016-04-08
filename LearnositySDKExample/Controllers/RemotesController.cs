using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnositySDKExample.Controllers
{
    public class RemotesController : Controller
    {
        public ActionResult Data()
        {
            string JSON;
            ViewBag.heading = "Data API";
            ViewBag.description = "<p>Use the Data API to retrieve or update content in the Learnosity Assessment Platform.</p>";
            ViewBag.RESULT = LearnositySDK.Examples.Data.Simple(out JSON);
            ViewBag.JSON = JSON;
            return View();
        }

        public ActionResult DataApi()
        {
            ViewBag.heading = "Data API";
            ViewBag.description = "<p>Use the Data API to retrieve or update content in the Learnosity Assessment Platform.</p>";
            ViewBag.RESULT = LearnositySDK.Examples.Data.DataApi();
            return View();
        }

        public ActionResult DataApiRecursive(int maxIterations = -1, bool noCallback = false)
        {
            ViewBag.heading = "Data API";
            ViewBag.description = "<p>Use the Data API to retrieve or update content in the Learnosity Assessment Platform.</p>";
            ViewBag.RESULT = LearnositySDK.Examples.Data.DataApiRecursive(maxIterations, noCallback);
            return View();
        }

        public ActionResult Schemas()
        {
            string URL;
            ViewBag.heading = "Schemas API";
            ViewBag.description = "<p>Retrieve JSON schema information for question types, attributes and templates.</p>";
            ViewBag.RESULT = LearnositySDK.Examples.Schemas.Simple(out URL);
            ViewBag.URL = URL;
            return View();
        }
    }
}
