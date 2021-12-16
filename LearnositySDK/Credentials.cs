using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace LearnositySDK
{
    public static class Credentials
    {
        // The consumerKey and consumerSecret are the public & private
        // security keys required to access Learnosity APIs and
        // data. Learnosity will provide keys for your own private account.
        // Note: The consumer secret should be in a properly secured credential
        // store, and *NEVER* checked into version control. 
        // The keys listed here grant access to Learnosity's public demos account.

        public static string ConsumerKey = "yis0TYCu7U9V4o7M";
        public static string ConsumerSecret = "74c5fd430cf1242a527f6223aebd42d30464be22";
        public static string Domain = "localhost";
    }
}
