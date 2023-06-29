using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnositySDK.Request;
using LearnositySDKUnitTests.TestData;

namespace LearnositySDKUnitTests
{
    /// <summary>
    /// Tests for the Init class of the SdK
    /// </summary>
    [TestClass]
    public class InitTests
    {
        [TestInitialize]
        public void beforeEach()
        {
            Init.disableTelemetry();
        }

        [TestCleanup]
        public void afterEach()
        {
            Init.enableTelemetry();
        }

        [TestMethod]
        public void testGenerateSignatureJsonRequests()
        {
            foreach (KeyValuePair<string, string> testCase in getInitTestCases())
            {
                Init init = TestRequest.getTestRequestFor(testCase.Key).getJsonInit();

                Assert.AreEqual(testCase.Value, init.generateSignature());
            }
        }

        [TestMethod]
        public void testGenerateSignatureStringRequests()
        {
            foreach (KeyValuePair<string, string> testCase in getInitTestCases())
            {
                Init init = TestRequest.getTestRequestFor(testCase.Key).getStringInit();

                Assert.AreEqual(testCase.Value, init.generateSignature());
            }
        }

        private Dictionary<string, string> getInitTestCases() {
            return new Dictionary<string, string>
            {
                { "assess", "$02$8de51b7601f606a7f32665541026580d09616028dde9a929ce81cf2e88f56eb8" },
                { "author", "$02$ca2769c4be77037cf22e0f7a2291fe48c470ac6db2f45520a259907370eff861" },
                { "items", "$02$78ef0334e708829bd3c92decc040f91c8433b07ba32cc9198705a18c36c2ea54" },
                { "questions", "$02$8de51b7601f606a7f32665541026580d09616028dde9a929ce81cf2e88f56eb8" },
                { "reports", "$02$8e0069e7aa8058b47509f35be236c53fa1a878c64b12589fd42f48b568f6ac84" },
            };
        }

        [TestMethod]
        public void testGenerateJsonDataApiGetRequest()
        {
            Init init = TestRequest.getTestRequestFor("data", "get").getJsonInit();
            string expectedSignature = "%2402%24e19c8a62fba81ef6baf2731e2ab0512feaf573ca5ca5929c2ee9a77303d2e197";


            Assert.AreEqual(
                "security=%7b%22consumer_key%22%3a%22yis0TYCu7U9V4o7M%22%2c%22domain%22%3a%22localhost%22%2c%22timestamp%22%3a%2220140626-0528%22%2c%22signature%22%3a%22" + expectedSignature + "%22%7d&request=%7b%22limit%22%3a100%7d&action=get",
                init.generate()
            );
        }

        [TestMethod]
        public void testGenerateJsonDataApiPostRequest()
        {
            Init init = TestRequest.getTestRequestFor("data", "post").getJsonInit();
            string expectedSignature = "%2402%249d1971fb9ac51482f7e73dcf87fc029d4a3dfffa05314f71af9d89fb3c2bcf16";

            Assert.AreEqual(
                "security=%7b%22consumer_key%22%3a%22yis0TYCu7U9V4o7M%22%2c%22domain%22%3a%22localhost%22%2c%22timestamp%22%3a%2220140626-0528%22%2c%22signature%22%3a%22" + expectedSignature + "%22%7d&request=%7b%22limit%22%3a100%7d&action=post",
                init.generate()
            );
        }

        [TestMethod]
        public void testNullRequestPacketWithTelemetry()
        {
            Init.enableTelemetry();

            Init init = new Init("data", TestRequest.getBaseSecurity(), TestRequest.getSecret(), null, "get");
            string generatedString = init.generate();

            Assert.IsTrue(generatedString.Contains("meta"));
            Assert.IsTrue(generatedString.Contains("sdk"));
        }

        [TestMethod]
        public void testEmptyStringRequestPacketWithTelemetry()
        {
            Init.enableTelemetry();

            Init init = new Init("data", TestRequest.getBaseSecurity(), TestRequest.getSecret(), "", "get");
            string generatedString = init.generate();

            Assert.IsTrue(generatedString.Contains("meta"));
            Assert.IsTrue(generatedString.Contains("sdk"));
        }
    }
}
