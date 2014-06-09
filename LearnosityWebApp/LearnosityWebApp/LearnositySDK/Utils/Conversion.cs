
namespace Learnosity.Utils
{
    class Conversion
    {
        /**
         * Converts a raw value in bytes to a friendly format.
         * Either Bytes, KB, MB or GB
         *
         * @param  int $bytes   Raw value to convert
         */
        public static string formatSizeUnits(int bytes)
        {
            string formatedBytes = "";
            if (bytes >= 1073741824) {
                formatedBytes = (bytes / 1073741824).ToString("0.00") + " GB";
            } else if (bytes >= 1048576) {
                formatedBytes = (bytes / 1048576).ToString("0.00") + " MB";
            } else if (bytes >= 1024) {
                formatedBytes = (bytes / 1024).ToString("0.00") + " KB";
            } else if (bytes > 1) {
                formatedBytes = bytes + " bytes";
            } else if (bytes == 1) {
                formatedBytes = bytes + " byte";
            } else {
                formatedBytes = "0 bytes";
            }

            return formatedBytes;
        }
    }
}