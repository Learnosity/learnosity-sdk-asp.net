using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnositySDK.Utils;
using LearnositySDK.Request;


namespace LearnositySDKUnitTests
{
    [TestClass]
    public class RequestAsStringUnitTests
    {
        private readonly string consumerKey = "yis0TYCu7U9V4o7M";
        private readonly string consumerSecret = "74c5fd430cf1242a527f6223aebd42d30464be22";
        private readonly string domain = "localhost";

        [TestMethod]
        public void AuthorApiItemListAsString()
        {
            // Arrange
            string service = "author";
            JsonObject security = GetSecurityObj();
            string secret = consumerSecret;
            JsonObject request = new JsonObject();

            request.set("mode", "item_list");

            JsonObject config = new JsonObject();
            JsonObject config_item_list = new JsonObject();
            JsonObject config_item_list_item = new JsonObject();
            config_item_list_item.set("status", true);
            config_item_list.set("item", config_item_list_item);
            config.set("item_list", config_item_list);
            request.set("config", config);

            JsonObject user = new JsonObject();
            user.set("id", "walterwhite");
            user.set("firstname", "walter");
            user.set("lastname", "white");
            request.set("user", user);

            // Act
            Init initAsString = new Init(service, security.toJson(), secret, request.toJson());
            string signedRequestAsString = initAsString.generate();

            Init initAsObject = new Init(service, security, secret, request);
            string signedRequestAsObject = initAsObject.generate();

            // Assert
            Assert.AreEqual(
                signedRequestAsString,
                signedRequestAsObject
            );
        }

        [TestMethod]
        public void ItemsApiAsString()
        {
            // Arrange
            string service = "items";
            JsonObject security = GetSecurityObj();
            string secret = consumerSecret;
            JsonObject request = new JsonObject();

            request.set("activity_template_id", "demo-activity");
            request.set("activity_id", "my-demo-activity");
            request.set("name", "Demo Activity");
            request.set("session_id", Uuid.generate());
            request.set("user_id", "demo_student");

            // Act
            Init initAsString = new Init(service, security.toJson(), secret, request.toJson());
            string signedRequestAsString = initAsString.generate();

            Init initAsObject = new Init(service, security, secret, request);
            string signedRequestAsObject = initAsObject.generate();

            // Assert
            Assert.AreEqual(
                signedRequestAsString,
                signedRequestAsObject
            );
        }

        private JsonObject GetSecurityObj() {
            JsonObject security = new JsonObject();
            security.set("consumer_key", consumerKey);
            security.set("domain", domain);

            return security;
        }

    }
}
