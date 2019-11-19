using System;
using Xunit;
using System.IO;
using LearnositySDK.Utils;
using LearnositySDK.Request;

namespace LearnositySDKIntegrationTests
{
    public class LearnositySDKIntegrationTests
    {
        private readonly string consumerKey = Config.ConsumerKey;
        private readonly string consumerSecret = Config.ConsumerSecret;
        private readonly string domain = Config.Domain;
        private readonly string baseDataAPIUrl = BuildDataAPIBaseUrl(Config.ENV, Config.REGION, Config.VER_DataAPI);

        [Fact]
        public void DataAPIGetItems()
        {
            // Arrange
            JsonObject JsonResponse;

            string endpoint = this.baseDataAPIUrl + "/itembank/items";

            string action = "get";

            JsonObject security = new JsonObject();
            security.set("consumer_key", this.consumerKey);
            security.set("domain", this.domain);

            JsonObject request = new JsonObject();
            request.set("limit", 3);

            // Act
            DataApi da = new DataApi();
            Remote r = da.request(endpoint, security, this.consumerSecret, request, action);

            JsonResponse = JsonObjectFactory.fromString(r.getBody());

            // Assert
            Assert.True(JsonResponse.getJsonObject("meta").getBool("status"));
        }

        private static string BuildDataAPIBaseUrl(string env, string region, string version)
        {
            string regionDomain = "";
            string envDomain = "";
            string versionPath = "v1";

            if (!string.IsNullOrEmpty(region))
            {
                regionDomain = string.Concat("-", region);
            }

            if (!string.IsNullOrEmpty(env) && string.Compare(env, "prod") != 0)
            {
                envDomain = "." + env;
            }

            if (!string.IsNullOrEmpty(env) && string.Compare(env, "vg") == 0)
            {
                versionPath = "latest";
            }
            else if (!string.IsNullOrEmpty(version))
            {
                versionPath = version;
            }

            return "https://data" + regionDomain + envDomain + ".learnosity.com/" + versionPath;
        }

        [Fact]
        public void InitGeneratesExactSameSignature()
        {
            string action = "get";

            JsonObject security = this.generateSecurityObject();

            JsonObject request = new JsonObject();
            request.set("limit", 100);

            Init.disableTelemetry();
            Init init = new Init("data", security, this.consumerSecret, request, action);

            // Assert signature is still the same
            Assert.Equal(
                "e1eae0b86148df69173cb3b824275ea73c9c93967f7d17d6957fcdd299c8a4fe",
                init.generateSignature()
            );

            // Assert telemetry is turned off
            Assert.False(init.isTelemetryEnabled());
            Init.enableTelemetry();
        }

        [Fact]
        public void InitGenerateBuildsNonEmptyRequest()
        {
            string action = "get";

            JsonObject security = this.generateSecurityObject();

            JsonObject request = new JsonObject();
            request.set("page", 1);

            Init init = new Init("data", security, this.consumerSecret, request, action);
            string generatedString = init.generate();

            // Assert generated string is not empty
            Assert.NotEmpty(generatedString);

            // Assert telemetry is turned on
            Assert.True(init.isTelemetryEnabled());
        }

        [Fact]
        public void EnabledTelemetryAddsSdkField()
        {
            string action = "get";
            JsonObject security = this.generateSecurityObject();

            JsonObject request = new JsonObject();
            request.set("page", 1);

            Init init = new Init("data", security, this.consumerSecret, request, action);
            string generatedString = init.generate();

            Assert.Contains("meta", generatedString);
            Assert.Contains("sdk", generatedString);
        }

        [Fact]
        public void EnabledTelemetryPreservesOtherMetaProps()
        {
            string action = "get";
            JsonObject security = this.generateSecurityObject();

            JsonObject metaField = new JsonObject();
            metaField.set("test_key_string", "test-string");
            metaField.set("test_key_integer", 12345);

            JsonObject request = new JsonObject();
            request.set("page", 1);
            request.set("meta", metaField);

            Init init = new Init("data", security, this.consumerSecret, request, action);
            string generatedString = init.generate();

            Assert.Contains("meta", generatedString);
            Assert.Contains("sdk", generatedString);
            Assert.Contains("test_key_string", generatedString);
            Assert.Contains("test_key_integer", generatedString);
        }

        [Fact]
        public void DisabledTelemetryPreservesEmptyProps()
        {
            string action = "get";
            JsonObject security = this.generateSecurityObject();

            Init.disableTelemetry();

            JsonObject request = new JsonObject();
            request.set("page", 1);

            Init init = new Init("data", security, this.consumerSecret, request, action);
            string generatedString = init.generate();

            Assert.DoesNotContain("meta", generatedString);
            Assert.DoesNotContain("sdk", generatedString);

            Init.enableTelemetry();
        }

        [Fact]
        public void DisabledTelemetryPreservesFilledProps()
        {
            string action = "get";
            JsonObject security = this.generateSecurityObject();

            Init.disableTelemetry();

            JsonObject metaField = new JsonObject();
            metaField.set("test_key_string", "test-string");
            metaField.set("test_key_integer", 12345);

            JsonObject request = new JsonObject();
            request.set("page", 1);
            request.set("meta", metaField);

            Init init = new Init("data", security, this.consumerSecret, request, action);
            string generatedString = init.generate();

            Assert.Contains("meta", generatedString);
            Assert.DoesNotContain("sdk", generatedString);
            Assert.Contains("test_key_string", generatedString);
            Assert.Contains("test_key_integer", generatedString);

            Init.enableTelemetry();
        }

        private JsonObject generateSecurityObject()
        {
            string timestamp = "20140626-0528";

            JsonObject security = new JsonObject();
            security.set("consumer_key", this.consumerKey);
            security.set("domain", this.domain);
            security.set("timestamp", timestamp);

            return security;
        }
    }
}
