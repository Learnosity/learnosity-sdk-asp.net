using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

            if (type == "object")
            {
                JObject jObject = JObject.Parse(JSON);
                return JsonObjectFactory.fromJObject(jObject);
            }
            else if (type == "array")
            {
                JArray jArray = JArray.Parse(JSON);
                return JsonObjectFactory.fromJArray(jArray);
            }
            else
            {
                throw new Exception("Type not recognized");
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
                if (recursive)
                {
                    string type1 = "";
                    string type2 = "";
                    Object temp1Object = obj1.get(key, ref type1);
                    Object temp2Object = obj2.get(key, ref type2);
                    
                    // recursive merge makes sense only when both items are associative arrays
                    // if not regular merge will be performed
                    if (
                        (type1 == "JsonObject" || type1 == "JsonArray") &&
                        (type2 == "JsonObject" || type2 == "JsonArray") )
                    {
                        JsonObject temp1 = (JsonObject)temp1Object;
                        JsonObject temp2 = (JsonObject)temp2Object;
                        int temp1Count = temp1.count();
                        int temp2Count = temp2.count();

                        if (temp1Count == 0 && temp2Count == 0)
                        {
                            type = "JsonObject";
                            temp = new JsonObject();
                        }
                        else if(temp1Count == 0)
                        {
                            type = type2;
                            temp = temp2;
                        }
                        else if(temp2Count == 0)
                        {
                            type = type1;
                            temp = temp1;
                        }
                        else if (type1 == "JsonArray" && type2 == "JsonArray")
                        {
                            type = "JsonArray";
                            temp = JsonObjectFactory.merge(temp1, temp2, overwrite, recursive);
                        }
                        else
                        {
                            type = "JsonObject";
                            temp = JsonObjectFactory.merge(temp1, temp2, overwrite, recursive);
                        }

                        newObj.set(type, key, temp);
                        continue;
                    }
                }
                
                if (overwrite && Tools.in_array(key, keys2))
                {
                    temp = obj2.get(key, ref type);
                }
                else if (Tools.in_array(key, keys1))
                {
                    temp = obj1.get(key, ref type);
                }
                else if (Tools.in_array(key, keys2))
                {
                    temp = obj2.get(key, ref type);
                }

                newObj.set(type, key, temp);
                continue;
            }

            return newObj;
        }
    }
}
