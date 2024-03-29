﻿using System;
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

            JsonObject pwd = new JsonObject();
            pwd.set("pwd", "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8");

            JsonObject config = new JsonObject();
            config.set("administration", pwd);

            JsonObject request = new JsonObject();
            request.set("user_id", Uuid.generate());
            request.set("session_id", Uuid.generate());

            request.set("activity_template_id", "demo-activity-1");
            request.set("activity_id", "my-demo-activity");
            request.set("name", "Demo Activity");
            request.set("course_id", "demo_yis0TYCu7U9V4o7M");
            request.set("config", config);

            // Instantiate Init class
            Init init = new Init(service, security, secret, request);

            // Call the generate() method to retrieve a JavaScript object
            ViewData["InitJSON"] = init.generate();
        }
    }
}
