using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LearnositySDK.Request;
using LearnositySDK.Utils;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection;


namespace LearnosityDemo.Pages
{
    public class QuestionsAPIDemoModel : PageModel
    {
        public void OnGet()
        {

            // prepare all the params
            string service = "questions";
            string uuid = Uuid.generate();
            string courseId = "mycourse";
            JsonObject security = new JsonObject();
            security.set("consumer_key", LearnositySDK.Credentials.ConsumerKey);
            security.set("user_id", "abc");
            security.set("domain", LearnositySDK.Credentials.Domain);
            string secret = LearnositySDK.Credentials.ConsumerSecret;
            JsonObject request = JsonObjectFactory.fromString(QuestionsAPIDemoModel.requestJson(uuid, courseId));

            // Instantiate Init class
            Init init = new Init(service, security, secret, request);

            // Call the generate() method to retrieve a JavaScript object
            ViewData["InitJSON"] = init.generate();
        }

        private static string requestJson(string uuid, string courseId)
        {
            return string.Format(@"{{
                ""type"": ""local_practice"",
                ""sigver"": ""v2"",
                ""state"": ""initial"",
                ""id"": ""questionsapi-demo"",
                ""name"": ""Questions API Demo"",
                ""course_id"": ""{1}"",
                ""questions"": [
                    {{
                      ""type"": ""association"",
                      ""response_id"": ""60001"",
                      ""stimulus"": ""Match the cities to the parent nation."",
                      ""stimulus_list"": [""London"", ""Dublin"", ""Paris"", ""Sydney""],
                      ""possible_responses"": [""Australia"", ""France"", ""Ireland"", ""England""],
                        ""instant_feedback"" : true,
                        ""validation"": {{
                            ""valid_responses"" : [
                                 [""England""],[""Ireland""],[""France""],[""Australia""]
                            ]
                        }}
                    }},
  
                ]
            }}", uuid, courseId);
        }
    }
}
