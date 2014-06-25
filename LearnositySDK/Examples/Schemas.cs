using System;
using System.Text;
using LearnositySDK.Request;
using LearnositySDK.Utils;

namespace LearnositySDK.Examples
{
    public class Schemas
    {
        public static string Simple(out string URL)
        {
            string url = "http://schemas.learnosity.com/stable/questions/templates";
            Remote remote = new Remote();
            remote.get(url);

            URL = url;
            return remote.getBody();
        }
    }
}
