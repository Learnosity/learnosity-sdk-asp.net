using System;
using System.Text;
using LearnositySDK.Request;
using LearnositySDK.Utils;

namespace LearnositySDK.Examples
{
    public class Questions
    {
        public static string Simple(out string uuid)
        {
            uuid = Uuid.generate();
            string courseId = "mycourse";

            string service = "questions";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);
            security.set("user_id", "demo_student");

            string secret = Credentials.ConsumerSecret;

            JsonObject request = JsonObjectFactory.fromString(Questions.requestJson(uuid, courseId));

            Init init = new Init(service, security, secret, request);
            return init.generate();
        }

        private static string requestJson(string uuid, string courseId)
        {
            return string.Format(@"{{
                ""type"": ""local_practice"",
                ""state"": ""initial"",
                ""id"": ""questionsapi-demo"",
                ""name"": ""Questions API Demo"",
                ""course_id"": ""{1}"",
                ""questions"": [
                    {{
                        ""type"": ""formula"",
                        ""response_id"": ""demoformula1_{0}"",
                        ""description"": ""Enter a math formula.""
                    }},

                    {{
                        ""type"": ""formula"",
                        ""response_id"": ""demoformula2_{0}"",
                        ""description"": ""Enter any expression that evaluates to x."",
                        ""instant_feedback"" : true,
                        ""validation"": {{
                            ""valid_responses"" : [
                                [{{ ""method"": ""equivSymbolic"", ""value"": ""x"" }}]
                            ]
                        }}
                    }},

                    {{
                        ""type"": ""formula"",
                        ""response_id"": ""demoformula3_{0}"",
                        ""description"": ""Complete the quadratic equation."",
                        ""template"": ""\\\\frac{{-b\\\\pm\\\\sqrt{{b^2-4ac}}}}{{{{{{response}}}}}}""
                    }},

                    {{
                        ""type"": ""formula"",
                        ""response_id"": ""demoformula4_{0}"",
                        ""description"": ""Enter some symbols using the custom math toolbar."",
                        ""symbols"": [{{
                            ""symbol"": ""\\\\Sigma"",
                            ""group"": 1,
                            ""title"": ""My custom title for uppercase sigma""
                        }}, {{
                            ""symbol"": ""\\\\sigma"",
                            ""group"": 1,
                            ""title"": ""My custom title for lowercase sigma""
                        }}, {{
                            ""symbol"": ""\\\\sqrt"",
                            ""group"": 2
                        }}, {{
                            ""symbol"": ""\\\\frac"",
                            ""group"": 2
                        }}, {{
                            ""symbol"": ""^"",
                            ""group"": 2
                        }}, {{
                            ""symbol"": ""\\\\cap"",
                            ""group"": 3
                        }}, {{
                            ""symbol"": ""\\\\cup"",
                            ""group"": 3
                        }}, {{
                            ""symbol"": ""\\\\subset"",
                            ""group"": 3
                        }}, {{
                            ""symbol"": ""\\\\supset"",
                            ""group"": 3
                        }}, {{
                            ""symbol"": ""\\\\in"",
                            ""group"": 3
                        }}, {{
                            ""symbol"": ""\\\\ni"",
                            ""group"": 3
                        }}]
                    }}
                ]
            }}", uuid, courseId);
        }
    }
}
