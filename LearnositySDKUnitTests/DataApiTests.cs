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
        /// Helper method to test the private extractEndpoint and buildActionMetadata methods
        /// by making an actual request and checking the action header
        /// </summary>
        private string GetActionMetadataFromUrl(string url, string action)
        {
            // Create a minimal request to test endpoint extraction
            JsonObject security = new JsonObject();
            security.set("consumer_key", "test_consumer");
            security.set("domain", "localhost");

            JsonObject request = new JsonObject();
            request.set("limit", 1);

            DataApi da = new DataApi();

            try
            {
                // Make the request (it will fail because of invalid credentials, but that's OK)
                // We just need to check that the metadata extraction works
                Remote r = da.request(url, security, "test_secret", request, action);

                // Get the action header that was set
                var headers = r.getRequestHeaders();
                if (headers != null)
                {
                    return headers.Get("X-Learnosity-Action");
                }
            }
            catch
            {
                // Request will fail due to invalid credentials, but we can still check headers
                // by examining what would have been sent
            }

            return null;
        }
    }
}

