using System;
using System.Text;
using LearnositySDK.Request;
using LearnositySDK.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LearnosityDemo.Pages
{
    public class AuthorAideAPIDemoModel : PageModel
    {
        public void OnGet(string mode)
        {
            Init init = initializeAide();
            ViewData["InitJSON"] = init.generate();
        }

        private static Init initializeAide()
        {
            string service = "authoraide";

            JsonObject security = new JsonObject();
            security.set("consumer_key", LearnositySDK.Credentials.ConsumerKey);
            security.set("domain", LearnositySDK.Credentials.Domain);

            string secret = LearnositySDK.Credentials.ConsumerSecret;

            JsonObject request = new JsonObject();

            JsonObject user = new JsonObject();
            user.set("id", "brianmoser");
            user.set("firstname", "Test");
            user.set("lastname", "Test");
            user.set("email", "test@test.com");
            request.set("user", user);

            return new Init(service, security, secret, request);
        }
    }
}
