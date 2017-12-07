using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnositySDK.Utils;
using Newtonsoft.Json.Linq;

namespace LearnositySDKUnitTests
{
    /// <summary>
    /// Summary description for MergingObjectsUnitTests
    /// </summary>
    [TestClass]
    public class MergingObjectsUnitTests
    {
        [TestMethod]
        public void MergingJSONArraysWorksProperly()
        {
            JsonObject ja1 = new JsonObject(true);
            JsonObject ja2 = new JsonObject(true);
            JsonObject jaResult1 = new JsonObject(true);

            ja1.set("a");
            ja1.set("b");
            ja1.set("c");

            ja2.set("d");
            ja2.set("e");
            ja2.set("f");

            jaResult1.set("a");
            jaResult1.set("b");
            jaResult1.set("c");
            jaResult1.set("d");
            jaResult1.set("e");
            jaResult1.set("f");

            JsonObject jaResult2 = Tools.array_merge(ja1, ja2);

            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(jaResult1, jaResult2),
                "Merging of two arrays doesn't work properly"
            );

            jaResult1 = new JsonObject(true);
            jaResult1.set("d");
            jaResult1.set("e");
            jaResult1.set("f");

            jaResult2 = Tools.array_merge(ja1, ja2, true);

            // overwrite should only work for objects
            Assert.IsFalse(
                JsonObjectFactory.JSONEquality(jaResult1, jaResult2),
                "Merging of two arrays with overwriting enabled does work properly"
            );
        }

        [TestMethod]
        public void MergingJSONObjectsWorksProperly()
        {
            JsonObject jo1 = new JsonObject();
            JsonObject jo2 = new JsonObject();
            JsonObject jo3 = new JsonObject();
            JsonObject joResult1 = new JsonObject();

            jo1.set("a", "a");
            jo1.set("b", "b");
            jo1.set("c", "c");

            jo2.set("d", "d");
            jo2.set("e", "e");
            jo2.set("f", "f");

            jo3.set("a", "d");
            jo3.set("b", "e");
            jo3.set("c", "f");

            joResult1.set("a", "a");
            joResult1.set("b", "b");
            joResult1.set("c", "c");
            joResult1.set("d", "d");
            joResult1.set("e", "e");
            joResult1.set("f", "f");

            JsonObject joResult2 = Tools.array_merge(jo1, jo2);

            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(joResult1, joResult2),
                "Merging of two objects doesn't work properly"
            );

            joResult1 = new JsonObject();
            joResult1.set("a", "a");
            joResult1.set("b", "b");
            joResult1.set("c", "c");

            joResult2 = Tools.array_merge(jo1, jo3);

            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(joResult1, joResult2),
                "Merging of two objects doesn't work properly"
            );

            joResult1 = new JsonObject();
            joResult1.set("a", "d");
            joResult1.set("b", "e");
            joResult1.set("c", "f");

            joResult2 = Tools.array_merge(jo1, jo3, true);

            // overwrite works for objects
            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(joResult1, joResult2),
                "Merging of two objects using the same property names with overwriting enabled doesn't work properly"
            );
        }

        [TestMethod]
        public void RecursiveMergingJSONObjectsWorksProperly()
        {
            JsonObject jo1 = new JsonObject();
            JsonObject jo1o = new JsonObject();
            JsonObject jo1a = new JsonObject(true);
            JsonObject jo1ao1 = new JsonObject();
            JsonObject jo1ao2 = new JsonObject();
            JsonObject jo2 = new JsonObject();
            JsonObject jo2o = new JsonObject();
            JsonObject jo2a = new JsonObject(true);
            JsonObject jo2ao1 = new JsonObject();
            JsonObject jo2ao2 = new JsonObject();
            JsonObject joResult1 = new JsonObject();
            JsonObject joResult2;

            jo1.set("a", "a1");
            jo1.set("b", "b1");
            jo1.set("obj", jo1o);
            jo1.set("arr", jo1a);
            jo1o.set("d", "d1");
            jo1o.set("e", "e1");
            jo1a.set(true);
            jo1a.setNull();
            jo1a.set("string");
            jo1a.set(1);
            jo1a.set(1.2m);
            jo1a.set(jo1ao1);
            jo1a.set(jo1ao2);
            jo1ao1.set("a", "a1");
            jo1ao1.set("b", "b1");
            jo1ao2.set("a", "a1");
            jo1ao2.set("b", "b1");

            jo2.set("a", "a2");
            jo2.set("c", "c2");
            jo2.set("obj", jo2o);
            jo2.set("arr", jo2a);
            jo2o.set("d", "d2");
            jo2o.set("f", "f2");
            jo2a.set(true);
            jo2a.setNull();
            jo2a.set("string");
            jo2a.set(1);
            jo2a.set(1.2m);
            jo2a.set(jo2ao1);
            jo2a.set(jo2ao2);
            jo2ao1.set("a", "a2");
            jo2ao1.set("b", "b2");
            jo2ao2.set("a", "a2");
            jo2ao2.set("b", "b2");

            joResult1 = JsonObjectFactory.fromString(@"{
    ""a"": ""a1"",
    ""b"": ""b1"",
    ""c"": ""c2"",
    ""obj"": {
        ""d"": ""d1"",
        ""e"": ""e1"",
        ""f"": ""f2""
    },
    ""arr"": [
        true,
        null,
        ""string"",
        1,
        1.2,
        {
            ""a"": ""a1"",
            ""b"": ""b1""
        },
        {
            ""a"": ""a1"",
            ""b"": ""b1""
        },
        true,
        null,
        ""string"",
        1,
        1.2,
        {
            ""a"": ""a2"",
            ""b"": ""b2""
        },
        {
            ""a"": ""a2"",
            ""b"": ""b2""
        }
    ]
}");

            joResult2 = Tools.array_merge_recursive(jo1, jo2, false);
            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(joResult1, joResult2),
                "Merging of two objects recursively using the same property names doesn't work properly"
            );

            joResult1 = JsonObjectFactory.fromString(@"{
    ""a"": ""a2"",
    ""b"": ""b1"",
    ""c"": ""c2"",
    ""obj"": {
        ""d"": ""d2"",
        ""e"": ""e1"",
        ""f"": ""f2""
    },
    ""arr"": [
        true,
        null,
        ""string"",
        1,
        1.2,
        {
            ""a"": ""a1"",
            ""b"": ""b1""
        },
        {
            ""a"": ""a1"",
            ""b"": ""b1""
        },
        true,
        null,
        ""string"",
        1,
        1.2,
        {
            ""a"": ""a2"",
            ""b"": ""b2""
        },
        {
            ""a"": ""a2"",
            ""b"": ""b2""
        }
    ]
}");

            joResult2 = Tools.array_merge_recursive(jo1, jo2, true);
            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(joResult1, joResult2),
                "Merging of two objects recursively using the same property names with overwriting enabled doesn't work properly"
            );
        }

        [TestMethod]
        public void RecursiveMergingJSONArraysWorksProperly()
        {
            JsonObject jo1 = new JsonObject();
            JsonObject jo2 = new JsonObject();
            JsonObject joResult1 = new JsonObject();
            JsonObject joResult2;



            joResult2 = Tools.array_merge_recursive(jo1, jo2, false);
            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(joResult1, joResult2),
                "Merging of two objects recursively using the same property names doesn't work properly"
            );

            joResult2 = Tools.array_merge_recursive(jo1, jo2, true);
            Assert.IsTrue(
                JsonObjectFactory.JSONEquality(joResult1, joResult2),
                "Merging of two objects recursively using the same property names with overwriting enabled doesn't work properly"
            );
        }
    }
}
