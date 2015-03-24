using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnositySDKExample.Controllers
{
    public class JSController : Controller
    {
        public ActionResult Assess()
        {
            ViewBag.heading = "Assess API";
            ViewBag.JSON = LearnositySDK.Examples.Assess.Simple();
            return View();
        }

        public ActionResult Items()
        {
            ViewBag.heading = "Items API";
            ViewBag.description = "<p>Prepare your security, assessment configuration, item and question information and we'll do the rest.</p>";
            ViewBag.JSON = LearnositySDK.Examples.Items.Simple();
            return View();
        }

        public ActionResult Author()
        {
            ViewBag.heading = "Author API";
            ViewBag.description = "<p>Retrieve content from the Learnosity ItemBank to embed in your own authoring environment.</p>";
            ViewBag.JSON = LearnositySDK.Examples.Author.Simple();
            return View();
        }

        public ActionResult Questions()
        {
            ViewBag.heading = "Questions API";
            ViewBag.description = "<p>Prepare your question JSON and security credentials, we'll do the rest!</p>";
            string uuid = "";
            ViewBag.JSON = LearnositySDK.Examples.Questions.Simple(out uuid);
            ViewBag.UUID = uuid;
            return View();
        }

        public ActionResult Reports()
        {
            ViewBag.heading = "Reports API";
            ViewBag.description = "<p>Prepare your report configuration and security credentials, we'll do the rest!</p>";
            ViewBag.JSON = LearnositySDK.Examples.Reports.Simple();
            return View();
        }
    }
}
