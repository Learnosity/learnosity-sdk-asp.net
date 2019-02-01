using System;
using Xunit;
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

        /* Passing request as string */
        [Fact]
        public void AuthorApiItemListAsString()
        {
            // Arrange
            string expectedSignedRequest = "{\"security\":{\"consumer_key\":\"yis0TYCu7U9V4o7M\",\"domain\":\"localhost\",\"timestamp\":\"20190129-1649\",\"signature\":\"0efa3c31f92d7259dd058809fe371f282f6e075eadde99495d8fae389c8dddf5\"},\"request\":{\"mode\":\"item_list\",\"config\":{\"item_list\":{\"toolbar\":{\"add\":true}}},\"user\":{\"id\":\"brianmoser\",\"firstname\":\"Test\",\"lastname\":\"Test\",\"email\":\"test@test.com\"},\"tags\":[{\"type\":\"course\",\"name\":\"commoncore\"}]}}";
            string service = "author";
            string security = "{\"consumer_key\":\"yis0TYCu7U9V4o7M\",\"domain\":\"localhost\",\"timestamp\":\"20190129-1649\"}";
            string secret = consumerSecret;
            string request = "{\"mode\":\"item_list\",\"config\":{\"item_list\":{\"toolbar\":{\"add\":true}}},\"user\":{\"id\":\"brianmoser\",\"firstname\":\"Test\",\"lastname\":\"Test\",\"email\":\"test@test.com\"},\"tags\":[{\"type\":\"course\",\"name\":\"commoncore\"}]}";

            // Act
            Init init = new Init(service, security, secret, request);
            string signedRequest = init.generate();

            // Assert
            Assert.Equal(
                expectedSignedRequest,
                signedRequest
            );
        }

        [Fact]
        public void ItemsApiAsString()
        {
            // Arrange
            string expectedSignedRequest = "{\"security\":{\"consumer_key\":\"yis0TYCu7U9V4o7M\",\"domain\":\"localhost\",\"timestamp\":\"20190129-1649\",\"user_id\":\"demo_student\",\"signature\":\"6c617f6f5be983af46c5549277e04969d80597f54be4fdc9743cff7c61b3a4e1\"},\"request\":{\"activity_template_id\":\"demo-activity-1\",\"activity_id\":\"my-demo-activity\",\"name\":\"Demo Activity\",\"session_id\":\"be3fdeff-a92b-4a4e-9ba6-82ce303b62c0\",\"user_id\":\"demo_student\",\"config\":{\"administration\":{\"pwd\":\"5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8\"}}}}";
            string service = "items";
            string security = "{\"consumer_key\":\"yis0TYCu7U9V4o7M\",\"domain\":\"localhost\",\"timestamp\":\"20190129-1649\"}";
            string secret = consumerSecret;
            string request = "{\"activity_template_id\":\"demo-activity-1\",\"activity_id\":\"my-demo-activity\",\"name\":\"Demo Activity\",\"session_id\":\"be3fdeff-a92b-4a4e-9ba6-82ce303b62c0\",\"user_id\":\"demo_student\",\"config\":{ \"administration\":{ \"pwd\":\"5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8\"}}}";

            // Act
            Init init = new Init(service, security, secret, request);
            string signedRequest = init.generate();

            // Assert
            Assert.Equal(
                expectedSignedRequest,
                signedRequest
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
    }
}
