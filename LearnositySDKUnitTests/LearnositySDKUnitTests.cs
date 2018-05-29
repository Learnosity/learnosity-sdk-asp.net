using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnositySDK.Utils;
using LearnositySDK;
using LearnositySDK.Request;

namespace LearnositySDKUnitTests
{
    [TestClass]
    public class LearnositySDKUnitTests
    {
        [TestMethod]
        public void DataAPIGetItems()
        {
            JsonObject JsonResponse;

            string url = "https://data.learnosity.com/v1/itembank/items";
            string secret = Credentials.ConsumerSecret;
            string action = "get";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);

            JsonObject request = new JsonObject();
            request.set("limit", 3);

            DataApi da = new DataApi();
            Remote r = da.request(url, security, secret, request, action);

            JsonResponse = JsonObjectFactory.fromString(r.getBody());

            Assert.IsTrue(JsonResponse.getJsonObject("meta").getBool("status"),
                          "Data API get items endpoint returned false"
            );
        }
    }
}
