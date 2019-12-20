using System;
using System.Collections.Generic;
using LearnositySDK.Request;
using LearnositySDK.Utils;

namespace LearnositySDKUnitTests.TestData
{
    public class TestRequest
    {
        private const string Secret = "74c5fd430cf1242a527f6223aebd42d30464be22";

        private const string BaseSecurity = "{\"consumer_key\": \"yis0TYCu7U9V4o7M\", \"domain\": \"localhost\", \"timestamp\": \"20140626-0528\"}";

        private static Dictionary<string, string> requests = new Dictionary<string, string>
        {
            { "assess", "{ \"items\": [ { \"content\": \"<span class=\\\"learnosity-response question-demoscience1234\\\"></span>\", \"response_ids\": [ \"demoscience1234\" ], \"workflow\": \"\", \"reference\": \"question-demoscience1\" }, { \"content\": \"<span class=\\\"learnosity-response question-demoscience5678\\\"></span>\", \"response_ids\": [ \"demoscience5678\" ], \"workflow\": \"\", \"reference\": \"question-demoscience2\" } ], \"ui_style\": \"horizontal\", \"name\": \"Demo (2 questions)\", \"state\": \"initial\", \"metadata\": {}, \"navigation\": { \"show_next\": true, \"toc\": true, \"show_submit\": true, \"show_save\": false, \"show_prev\": true, \"show_title\": true, \"show_intro\": true }, \"time\": { \"max_time\": 600, \"limit_type\": \"soft\", \"show_pause\": true, \"warning_time\": 60, \"show_time\": true }, \"configuration\": { \"onsubmit_redirect_url\": \"/assessment/\", \"onsave_redirect_url\": \"/assessment/\", \"idle_timeout\": true, \"questionsApiVersion\": \"v2\" }, \"questionsApiActivity\": { \"user_id\": \"$ANONYMIZED_USER_ID\", \"type\": \"submit_practice\", \"state\": \"initial\", \"id\": \"assessdemo\", \"name\": \"Assess API - Demo\", \"questions\": [ { \"response_id\": \"demoscience1234\", \"type\": \"sortlist\", \"description\": \"In this question, the student needs to sort the events, chronologically earliest to latest.\", \"list\": [ \"Russian Revolution\", \"Discovery of the Americas\", \"Storming of the Bastille\", \"Battle of Plataea\", \"Founding of Rome\", \"First Crusade\" ], \"instant_feedback\": true, \"feedback_attempts\": 2, \"validation\": { \"valid_response\": [ 4, 3, 5, 1, 2, 0 ], \"valid_score\": 1, \"partial_scoring\": true, \"penalty_score\": -1 } }, { \"response_id\": \"demoscience5678\", \"type\": \"highlight\", \"description\": \"The student needs to mark one of the flowers anthers in the image.\", \"img_src\": \"http://www.learnosity.com/static/img/flower.jpg\", \"line_color\": \"rgb(255, 20, 0)\", \"line_width\": \"4\" } ] }, \"type\": \"activity\" }" },
            { "author", "{ \"mode\": \"item_list\", \"config\": { \"item_list\": { \"item\": { \"status\": true } } }, \"user\": { \"id\": \"walterwhite\", \"firstname\": \"walter\", \"lastname\": \"white\" } }" },
            { "data", "{ \"limit\": 100 }" },
            { "items", "{ \"user_id\": \"$ANONYMIZED_USER_ID\", \"rendering_type\": \"assess\", \"name\": \"Items API demo - assess activity demo\", \"state\": \"initial\", \"activity_id\": \"items_assess_demo\", \"session_id\": \"demo_session_uuid\", \"type\": \"submit_practice\", \"config\": { \"configuration\": { \"responsive_regions\": true }, \"navigation\": { \"scrolling_indicator\": true }, \"regions\": \"main\", \"time\": { \"show_pause\": true, \"max_time\": 300 }, \"title\": \"ItemsAPI Assess Isolation Demo\", \"subtitle\": \"Testing Subtitle Text\" }, \"items\": [ \"Demo3\" ] }" },
            { "questions", "{ \"type\": \"local_practice\", \"state\": \"initial\", \"questions\": [ { \"response_id\": \"60005\", \"type\": \"association\", \"stimulus\": \"Match the cities to the parent nation.\", \"stimulus_list\": [ \"London\", \"Dublin\", \"Paris\", \"Sydney\" ], \"possible_responses\": [ \"Australia\", \"France\", \"Ireland\", \"England\" ], \"validation\": { \"valid_responses\": [ [ \"England\" ], [ \"Ireland\" ], [ \"France\" ], [ \"Australia\" ] ] } } ] }" },
            { "reports", "{ \"reports\": [ { \"id\": \"report-1\", \"type\": \"sessions-summary\", \"user_id\": \"$ANONYMIZED_USER_ID\", \"session_ids\": [ \"AC023456-2C73-44DC-82DA28894FCBC3BF\" ] } ] }" },
        };

        private string service;
        private JsonObject security;
        private JsonObject request;
        private string action;

        public static string getSecret()
        {
            return Secret;
        }

        public static string getBaseSecurity()
        {
            return BaseSecurity;
        }

        public static TestRequest getTestRequestFor(string service, string action = null)
        {
            JsonObject security = JsonObjectFactory.fromString(TestRequest.BaseSecurity);
            JsonObject request = JsonObjectFactory.fromString(TestRequest.requests.GetValueOrDefault(service));

            switch (service)
            {
                case "assess":
                case "questions":
                    security.set("user_id", "$ANONYMIZED_USER_ID");
                    break;

                case "items":
                    security.set("user_id", request.getString("user_id"));
                    break;

                default:
                    // no changes
                    break;
            }

            return new TestRequest(service, security, request, action);
        }



        private TestRequest(string service, JsonObject security, JsonObject request, string action = null)
        {
            this.service = service;
            this.security = security;
            this.request = request;
            this.action = action;
        }

        public Init getJsonInit()
        {
            return new Init(
                this.service,
                this.security,
                TestRequest.Secret,
                this.request,
                this.action
            );
        }

        public Init getStringInit()
        {
            return new Init(
                this.service,
                this.security.toJson(),
                TestRequest.Secret,
                this.request.toJson(),
                this.action
            );
        }
    }
}
