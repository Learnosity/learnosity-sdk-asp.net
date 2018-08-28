﻿using System;
namespace LearnositySDKIntegrationTests
{
    public static class Config
    {
        // Demo credentials
        public static string ConsumerKey = "yis0TYCu7U9V4o7M";
        public static string ConsumerSecret = "74c5fd430cf1242a527f6223aebd42d30464be22";

        // Default environment settings
        public static string ENVIRONMENT;
        public static string REGION;
        public static string Domain = "localhost";

        // API latest version
        public static string VER_DataAPI = "v1";

        static Config()
        {
            // Determine whether the environment variable exists.
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LRN_ENV")))
            {
                Environment.SetEnvironmentVariable("LRN_ENV", "prod");
            }
            ENVIRONMENT = Environment.GetEnvironmentVariable("LRN_ENV");

            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LRN_REG")))
            {
                Environment.SetEnvironmentVariable("LRN_REG", "va");
            }
            REGION = Environment.GetEnvironmentVariable("LRN_REG");
        }
    }
}
