using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnositySDK.Utils
{
    class Conversion
    {
        /// <summary>
        /// Converts a raw value in bytes to a friendly format. Either Bytes, KB, MB or GB
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string formatSizeUnits(int bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (bytes >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                bytes = bytes / 1024;
            }
            string result = String.Format("{0:0.##} {1}", bytes, sizes[order]);
            return result;
        }
    }
}
