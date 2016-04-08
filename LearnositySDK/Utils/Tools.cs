using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;

namespace LearnositySDK.Utils
{
    public class Tools
    {
        /// <summary>
        /// Returns integer from hexadecimal representation
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int hexdec(string str)
        {
            return Int32.Parse(str, NumberStyles.HexNumber);
        }

        /// <summary>
        /// Returns string based on ASCII number
        /// </summary>
        /// <param name="ascii"></param>
        /// <returns></returns>
        public static string chr(int ascii)
        {
            return Convert.ToString(ascii);
        }

        /// <summary>
        /// Checks whether specified value is in the array
        /// </summary>
        /// <param name="item"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool in_array(string item, string[] array)
        {
            int index = Array.IndexOf(array, item);

            if (index < 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether value is empty
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool empty(Object obj)
        {
            if (obj == null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether value is empty
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool empty(JsonObject obj)
        {
            if (obj == null || obj.isEmpty())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether value is empty
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool empty(string obj)
        {
            if (obj == null || obj.Length == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Merges two objects
        /// </summary>
        /// <param name="arr1"></param>
        /// <param name="arr2"></param>
        /// <param name="overwrite">whether to overwrite</param>
        /// <returns></returns>
        public static JsonObject array_merge(JsonObject arr1, JsonObject arr2, bool overwrite = false)
        {
            return JsonObjectFactory.merge(arr1, arr2, overwrite);
        }

        /// <summary>
        /// Merges two objects recursively
        /// </summary>
        /// <param name="arr1"></param>
        /// <param name="arr2"></param>
        /// <param name="overwrite">whether to overwrite</param>
        /// <returns></returns>
        public static JsonObject array_merge_recursive(JsonObject arr1, JsonObject arr2, bool overwrite = false)
        {
            return JsonObjectFactory.merge(arr1, arr2, overwrite, true);
        }

        /// <summary>
        /// Checks whether specified key exists in the object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool array_key_exists(string key, JsonObject array)
        {
            string[] keys = Tools.array_keys(array);
            return Tools.in_array(key, keys);
        }

        /// <summary>
        /// Returns SHA256 hash
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string sha256(string str)
        {
            return CryptoUtil.sha256(str);
        }

        /// <summary>
        /// Returns hash
        /// </summary>
        /// <param name="algorithm">Possible algorithms: md5, sha256 (default), sha1</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string hash(string algorithm, string message)
        {
            string str = "";

            switch (algorithm)
            {
                case "sha256":
                    // fall through
                default:
                    str = Tools.sha256(message);
                    break;
            }

            return str;
        }

        /// <summary>
        /// Joins array values into string using specified separator
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string implode(string separator, string[] array)
        {
            return String.Join(separator, array);
        }

        /// <summary>
        /// Returns array of keys
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[] array_keys(JsonObject array)
        {
            return array.getKeys();
        }

        /// <summary>
        /// Returns UNIX timestamp
        /// </summary>
        /// <returns></returns>
        public static int timestamp()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp;
        }

        /// <summary>
        /// Returns gmdate according to specified format
        /// </summary>
        /// <see>http://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx</see>
        /// <see>http://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx</see>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string gmdate(string format)
        {
            DateTime dt = DateTime.UtcNow;
            return dt.ToString(format);
        }

        /// <summary>
        /// Returns random number from defined range
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int mt_rand(int min = Int32.MinValue, int max = Int32.MaxValue)
        {
            Random r = new Random();
            return r.Next(min, max);
        }
    }
}
