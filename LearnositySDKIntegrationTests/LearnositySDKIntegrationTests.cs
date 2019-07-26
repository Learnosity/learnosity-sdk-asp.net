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
            Assert.Equal(
                JsonResponse.getJsonObject("meta").getBool("status"),
                true
            );
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
            string timestamp = "20140626-0528";

            JsonObject security = new JsonObject();
            security.set("consumer_key", this.consumerKey);
            security.set("domain", this.domain);
            security.set("timestamp", timestamp);

            JsonObject request = new JsonObject();
            request.set("limit", 100);

            Init.disableTelemetry();
            Init init = new Init("data", security, this.consumerSecret, request, action);

            // Assert signature is still the same
            Assert.Equal(
                init.generateSignaturePublic(),
                "e1eae0b86148df69173cb3b824275ea73c9c93967f7d17d6957fcdd299c8a4fe"
            );

            // Assert telemetry is turned off
            Assert.False(init.isTelemetryEnabled());
        }

        [Fact]
        public void InitGenerateBuildsNonEmptyRequest()
        {
            string action = "get";
            string timestamp = "20140626-0528";

            JsonObject security = new JsonObject();
            security.set("consumer_key", this.consumerKey);
            security.set("domain", this.domain);
            security.set("timestamp", timestamp);

            JsonObject request = new JsonObject();
            request.set("page", 1);

            Init init = new Init("data", security, this.consumerSecret, request, action);
            string generatedString = init.generate();

            // Assert generated string is not empty
            Assert.NotEmpty(generatedString);

            // Assert telemetry is turned on
            Assert.True(init.isTelemetryEnabled());
        }
    }
}
