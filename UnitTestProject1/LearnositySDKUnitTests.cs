using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnositySDK.Utils;

namespace UnitTestProject1
{
    [TestClass]
    public class LearnositySDKUnitTests
    {
        [TestMethod]
        public void inputJSONEqualsOutput()
        {
            string JSON = "{\"boolean\":true,\"integer\":1,\"float\":1.2,\"string\":\"string\",\"object\":{\"property\":null},\"array\":[null,[2]]}";
            JsonObject jo = JsonObjectFactory.fromString(JSON);
            string outputJSON = jo.toJson();

            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(JSON, outputJSON),
                "Input JSON (" + JSON + ") doesn't equal the output (" + outputJSON + ")"
            );

            JSON = "[0,1,2,3,4,5,6,7,8,9,10]";
            jo = JsonObjectFactory.fromString(JSON);
            outputJSON = jo.toJson();

            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(JSON, outputJSON),
                "Input JSON (" + JSON + ") doesn't equal the output (" + outputJSON + ")"
            );

            JSON = "[0,\"1\",2,\"3\",4,\"5\",6,\"7\",8,\"9\",10]";
            jo = JsonObjectFactory.fromString(JSON);
            outputJSON = jo.toJson();

            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(JSON, outputJSON),
                "Input JSON (" + JSON + ") doesn't equal the output (" + outputJSON + ")"
            );
        }

        [TestMethod]
        public void JSONEqualityMethodWorksProperly()
        {
            string JSON = "{\"boolean\":true,\"integer\":1,\"float\":1.2,\"string\":\"string\",\"object\":{\"property\":null},\"array\":[null,[2]]}";

            // building a structure for comparison purposes
            JsonObject jo = new JsonObject();
            JsonObject joInner = new JsonObject();
            JsonObject ja = new JsonObject(true);
            JsonObject jaInner = new JsonObject(true);
            jo.set("boolean", true);
            jo.set("integer", 1);
            jo.set("float", 1.2f);
            jo.set("string", "string");
            jo.set("object", joInner);
            jo.set("array", ja);
            joInner.set("property", 2);
            ja.set(2);
            ja.set(jaInner);
            jaInner.set(3);
            jaInner.set(4);
            
            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(JSON, JSON),
                "JSONEquality method doesn't work correctly with strings"
            );

            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(jo, jo),
                "JSONEquality method doesn't work correctly with strings"
            );
        }
    }
}
