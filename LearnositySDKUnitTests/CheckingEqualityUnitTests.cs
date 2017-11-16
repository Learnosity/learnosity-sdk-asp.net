using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnositySDK.Utils;
using Newtonsoft.Json.Linq;

namespace LearnositySDKUnitTests
{
    /// <summary>
    /// Summary description for CheckingEqualityUnitTests
    /// </summary>
    [TestClass]
    public class CheckingEqualityUnitTests
    {
        [TestMethod]
        public void inputJSONEqualsOutput()
        {
            string JSON = "{\"boolean\":true,\"integer\":1,\"decimal\":1.2,\"string\":\"string\",\"object\":{\"property\":null},\"array\":[null,[2]]}";
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
            string JSON = "{\"boolean\":true,\"integer\":1,\"decimal\":1.2,\"string\":\"string\",\"object\":{\"property\":null},\"array\":[null,[2]]}";

            // building a structure for comparison purposes
            JsonObject jo = new JsonObject();
            JsonObject joInner = new JsonObject();
            JsonObject ja = new JsonObject(true);
            JsonObject jaInner = new JsonObject(true);
            jo.set("boolean", true);
            jo.set("integer", 1);
            jo.set("decimal", 1.2m);
            jo.set("string", "string");
            jo.set("object", joInner);
            jo.set("array", ja);
            joInner.setNull("property");
            ja.setNull();
            ja.set(jaInner);
            jaInner.set(2);

            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(JSON, JSON),
                "JSONEquality method doesn't work correctly"
            );

            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(jo, jo),
                "JSONEquality method doesn't work correctly"
            );

            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(jo, JSON),
                "JSONEquality method doesn't work correctly"
            );

            JsonObject jo1 = new JsonObject();
            JsonObject jo2 = new JsonObject();
            JsonObject jo3 = new JsonObject();

            jo1.set("a", "a");
            jo1.set("b", "b");
            jo1.set("c", "c");

            jo2.set("d", "d");
            jo2.set("e", "e");
            jo2.set("f", "f");

            jo3.set("a", "d");
            jo3.set("b", "e");
            jo3.set("c", "f");

            Assert.IsFalse(
                JsonObjectFactory.JSONEquality(jo1, jo2),
                "JSONEquality method doesn't work correctly"
            );

            Assert.IsFalse(
                JsonObjectFactory.JSONEquality(jo1, jo3),
                "JSONEquality method doesn't work correctly"
            );

            Assert.IsFalse(
                JsonObjectFactory.JSONEquality(jo2, jo3),
                "JSONEquality method doesn't work correctly"
            );

            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(jo1, jo1),
                "JSONEquality method doesn't work correctly"
            );

            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(jo2, jo2),
                "JSONEquality method doesn't work correctly"
            );

            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(jo3, jo3),
                "JSONEquality method doesn't work correctly"
            );
        }
    }
}
