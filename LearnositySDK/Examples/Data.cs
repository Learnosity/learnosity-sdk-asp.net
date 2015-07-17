using System;
using System.Text;
using LearnositySDK.Request;
using LearnositySDK.Utils;
using System.Collections.Generic;

namespace LearnositySDK.Examples
{
    public class Data
    {
        /// <summary>
        /// Sets JSON parameters as well as result of the request
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public static string Simple(out string JSON)
        {
            string service = "data";
            string url = "https://data.learnosity.com/stable/sessions/responses";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);

            string secret = Credentials.ConsumerSecret;

            JsonObject request = new JsonObject();
            request.set("limit", 1000);
            
            string action = "get";

            Init i = new Init(service, security, secret, request, action);
            string parameters = i.generate();
            Remote remote = new Remote();
            Remote r = remote.post(url, parameters);

            JSON = parameters;
            return r.getBody();
        }

        /// <summary>
        /// Run the request using DataApi and returns result
        /// </summary>
        /// <returns></returns>
        public static string DataApi()
        {
            string url = "https://data.learnosity.com/stable/sessions/responses";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);

            string secret = Credentials.ConsumerSecret;

            JsonObject request = new JsonObject();
            request.set("limit", 1000);

            string action = "get";

            DataApi da = new DataApi();
            Remote r = da.request(url, security, secret, request, action);

            return r.getBody();
        }

        /// <summary>
        /// Run the request using recursive version of DataApi and returns result
        /// </summary>
        /// <returns></returns>
        public static string DataApiRecursive()
        {
            string url = "https://data.learnosity.com/stable/sessions/responses";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);

            string secret = Credentials.ConsumerSecret;

            JsonObject request = new JsonObject();
            request.set("limit", 1000);

            string action = "get";

            ProcessData callback = new ProcessData(Data.DataApiRecursiveCallback);

            DataApi da = new DataApi();
            JsonObject jo = da.requestRecursive(url, security, secret, request, action, callback);

            return jo.toJson();
        }

        /// <summary>
        /// Callback function with the same definition as defined delegate (ProcessData) in DataApi
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool DataApiRecursiveCallback(string data)
        {
            JsonObject jo = JsonObjectFactory.fromString(data);

            if (jo != null)
            {
                return true;
            }

            return false;
        }
    }
}
