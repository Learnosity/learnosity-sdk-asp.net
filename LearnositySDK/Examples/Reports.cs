using System;
using System.Text;
using LearnositySDK.Request;
using LearnositySDK.Utils;

namespace LearnositySDK.Examples
{
    public class Reports
    {
        public static string Simple()
        {
            string service = "reports";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);

            string secret = Credentials.ConsumerSecret;

            JsonObject session_ids = new JsonObject(true);
            session_ids.set("AC023456-2C73-44DC-82DA28894FCBC3BF");

            JsonObject report = new JsonObject();
            report.set("id", "report-1");
            report.set("type", "sessions-summary");
            report.set("user_id", "brianmoser");
            report.set("session_ids", session_ids);

            JsonObject reports = new JsonObject(true);
            reports.set(report);

            JsonObject request = new JsonObject();
            request.set("reports", reports);

            Init init = new Init(service, security, secret, request);
            return init.generate();
        }
    }
}
