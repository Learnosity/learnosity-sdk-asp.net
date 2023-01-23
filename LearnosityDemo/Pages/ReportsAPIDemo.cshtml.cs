using System;
using System.Text;
using LearnositySDK;
using LearnositySDK.Request;
using LearnositySDK.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace LearnosityDemo.Pages
{
    public class ReportsAPIDemoModel : PageModel
    {
        public void OnGet(string mode)
        {
            string service = "reports";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);

            string secret = Credentials.ConsumerSecret;

            JsonObject report = new JsonObject();
            report.set("id", "session-detail");
            report.set("type", "session-detail-by-item");
            report.set("user_id", "906d564c-39d4-44ba-8ddc-2d44066e2ba9");
            report.set("session_id", "906d564c-39d4-44ba-8ddc-2d44066e2ba9");

            JsonObject reports = new JsonObject(true);
            reports.set(report);

            JsonObject request = new JsonObject();
            request.set("reports", reports);

            Init init = new Init(service, security, secret, request);
            ViewData["initJSON"] = init.generate();
        }   
    }
}
