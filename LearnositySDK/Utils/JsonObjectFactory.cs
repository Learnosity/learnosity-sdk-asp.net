using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace LearnositySDK.Utils
{
    public class JsonObjectFactory
    {
        /// <summary>
        /// Loads JsonObject from JSON string
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public static JsonObject fromString(string JSON)
        {
            string type;

            if (!Json.isJson(JSON, out type) || type == "")
            {
                return null;
            }

            using (var sr = new StringReader(JSON))
            using (var jr = new JsonTextReader(sr) { DateParseHandling = DateParseHandling.None })
            {
                JToken parsed = JToken.ReadFrom(jr);
                switch (parsed.Type)
                {
                    case JTokenType.Object:
                        return JsonObjectFactory.fromJObject((JObject)parsed);
                    case JTokenType.Array:
                        return JsonObjectFactory.fromJArray((JArray)parsed);
                    default:
                        throw new Exception("Currently we don't accept single values, only objects and arrays");
                }
            }
        }

        /// <summary>
        /// Converts JObject to JsonObject
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public static JsonObject fromJObject(JObject jObject)
        {
            JsonObject jsonObject = new JsonObject();

            foreach (KeyValuePair<string, JToken> pair in jObject)
            {
                jsonObject = JsonObjectFactory.fromJToken(jsonObject, pair.Key, pair.Value);
            }

            return jsonObject;
        }

        /// <summary>
        /// Converts JArray to JsonObject
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public static JsonObject fromJArray(JArray jArray)
        {
            JsonObject jsonObject = new JsonObject(true);

            int index = 0;

            foreach (JToken item in (IEnumerable<JToken>)jArray)
            {
                jsonObject = JsonObjectFactory.fromJToken(jsonObject, index.ToString(), item);
                index++;
            }

            return jsonObject;
        }

        /// <summary>
        /// Converts JToken to JsonObject
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public static JsonObject fromJToken(JsonObject jsonObject, string key, JToken item)
        {
            switch (item.Type)
            {
                case JTokenType.Array:
                    jsonObject.set("JsonArray", key, JsonObjectFactory.fromJArray((JArray)item));
                    break;
                case JTokenType.Boolean:
                    jsonObject.set(key, (bool)item);
                    break;
                case JTokenType.Integer:
                    jsonObject.set(key, (int)item);
                    break;
                case JTokenType.Float:
                    jsonObject.set(key, (float)item);
                    break;
                case JTokenType.String:
                    jsonObject.set(key, (string)item);
                    break;
                case JTokenType.Object:
                    jsonObject.set("JsonObject", key, JsonObjectFactory.fromJObject((JObject)item));
                    break;
                case JTokenType.Null:
                    jsonObject.set("NULL", key, item);
                    break;
                default:
                    break;
            }

            return jsonObject;
        }

        private static JsonObject mergeArrays(JsonObject arr1, JsonObject arr2)
        {
            JsonObject newObj = new JsonObject(true);
            string[] keys1 = arr1.getKeys();
            string[] keys2 = arr2.getKeys();
            string type = "";

            foreach (string key in keys1)
            {
                Object value = arr1.get(key, ref type);
                newObj.set(value, type);
            }

            foreach (string key in keys2)
            {
                Object value = arr2.get(key, ref type);
                newObj.set(value, type);
            }

            return newObj;
        }

        /// <summary>
        /// Merges two JsonObjects into one.
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="overwrite">whether second should overwrite first</param>
        /// <param name="recursive">whether merge should be recursive for the same keys</param>
        /// <returns></returns>
        public static JsonObject merge(JsonObject obj1, JsonObject obj2, bool overwrite = false, bool recursive = false)
        {
            JsonObject newObj = null;

            // for arrays we just insert elements from the second array to the first one and return a new array
            if (obj1.isArray() && obj2.isArray())
            {
                newObj = JsonObjectFactory.mergeArrays(obj1, obj2);
                return newObj;
            }

            newObj = new JsonObject();

            string[] keys1 = obj1.getKeys();
            string[] keys2 = obj2.getKeys();
            string[] keys = keys1.Union(keys2).ToArray();

            Object temp = null;
            string type = "";

            foreach (string key in keys)
            {
                string type1 = "";
                string type2 = "";
                Object temp1Object = obj1.get(key, ref type1);
                Object temp2Object = obj2.get(key, ref type2);

                // recursive merging for objects only
                if (recursive && (type1 == "JsonObject" && type2 == "JsonObject"))
                {
                    JsonObject temp1 = (JsonObject)temp1Object;
                    JsonObject temp2 = (JsonObject)temp2Object;
                    type = "JsonObject";
                    temp = JsonObjectFactory.merge(temp1, temp2, overwrite, recursive);
                }
                // special treatment for arrays
                else if (type1 == "JsonArray" && type2 == "JsonArray")
                {
                    JsonObject temp1 = (JsonObject)temp1Object;
                    JsonObject temp2 = (JsonObject)temp2Object;
                    type = "JsonArray";
                    temp = JsonObjectFactory.merge(temp1, temp2);
                }
                // for the rest we simple insert the value depending on overwrite parameter
                // overwrite is enabled and value is in the second object
                else if (overwrite && Tools.in_array(key, keys2))
                {
                    temp = obj2.get(key, ref type);
                }
                // value is in the first object
                else if (Tools.in_array(key, keys1))
                {
                    temp = obj1.get(key, ref type);
                }
                // value is in the second object only
                else if (Tools.in_array(key, keys2))
                {
                    temp = obj2.get(key, ref type);
                }

                if (type == "JsonObject" || type == "JsonArray")
                {
                    temp = ((JsonObject)temp).Clone();
                }

                newObj.set(type, key, temp);
                continue;
            }

            return newObj;
        }

        /// <summary>
        /// Checks whether two JSON strings are equal
        /// </summary>
        /// <param name="JSON1">first JSON string to compare</param>
        /// <param name="JSON2">second JSON string to compare</param>
        /// <returns></returns>
        public static bool JSONEquality(string JSON1, string JSON2)
        {
            JToken jo1 = JToken.Parse(JSON1);
            JToken jo2 = JToken.Parse(JSON2);
            return Newtonsoft.Json.Linq.JToken.DeepEquals(jo1, jo2);
        }

        /// <summary>
        /// Checks whether two JsonObjects are equal
        /// </summary>
        /// <param name="JSON1">first JsonObject to compare</param>
        /// <param name="JSON2">second JsonObject to compare</param>
        /// <returns></returns>
        public static bool JSONEquality(JsonObject JSON1, JsonObject JSON2)
        {
            return JSONEquality(JSON1.toJson(), JSON2.toJson());
        }

        /// <summary>
        /// Checks whether two JsonObjects are equal
        /// </summary>
        /// <param name="JSON1">first JsonObject to compare</param>
        /// <param name="JSON2">second JsonObject to compare</param>
        /// <returns></returns>
        public static bool JSONEquality(JsonObject JSON1, string JSON2)
        {
            return JSONEquality(JSON1.toJson(), JSON2);
        }
    }
}
