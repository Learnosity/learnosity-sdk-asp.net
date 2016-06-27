using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace LearnositySDK.Utils
{
    public class JsonObject : ICloneable
    {
        private bool array;
        private int arrayIndex;
        private Dictionary<string, int> di;
        private Dictionary<string, float> df;
        private Dictionary<string, string> ds;
        private Dictionary<string, JsonObject> dj;
        private Dictionary<string, bool> db;
        private Dictionary<string, JsonObject> da; // arrays
        private Dictionary<string, JToken> dt; // NULL values
        private string[] types;

        public JsonObject(bool isArray = false)
        {
            this.array = isArray;
            this.arrayIndex = 0; // lastInsertedIndex
            this.di = new Dictionary<string, int>();
            this.df = new Dictionary<string, float>();
            this.ds = new Dictionary<string, string>();
            this.dj = new Dictionary<string, JsonObject>();
            this.db = new Dictionary<string, bool>();
            this.da = new Dictionary<string, JsonObject>();
            this.dt = new Dictionary<string, JToken>();
            this.types = new string[7] { "int", "string", "JsonObject", "bool", "JsonArray", "float", "NULL" };
        }

        public bool isArray()
        {
            return this.array;
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="value"></param>
        public void set(int value)
        {
            this.set(this.arrayIndex, value);
            this.arrayIndex++;
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="value"></param>
        public void set(float value)
        {
            this.set(this.arrayIndex, value);
            this.arrayIndex++;
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="value"></param>
        public void set(string value)
        {
            this.set(this.arrayIndex, value);
            this.arrayIndex++;
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="value"></param>
        public void set(JsonObject value)
        {
            this.set(this.arrayIndex, value);
            this.arrayIndex++;
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="value"></param>
        public void set(bool value)
        {
            this.set(this.arrayIndex, value);
            this.arrayIndex++;
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void set(JToken value)
        {
            this.set(this.arrayIndex, value);
            this.arrayIndex++;
        }

        /// <summary>
        /// Sets/adds NULL
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void setNull()
        {
            this.set(JToken.Parse("null"));
        }

        /// <summary>
        /// Sets/adds value with given type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public void set(Object value, string type)
        {
            this.set(type, this.arrayIndex.ToString(), value);
            this.arrayIndex++;
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void set(int key, int value)
        {
            this.set(key.ToString(), value);
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void set(int key, float value)
        {
            this.set(key.ToString(), value);
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void set(int key, string value)
        {
            this.set(key.ToString(), value);
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void set(int key, JsonObject value)
        {
            this.set(key.ToString(), value);
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void set(int key, bool value)
        {
            this.set(key.ToString(), value);
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void set(int key, JToken value)
        {
            this.set(key.ToString(), value);
        }

        /// <summary>
        /// Sets/adds NULL
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void setNull(int key)
        {
            this.set(key, JToken.Parse("null"));
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void set(string key, int value)
        {
            this.set("int", key, value);
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void set(string key, float value)
        {
            this.set("float", key, value);
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void set(string key, string value)
        {
            this.set("string", key, value);
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void set(string key, JsonObject value)
        {
            if (value.isArray())
            {
                this.set("JsonArray", key, value);
            }
            else
            {
                this.set("JsonObject", key, value);
            }
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void set(string key, JToken value)
        {
            this.set("NULL", key, value);
        }

        /// <summary>
        /// Sets/adds NULL
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void setNull(string key)
        {
            this.set(key, JToken.Parse("null"));
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="key"></param>
        public void remove(string key)
        {
            this.db.Remove(key);
            this.di.Remove(key);
            this.df.Remove(key);
            this.ds.Remove(key);
            this.dj.Remove(key);
            this.da.Remove(key);
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void set(string key, bool value)
        {
            this.set("bool", key, value);
        }

        /// <summary>
        /// Sets/adds the value
        /// </summary>
        /// <param name="type">check this.types for available types</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void set(string type, string key, Object value)
        {
            if (value == null)
            {
                return;
            }

            int n;
            bool isNumeric = int.TryParse(key, out n);
            if (isNumeric)
            {
                this.arrayIndex = n;
            }
            
            int index = Array.IndexOf(this.types, type);

            if (index < 0)
            {
                return;
            }

            this.remove(key);

            switch (index)
            {
                case 0: this.di.Add(key, (int)value);
                    break;
                case 1: this.ds.Add(key, (string)value);
                    break;
                case 2: this.dj.Add(key, (JsonObject)value);
                    break;
                case 3: this.db.Add(key, (bool)value);
                    break;
                case 4: this.da.Add(key, (JsonObject)value);
                    break;
                case 5: this.df.Add(key, (float)value);
                    break;
                case 6: this.dt.Add(key, (JToken)value);
                    break;
            }
        }

        /// <summary>
        /// Gets an item if it's string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string getString(string key)
        {
            string type = "";
            Object temp = this.get(key, ref type);
            
            if (type == "string")
            {
                return (string)temp;
            }

            return null;
        }

        /// <summary>
        /// Gets an item if it's JsonObject
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public JsonObject getJsonObject(string key)
        {
            string type = "";
            Object temp = this.get(key, ref type);

            if (type == "JsonObject" || type == "JsonArray")
            {
                return (JsonObject)temp;
            }

            return null;
        }

        /// <summary>
        /// Gets an item if it's bool
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool getBool(string key)
        {
            string type = "";
            Object temp = this.get(key, ref type);

            if (type == "bool")
            {
                return (bool)temp;
            }

            throw new Exception("`bool` is not nullable, so can't return `null`");
        }

        /// <summary>
        /// Gets an item if it's integer
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int getInt(string key)
        {
            string type = "";
            Object temp = this.get(key, ref type);

            if (type == "int")
            {
                return (int)temp;
            }
            
            throw new Exception("`int` is not nullable, so can't return `null`");
        }

        /// <summary>
        /// Gets an item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type">type will be set on success</param>
        /// <returns></returns>
        public Object get(int key, ref string type)
        {
            return this.get(key.ToString(), ref type);
        }

        /// <summary>
        /// Gets an item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type">type will be set on success</param>
        /// <returns></returns>
        public Object get(string key, ref string type)
        {
            if (this.di.ContainsKey(key))
            {
                type = "int";
                return this.di[key];
            }

            if (this.ds.ContainsKey(key))
            {
                type = "string";
                return this.ds[key];
            }

            if (this.dj.ContainsKey(key))
            {
                type = "JsonObject";
                return this.dj[key];
            }

            if (this.db.ContainsKey(key))
            {
                type = "bool";
                return this.db[key];
            }

            if (this.da.ContainsKey(key))
            {
                type = "JsonArray";
                return this.da[key];
            }

            if (this.df.ContainsKey(key))
            {
                type = "float";
                return this.df[key];
            }

            if (this.dt.ContainsKey(key))
            {
                type = "NULL";
                return this.dt[key];
            }

            type = "";
            return null;
        }

        /// <summary>
        /// Gets all the existing values
        /// </summary>
        /// <param name="includeInts">will convert int value to string</param>
        /// <param name="includeBools">will convert bool value to string</param>
        /// <param name="includeJsonObjects">will convert JsonObjects to JSON strings</param>
        /// <returns></returns>
        public string[] getValuesArray(bool includeInts = false, bool includeBools = false, bool includeJsonObjects = false)
        {
            List<string> stringsList = this.getValuesList(includeInts, includeBools);
            return stringsList.ToArray();
        }

        /// <summary>
        /// Gets all the existing values
        /// </summary>
        /// <param name="includeInts">will convert int value to string</param>
        /// <param name="includeBools">will convert bool value to string</param>
        /// <param name="includeJsonObjects">will convert JsonObjects to JSON strings</param>
        /// <returns></returns>
        public List<string> getValuesList(bool includeInts = false, bool includeBools = false, bool includeJsonObjects = false)
        {
            List<string> stringsList = new List<string>();

            foreach (KeyValuePair<string, string> pair in this.ds)
            {
                stringsList.Add(pair.Value);
            }

            if (includeInts)
            {
                foreach (KeyValuePair<string, int> pair in this.di)
                {
                    stringsList.Add(pair.Key.ToString());
                }
            }

            if (includeBools)
            {
                foreach (KeyValuePair<string, bool> pair in this.db)
                {
                    stringsList.Add(pair.Key.ToString());
                }
            }

            return stringsList;
        }

        /// <summary>
        /// Gets all the keys as an array of string
        /// </summary>
        /// <returns></returns>
        public string[] getKeys()
        {
            List<string> l = new List<string>();

            foreach (KeyValuePair<string, int> item in this.di)
            {
                l.Add(item.Key);
            }

            foreach (KeyValuePair<string, float> item in this.df)
            {
                l.Add(item.Key);
            }

            foreach (KeyValuePair<string, string> item in this.ds)
            {
                l.Add(item.Key);
            }

            foreach (KeyValuePair<string, bool> item in this.db)
            {
                l.Add(item.Key);
            }

            foreach (KeyValuePair<string, JsonObject> item in this.dj)
            {
                l.Add(item.Key);
            }

            foreach (KeyValuePair<string, JsonObject> item in this.da)
            {
                l.Add(item.Key);
            }

            foreach (KeyValuePair<string, JToken> item in this.dt)
            {
                l.Add(item.Key);
            }

            l.Sort();

            return l.ToArray();
        }

        /// <summary>
        /// Returns number of all elements
        /// </summary>
        /// <returns></returns>
        public int count()
        {
            int count = 0;

            count += this.di.Count;
            count += this.ds.Count;
            count += this.db.Count;
            count += this.dj.Count;
            count += this.da.Count;
            count += this.df.Count;
            count += this.dt.Count;

            return count;
        }

        /// <summary>
        /// Checks whether JsonObject is empty
        /// </summary>
        /// <returns></returns>
        public bool isEmpty()
        {
            if (this.count() == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns JSON representation of this object
        /// </summary>
        /// <returns></returns>
        public string toJson()
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            int count = this.count();

            if (this.isArray())
            {
                sb.Append("[");

                for (index = 0; index < count; index++)
                {
                    if (index > 0)
                    {
                        sb.Append(",");
                    }

                    string key = index.ToString();

                    if (this.di.ContainsKey(key))
                    {
                        sb.Append(this.di[key].ToString());
                    }
                    else if (this.df.ContainsKey(key))
                    {
                        sb.Append(this.df[key].ToString("R", CultureInfo.InvariantCulture));
                    }
                    else if (this.ds.ContainsKey(key))
                    {
                        sb.Append(Json.encode(this.ds[key]));
                    }
                    else if (this.db.ContainsKey(key))
                    {
                        sb.Append(this.db[key].ToString().ToLower());
                    }
                    else if (this.dt.ContainsKey(key))
                    {
                        sb.Append("null");
                    }
                    else if (this.dj.ContainsKey(key))
                    {
                        sb.Append(this.dj[key].toJson());
                    }
                    else if (this.da.ContainsKey(key))
                    {
                        sb.Append(this.da[key].toJson());
                    }
                }

                sb.Append("]");
            }
            else
            {
                sb.Append("{");

                // order of properties in object is not important, hence we just do it in order

                foreach (KeyValuePair<string, int> item in this.di)
                {
                    if (index > 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append(Json.encode(item.Key) + ":" + item.Value.ToString());

                    index++;
                }

                foreach (KeyValuePair<string, float> item in this.df)
                {
                    if (index > 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append(Json.encode(item.Key) + ":" + item.Value.ToString("R", CultureInfo.InvariantCulture));

                    index++;
                }

                foreach (KeyValuePair<string, string> item in this.ds)
                {
                    if (index > 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append(Json.encode(item.Key) + ":" + Json.encode(item.Value));

                    index++;
                }

                foreach (KeyValuePair<string, bool> item in this.db)
                {
                    if (index > 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append(Json.encode(item.Key) + ":" + item.Value.ToString().ToLower());

                    index++;
                }

                foreach (KeyValuePair<string, JToken> item in this.dt)
                {
                    if (index > 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append(Json.encode(item.Key) + ":null");

                    index++;
                }

                foreach (KeyValuePair<string, JsonObject> item in this.dj)
                {
                    if (index > 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append(Json.encode(item.Key) + ":" + item.Value.toJson());

                    index++;
                }

                foreach (KeyValuePair<string, JsonObject> item in this.da)
                {
                    if (index > 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append(Json.encode(item.Key) + ":" + item.Value.toJson());

                    index++;
                }

                sb.Append("}");
            }

            return sb.ToString();
        }

        public object Clone()
        {
            return JsonObjectFactory.fromString(this.toJson());
        }
    }
}
