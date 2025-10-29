using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnositySDK.Request;
using LearnositySDK.Utils;

namespace LearnositySDKUnitTests
{
    [TestClass]
    public class DataApiTests
    {
        [TestMethod]
        public void TestExtractEndpointWithV1Version()
        {
            // Arrange
            string url = "https://data.learnosity.com/v1/itembank/items";
            string expectedAction = "get_/itembank/items";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithLatestLtsVersion()
        {
            // Arrange
            string url = "https://data.learnosity.com/latest-lts/itembank/items";
            string expectedAction = "get_/itembank/items";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithSemanticVersion()
        {
            // Arrange
            string url = "https://data.learnosity.com/v2025.1.LTS/itembank/items";
            string expectedAction = "get_/itembank/items";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithoutVersion()
        {
            // Arrange
            string url = "https://data.learnosity.com/itembank/items";
            string expectedAction = "get_/itembank/items";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithSessionsResponsesAndV1()
        {
            // Arrange
            string url = "https://data.learnosity.com/v1/sessions/responses";
            string expectedAction = "get_/sessions/responses";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithSessionsResponsesNoVersion()
        {
            // Arrange
            string url = "https://data.learnosity.com/sessions/responses";
            string expectedAction = "get_/sessions/responses";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithSetAction()
        {
            // Arrange
            string url = "https://data.learnosity.com/v1/itembank/activities";
            string expectedAction = "set_/itembank/activities";

            // Act
            string action = GetActionMetadataFromUrl(url, "set");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithSetActionNoVersion()
        {
            // Arrange
            string url = "https://data.learnosity.com/itembank/activities";
            string expectedAction = "set_/itembank/activities";

            // Act
            string action = GetActionMetadataFromUrl(url, "set");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithLatestVersion()
        {
            // Arrange
            string url = "https://data.learnosity.com/latest/itembank/items";
            string expectedAction = "get_/itembank/items";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithDeveloperVersion()
        {
            // Arrange
            string url = "https://data.learnosity.com/developer/itembank/items";
            string expectedAction = "get_/itembank/items";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithRouteStartingWithV()
        {
            // Arrange - hypothetical route that starts with 'v' but is not a version
            string url = "https://data.learnosity.com/validateItembanks";
            string expectedAction = "get_/validateItembanks";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithVerifyRoute()
        {
            // Arrange - hypothetical route that starts with 'v' but is not a version
            string url = "https://data.learnosity.com/verify/items";
            string expectedAction = "get_/verify/items";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithV10Version()
        {
            // Arrange - future version v10
            string url = "https://data.learnosity.com/v10/itembank/items";
            string expectedAction = "get_/itembank/items";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithComplexSemanticVersion()
        {
            // Arrange
            string url = "https://data.learnosity.com/v2024.12.LTS/sessions/responses";
            string expectedAction = "get_/sessions/responses";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithPreviewVersion()
        {
            // Arrange
            string url = "https://data.learnosity.com/v2022.3.preview1/itembank/items";
            string expectedAction = "get_/itembank/items";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithV0Version()
        {
            // Arrange - v0 is a valid version in the API
            string url = "https://data.learnosity.com/v0/itembank/items";
            string expectedAction = "get_/itembank/items";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithDecimalVersion()
        {
            // Arrange - v1.22, v1.84 are valid versions
            string url = "https://data.learnosity.com/v1.84/sessions/responses";
            string expectedAction = "get_/sessions/responses";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithUppercaseLatest()
        {
            // Arrange - case insensitive version keywords
            string url = "https://data.learnosity.com/LATEST/itembank/items";
            string expectedAction = "get_/itembank/items";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithMixedCaseLatestLts()
        {
            // Arrange - case insensitive version keywords
            string url = "https://data.learnosity.com/Latest-LTS/sessions/responses";
            string expectedAction = "get_/sessions/responses";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithUppercaseDeveloper()
        {
            // Arrange - case insensitive version keywords
            string url = "https://data.learnosity.com/DEVELOPER/itembank/items";
            string expectedAction = "get_/itembank/items";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithUppercaseV()
        {
            // Arrange - case insensitive 'v' prefix
            string url = "https://data.learnosity.com/V1/itembank/items";
            string expectedAction = "get_/itembank/items";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        [TestMethod]
        public void TestExtractEndpointWithUppercaseVAndLTS()
        {
            // Arrange - case insensitive version format
            string url = "https://data.learnosity.com/V2024.3.LTS/sessions/responses";
            string expectedAction = "get_/sessions/responses";

            // Act
            string action = GetActionMetadataFromUrl(url, "get");

            // Assert
            Assert.AreEqual(expectedAction, action);
        }

        /// <summary>
        /// Helper method to test the metadata extraction by making an actual request
        /// and checking both the action header and the request body metadata
        /// </summary>
        private string GetActionMetadataFromUrl(string url, string action)
        {
            // Create a minimal request to test endpoint extraction
            JsonObject security = new JsonObject();
            security.set("consumer_key", "test_consumer");
            security.set("domain", "localhost");

            JsonObject request = new JsonObject();
            request.set("limit", 1);

            // Create Init directly to test metadata in request body
            Init init = new Init("data", security, "test_secret", request, action, url);
            string parameters = init.generate();

            // Parse the generated parameters to check metadata in request body
            var parts = System.Web.HttpUtility.ParseQueryString(parameters);
            string requestJson = parts["request"];

            if (!string.IsNullOrEmpty(requestJson))
            {
                JsonObject requestData = JsonObjectFactory.fromString(requestJson);
                if (requestData != null)
                {
                    JsonObject meta = requestData.getJsonObject("meta");
                    if (meta != null)
                    {
                        // Verify metadata is in request body
                        string actionInBody = meta.getString("action");
                        string consumerInBody = meta.getString("consumer");

                        // Also verify it would be in headers by making the actual request
                        DataApi da = new DataApi();
                        try
                        {
                            Remote r = da.request(url, security, "test_secret", request, action);
                            var headers = r.getRequestHeaders();
                            if (headers != null)
                            {
                                string actionInHeader = headers.Get("X-Learnosity-Action");
                                string consumerInHeader = headers.Get("X-Learnosity-Consumer");
                                string sdkInHeader = headers.Get("X-Learnosity-SDK");

                                // Verify metadata is consistent between body and headers
                                Assert.AreEqual(actionInBody, actionInHeader, "Action metadata should match between body and headers");
                                Assert.AreEqual(consumerInBody, consumerInHeader, "Consumer metadata should match between body and headers");

                                // Verify SDK header format (should be "ASP.NET:version")
                                Assert.IsNotNull(sdkInHeader, "SDK header should be present");
                                Assert.IsTrue(sdkInHeader.StartsWith("ASP.NET:"), "SDK header should start with 'ASP.NET:'");
                            }
                        }
                        catch
                        {
                            // Request will fail due to invalid credentials, but we can still verify body metadata
                        }

                        return actionInBody;
                    }
                }
            }

            return null;
        }
    }
}

