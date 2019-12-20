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
                { "assess", "03f4869659eeaca81077785135d5157874f4800e57752bf507891bf39c4d4a90" },
                { "author", "108b985a4db36ef03905572943a514fc02ed7cc6b700926183df7babc2cd1c96" },
                { "items", "dd6bf2a5fd28c9935acef5e2918a1069269154414c19ab346d476c363a7a964c" },
                { "questions", "03f4869659eeaca81077785135d5157874f4800e57752bf507891bf39c4d4a90" },
                { "reports", "91085beccf57bf0df77c89df94d1055e631b36bc11941e61460b445b4ed774bc" },
            };
        }

        [TestMethod]
        public void testGenerateJsonDataApiGetRequest()
        {
            Init init = TestRequest.getTestRequestFor("data", "get").getJsonInit();
            string expectedSignature = "e1eae0b86148df69173cb3b824275ea73c9c93967f7d17d6957fcdd299c8a4fe";


            Assert.AreEqual(
                "security=%7b%22consumer_key%22%3a%22yis0TYCu7U9V4o7M%22%2c%22domain%22%3a%22localhost%22%2c%22timestamp%22%3a%2220140626-0528%22%2c%22signature%22%3a%22" + expectedSignature + "%22%7d&request=%7b%22limit%22%3a100%7d&action=get",
                init.generate()
            );
        }

        [TestMethod]
        public void testGenerateJsonDataApiPostRequest()
        {
            Init init = TestRequest.getTestRequestFor("data", "post").getJsonInit();
            string expectedSignature = "18e5416041a13f95681f747222ca7bdaaebde057f4f222083881cd0ad6282c38";

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
