using System;
using System.Text;
using LearnositySDK;
using LearnositySDK.Examples;
using LearnositySDK.Request;
using LearnositySDK.Utils;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LearnosityDemo.Pages
{
    public class QuestioneditorAPIDemoModel : PageModel
    {
        public void OnGet(string mode)
        {
            JsonObject request = new JsonObject();
            JsonObject consumer_key = new JsonObject();
            consumer_key.set("consumer_key", LearnositySDK.Credentials.ConsumerKey);
            request.set("configuration", consumer_key);
            request.set("widget_conversion", true);

            JsonObject ui = new JsonObject();
            ui.set("search_field", true);

            JsonObject layout = new JsonObject();
            layout.set("global_template", "edit_preview");
            layout.set("mode", "advanced");
            ui.set("layout", layout);
            request.set("ui", ui);
            string service = "questions";

            JsonObject security = new JsonObject();
            security.set("consumer_key", LearnositySDK.Credentials.ConsumerKey);
            security.set("user_id", "abc");
            security.set("domain", LearnositySDK.Credentials.Domain);

            string secret = LearnositySDK.Credentials.ConsumerSecret;

            Init init = new Init(service, security, secret, request);

            ViewData["InitJSON"] = init.generate();
        }

    }
}
