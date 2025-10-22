using System;
using LearnositySDK;
using LearnositySDK.Request;
using LearnositySDK.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LearnosityDemo.Pages
{
    public class DataAPIDemoModel : PageModel
    {
        private readonly ILogger<DataAPIDemoModel> _logger;

        public string ResponseBody { get; set; }
        public string ConsumerHeader { get; set; }
        public string ActionHeader { get; set; }
        public string Endpoint { get; set; }
        public string Action { get; set; }
        public string HttpStatusCode { get; set; }

        public DataAPIDemoModel(ILogger<DataAPIDemoModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // Set up the Data API request
            string endpoint = "https://data.learnosity.com/v1/itembank/items";
            string action = "get";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);

            JsonObject request = new JsonObject();
            request.set("limit", 3);

            // Make the Data API request
            DataApi da = new DataApi();
            Remote r = da.request(endpoint, security, Credentials.ConsumerSecret, request, action);

            // Get the response
            ResponseBody = r.getBody();
            HttpStatusCode = r.getStatusCode();
            Endpoint = endpoint;
            Action = action;

            // Get the metadata headers that were sent
            var headers = r.getRequestHeaders();
            if (headers != null)
            {
                ConsumerHeader = headers.Get("X-Learnosity-Consumer");
                ActionHeader = headers.Get("X-Learnosity-Action");

                // Log the metadata headers
                _logger.LogInformation("=== Data API Request Metadata ===");
                _logger.LogInformation($"Endpoint: {endpoint}");
                _logger.LogInformation($"Action: {action}");
                _logger.LogInformation($"X-Learnosity-Consumer: {ConsumerHeader}");
                _logger.LogInformation($"X-Learnosity-Action: {ActionHeader}");
                _logger.LogInformation($"Status Code: {HttpStatusCode}");
                _logger.LogInformation("=================================");
            }
        }
    }
}

