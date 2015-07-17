using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace LearnositySDK
{
    public static class Credentials
    {
        public static string ConsumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
        public static string ConsumerSecret =  ConfigurationManager.AppSettings["ConsumerSecret"];
        public static string Domain =  ConfigurationManager.AppSettings["Domain"];
    }
}
