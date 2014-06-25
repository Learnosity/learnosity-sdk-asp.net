using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LearnositySDK.Utils
{
    class Json
    {
        protected static string message = "";

        /// <summary>
        /// Returns last error, after failure
        /// </summary>
        /// <returns></returns>
        public static string checkError()
        {
            return Json.message;
        }

        /// <summary>
        /// Returns JSON representation of Object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string encode(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Returns JSON representation of JsonObject
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string encode(JsonObject obj)
        {
            return obj.toJson();
        }

        /// <summary>
        /// Checks whether provided string is a valid JSON
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type">type will be set on success: array or object, or empty string on failure</param>
        /// <returns></returns>
        public static bool isJson(string str, out string type)
        {
            Json.message = "";

            try
            {
                JObject obj = JObject.Parse(str);
                type = "object";
                return true;
            }
            catch (Exception e1)
            {
                try
                {
                    JArray arr = JArray.Parse(str);
                    type = "array";
                    return true;
                }
                catch (Exception e2)
                {
                    Json.message = "JObject.Parse: " + e1.Message + " JArray.Parse: " + e2.Message;
                    type = "";
                    return false;
                }
            }
        }
    }
}
