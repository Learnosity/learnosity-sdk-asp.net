using System;
using System.Text;
using LearnositySDK.Request;
using LearnositySDK.Utils;

namespace LearnositySDK.Examples
{
    public class Author
    {
        public static string Simple()
        {
            string service = "author";

            JsonObject security = new JsonObject();
            security.set("consumer_key", "yis0TYCu7U9V4o7M");
            security.set("domain", "localhost");

            string secret = "74c5fd430cf1242a527f6223aebd42d30464be22";

            JsonObject request = new JsonObject();
            JsonObject tags = new JsonObject(true);

            JsonObject tag1 = new JsonObject();
            tag1.set("type", "course");
            tag1.set("name", "commoncore");
            tags.set(tag1);

            JsonObject tag2 = new JsonObject();
            tag2.set("type", "subject");
            tag2.set("name", "Maths");
            tags.set(tag2);

            request.set("limit", 100);
            request.set("tags", tags);

            Init init = new Init(service, security, secret, request);
            return init.generate();
        }
    }
}
