using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnositySDK.Utils;

namespace LearnositySDKUnitTests
{
    /// <summary>
    /// Summary description for ToolIsEmptyUnitTests
    /// </summary>
    [TestClass]
    public class ToolIsEmptyUnitTests
    {
        [TestMethod]
        public void WhetherItWorksOnEmptyObjects()
        {
            JsonObject obj = new JsonObject();
            Assert.IsTrue(
                Tools.empty(obj),
                "Tools.empty doesn't work on empty objects"
            );
        }

        [TestMethod]
        public void WhetherItWorksOnEmptyArrays()
        {
            JsonObject arr = new JsonObject(true);
            Assert.IsTrue(
                Tools.empty(arr),
                "Tools.empty doesn't work on empty arrays"
            );
        }

        [TestMethod]
        public void WhetherItWorksOnEmptyStrings()
        {
            String str = "";
            Assert.IsTrue(
                Tools.empty(str),
                "Tools.empty doesn't work on empty strings"
            );
        }

        [TestMethod]
        public void WhetherItWorksOnNullObjects()
        {
            Object obj = null;
            Assert.IsTrue(
                Tools.empty(obj),
                "Tools.empty doesn't work on null objects"
            );
        }
    }
}
