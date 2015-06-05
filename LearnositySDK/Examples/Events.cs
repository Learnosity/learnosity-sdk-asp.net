using System;
using System.Text;
using LearnositySDK.Request;
using LearnositySDK.Utils;

namespace LearnositySDK.Examples
{
    public class Events
    {

        public static string Simple()
        {
            string service = "events";

            JsonObject security = new JsonObject();
            security.set("consumer_key", "yis0TYCu7U9V4o7M");
            security.set("user_id", "demo_student");
            security.set("domain", "localhost");

            string secret = "74c5fd430cf1242a527f6223aebd42d30464be22";

            JsonObject request = new JsonObject();
            request.set("eventbus", true);
            request.set("skip", true);
            request.set("users", Events.users());

            Init init = new Init(service, security, secret, request);
            return init.generate();
        }

        private static JsonObject users()
        {
            JsonObject users = new JsonObject(true);

            for (int i = 0; i <= 10; i++)
            {
                string user_id = "userid_" + i;
                users.set(user_id);
            }

            return users;
        }

    }
}
