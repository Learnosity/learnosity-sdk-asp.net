using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LearnositySDK.Utils
{
    /// <summary>
    /// Tool to generate universally unique identifiers.
    /// @see http://en.wikipedia.org/wiki/Universally_unique_identifier
    /// </summary>
    public class Uuid
    {
        /// <summary>
        /// Generating Uuid
        /// </summary>
        /// <returns>Generated UUid</returns>
        public static string generate()
        {
            return Uuid.generate("v4", "");
        }

        /// <summary>
        /// Generating Uuid
        /// </summary>
        /// <param name="type">Type of a Uuid you want to generate: "v3" (MD5), "v4" (random), "v5" (SHA-1)</param>
        /// <param name="name"></param>
        /// <returns>Generated UUid</returns>
        public static string generate(string type, string name)
        {
            switch (type)
            {
                case "v3":
                    return Uuid.v3(name).ToString();
                case "v5":
                    return Uuid.v5(name).ToString();
                case "v4":
                    // fall through
                default:
                    return Uuid.v4().ToString();
            }
        }

        /// <summary>
        /// Generating version 3 of Uuid (MD5)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static Guid v3(string name)
        {
            return GuidUtility.Create(GuidUtility.IsoOidNamespace, name, 3);
        }
        
        /// <summary>
        /// Quote: "Long story short, be as it may, it seems that System.Guid.NewGuid() indeed uses version 4 UUID generation algorithm, because all the GUIDs it generates matches the criteria (see for yourself, I tried a couple million GUIDs, they all matched)."
        /// @see http://stackoverflow.com/questions/2621563/how-random-is-system-guid-newguid-take-two
        /// </summary>
        /// <returns></returns>
        private static Guid v4()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Generating version 5 of Uuid (SHA1)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static Guid v5(string name)
        {
            return GuidUtility.Create(GuidUtility.IsoOidNamespace, name);
        }
    }
}
