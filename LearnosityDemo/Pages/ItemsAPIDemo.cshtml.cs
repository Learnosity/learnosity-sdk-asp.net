using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LearnositySDK.Request;
using LearnositySDK.Utils;

namespace LearnosityDemo.Pages
{
    public class ItemsAPIDemoModel : PageModel
    {
        public void OnGet()
        {
            // prepare all the params
            string service = "items";

            JsonObject security = new JsonObject();
            security.set("consumer_key", LearnositySDK.Credentials.ConsumerKey);
            security.set("domain", LearnositySDK.Credentials.Domain);
            security.set("user_id", Uuid.generate());
            string secret = LearnositySDK.Credentials.ConsumerSecret;

            JsonObject request = new JsonObject();
            request.set("user_id", Uuid.generate());
            request.set("activity_template_id", "quickstart_examples_activity_template_001");
            request.set("session_id", Uuid.generate());
            request.set("activity_id", "quickstart_examples_activity_001");
            request.set("rendering_type", "assess");
            request.set("type", "submit_practice");
            request.set("name", "Items API Quickstart");
            request.set("state", "initial");

            // Instantiate Init class
            Init init = new Init(service, security, secret, request);

            // Call the generate() method to retrieve a JavaScript object
            ViewData["InitJSON"] = init.generate();
        }
    }
}
