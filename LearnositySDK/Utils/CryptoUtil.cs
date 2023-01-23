using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace LearnositySDK.Utils
{
    public class CryptoUtil
    {
        /// <summary>
        /// Compute sha256 hash for string encoded as UTF8
        /// </summary>
        /// <param name="s">String to be hashed</param>
        /// <returns>64-character hex string</returns>
        public static string sha256(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(bytes);

            return hex(hashBytes);
        }

        /// <summary>
        /// Compute hmac256 hash for string encoded as UTF8
        /// </summary>
        /// <param name="prehash">String to be hashed</param>
        ///<param name="secret">String used for encryption</param>
        /// <returns>64-character hex string</returns>
        public static string hmacsha256(string prehash, string secret)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(secret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(prehash);
            var hmacsha256 = new HMACSHA256(keyByte);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return hex(hashmessage); 
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        public static string hex(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString().ToLower();
        }
    }
}
