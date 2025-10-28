using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnositySDK.Request;
using LearnositySDK.Utils;

namespace LearnositySDK.Request
{
    /// <summary>
    /// Delegate serving as a callback function for `requestRecursive`
    /// </summary>
    /// <param name="data">data in JSON format</param>
    /// <returns>whether processing was successful</returns>
    public delegate bool ProcessData(string data);

    /// <summary>
    /// Learnosity SDK - DataApi
    ///
    /// Used to make requests to the Learnosity Data API - including generating the security packet
    /// </summary>
    public class DataApi
    {
        /// <summary>
        /// Makes a single request to the data api
        /// </summary>
        /// <param name="url">URL to send the request</param>
        /// <param name="securityPacketJson">Security details</param>
        /// <param name="secret">Private key</param>
        /// <param name="requestPacketJson">Request packet</param>
        /// <param name="action">Action for the request</param>
        /// <returns>Instance of the Remote class</returns>
        public Remote request(string url, string securityPacketJson, string secret, string requestPacketJson = null, string action = null)
        {
            return this.handleRequest(url, new Init("data", securityPacketJson, secret, requestPacketJson, action));
        }

        /// <summary>
        /// Makes a single request to the data api
        /// </summary>
        /// <param name="url">URL to send the request</param>
        /// <param name="securityPacket">Security details</param>
        /// <param name="secret">Private key</param>
        /// <param name="requestPacket">Request packet</param>
        /// <param name="action">Action for the request</param>
        /// <returns>Instance of the Remote class</returns>
        public Remote request(string url, JsonObject securityPacket, string secret, JsonObject requestPacket = null, string action = null)
        {
            return this.handleRequest(url, new Init("data", securityPacket, secret, requestPacket, action));
        }

        /// <summary>
        /// Handling request
        /// </summary>
        /// <param name="url">URL to send the request</param>
        /// <param name="init"></param>
        /// <returns></returns>
        private Remote handleRequest(string url, Init init)
        {
            string parameters = init.generate();

            // Extract metadata for internal headers
            JsonObject metadata = this.extractMetadata(url, init);

            // Create options with metadata headers
            JsonObject options = new JsonObject();
            JsonObject headers = new JsonObject();

            if (metadata.getString("consumer") != null)
            {
                headers.set("X-Learnosity-Consumer", metadata.getString("consumer"));
            }

            if (metadata.getString("action") != null)
            {
                headers.set("X-Learnosity-Action", metadata.getString("action"));
            }

            options.set("headers", headers);

            Remote remote = new Remote();
            return remote.post(url, parameters, options);
        }

        /// <summary>
        /// Extracts metadata (consumer and action) from the request
        /// </summary>
        /// <param name="url">The request URL</param>
        /// <param name="init">The Init object containing security and action information</param>
        /// <returns>JsonObject containing consumer and action metadata</returns>
        private JsonObject extractMetadata(string url, Init init)
        {
            JsonObject metadata = new JsonObject();

            // Extract consumer key from security packet
            string consumerKey = init.getConsumerKey();
            if (!Tools.empty(consumerKey))
            {
                metadata.set("consumer", consumerKey);
            }

            // Extract and format action metadata
            string actionMetadata = this.buildActionMetadata(url, init.getAction());
            if (!Tools.empty(actionMetadata))
            {
                metadata.set("action", actionMetadata);
            }

            return metadata;
        }

        /// <summary>
        /// Builds the action metadata string in the format: {method}_{endpoint}
        /// Example: "get_/itembank/items" or "set_/itembank/activities"
        /// </summary>
        /// <param name="url">The full request URL</param>
        /// <param name="action">The action (e.g., "get", "set")</param>
        /// <returns>Formatted action string</returns>
        private string buildActionMetadata(string url, string action)
        {
            if (Tools.empty(url))
            {
                return null;
            }

            // Extract the endpoint path from the URL
            // URL format: https://data.learnosity.com/v1/itembank/items
            // We want: /itembank/items
            string endpoint = this.extractEndpoint(url);

            if (Tools.empty(endpoint))
            {
                return null;
            }

            // If action is provided, prepend it with underscore
            if (!Tools.empty(action))
            {
                return action + "_" + endpoint;
            }

            return endpoint;
        }

        /// <summary>
        /// Extracts the endpoint path from a Data API URL
        /// </summary>
        /// <param name="url">The full URL</param>
        /// <returns>The endpoint path (e.g., "/itembank/items")</returns>
        /// <remarks>
        /// Handles URLs with or without version:
        /// - https://data.learnosity.com/v1/itembank/items -> /itembank/items
        /// - https://data.learnosity.com/latest-lts/itembank/items -> /itembank/items
        /// - https://data.learnosity.com/v2025.1.LTS/itembank/items -> /itembank/items
        /// - https://data.learnosity.com/itembank/items -> /itembank/items
        /// </remarks>
        private string extractEndpoint(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                string path = uri.AbsolutePath;

                // Remove leading slash if present
                if (path.StartsWith("/"))
                {
                    path = path.Substring(1);
                }

                // Split the path into segments
                string[] segments = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                if (segments.Length == 0)
                {
                    return null;
                }

                // Check if the first segment is a version identifier
                // Version patterns: v1, v2, v2024.3.LTS, latest, latest-lts, etc.
                // We need to be more specific to avoid false positives like /validateItembanks
                string firstSegment = segments[0];
                bool isVersion = this.isVersionSegment(firstSegment);

                // If first segment is a version, skip it and return the rest
                // Otherwise, return the entire path
                int startIndex = isVersion ? 1 : 0;

                if (startIndex >= segments.Length)
                {
                    return null;
                }

                // Reconstruct the endpoint path
                string endpoint = "/" + string.Join("/", segments, startIndex, segments.Length - startIndex);
                return endpoint;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Checks if a path segment is a version identifier
        /// </summary>
        /// <param name="segment">The path segment to check</param>
        /// <returns>True if the segment is a version identifier, false otherwise</returns>
        /// <remarks>
        /// Valid version patterns (based on Learnosity API version list):
        /// - latest: latest
        /// - developer: developer
        /// - latest-lts: latest-lts
        /// - v{number}: v0, v1, v2, etc.
        /// - v{number}.{number}: v1.22, v1.84, etc.
        /// - v{year}.{minor}.LTS: v2024.3.LTS, v2025.1.LTS, etc.
        /// - v{year}.{minor}.preview{number}: v2022.3.preview1, etc.
        /// Invalid (should return false):
        /// - validateItembanks (starts with 'v' but not a version)
        /// - verify (starts with 'v' but not a version)
        /// </remarks>
        private bool isVersionSegment(string segment)
        {
            if (string.IsNullOrEmpty(segment))
            {
                return false;
            }

            // Check for known version keywords
            if (segment.Equals("latest", StringComparison.OrdinalIgnoreCase) ||
                segment.Equals("latest-lts", StringComparison.OrdinalIgnoreCase) ||
                segment.Equals("developer", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // Check for version patterns starting with 'v'
            // Valid: v0, v1, v2, v1.22, v1.84, v2024.3.LTS, v2025.1.LTS, v2022.3.preview1
            // Invalid: validateItembanks, verify
            if (segment.StartsWith("v", StringComparison.OrdinalIgnoreCase))
            {
                // Must be at least 2 characters (v + digit)
                if (segment.Length < 2)
                {
                    return false;
                }

                // The character after 'v' must be a digit
                char secondChar = segment[1];
                if (char.IsDigit(secondChar))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Makes a single request to the data api
        /// </summary>
        /// <param name="url">URL to send the request</param>
        /// <param name="securityPacketJson">Security details</param>
        /// <param name="secret">Private key</param>
        /// <param name="requestPacketJson">Request packet</param>
        /// <param name="action">Action for the request</param>
        /// <param name="callback">Callback to process JSON data. Returning false in callback breaks from the loop of recursive requests.</param>
        /// <returns>Instance of the Remote class</returns>
        public JsonObject requestRecursive(string url, string securityPacketJson, string secret, string requestPacketJson = null, string action = null, ProcessData callback = null)
        {
            JsonObject securityPacket = JsonObjectFactory.fromString(securityPacketJson);
            JsonObject requestPacket = null;

            if (requestPacketJson != null)
            {
                requestPacket = JsonObjectFactory.fromString(requestPacketJson);
            }

            return this.requestRecursive(url, securityPacket, secret, requestPacket, action, callback);
        }

        /// <summary>
        /// Makes a single request to the data api
        /// </summary>
        /// <param name="url">URL to send the request</param>
        /// <param name="securityPacket">Security details</param>
        /// <param name="secret">Private key</param>
        /// <param name="requestPacket">Request packet</param>
        /// <param name="action">Action for the request</param>
        /// <param name="callback">Callback to process JSON data. Returning false in callback breaks from the loop of recursive requests.</param>
        /// <returns>Instance of the Remote class</returns>
        public JsonObject requestRecursive(string url, JsonObject securityPacket, string secret, JsonObject requestPacket = null, string action = null, ProcessData callback = null)
        {
            return this.handleRequestRecursive(url, securityPacket, secret, requestPacket, action, callback);
        }

        /// <summary>
        /// Makes a single request to the data api
        /// </summary>
        /// <param name="url">URL to send the request</param>
        /// <param name="securityPacket">Security details</param>
        /// <param name="secret">Private key</param>
        /// <param name="requestPacket">Request packet</param>
        /// <param name="action">Action for the request</param>
        /// <param name="callback">Callback to process JSON data. Returning false in callback breaks from the loop of recursive requests.</param>
        /// <returns>Instance of the Remote class</returns>
        private JsonObject handleRequestRecursive(string url, JsonObject securityPacket, string secret, JsonObject requestPacket = null, string action = null, ProcessData callback = null)
        {
            JsonObject response = new JsonObject(true);
            JsonObject data, meta;
            Remote request;
            int recursion = 0;

            do
            {
                request = this.request(url, securityPacket, secret, requestPacket, action);
                data = JsonObjectFactory.fromString(request.getBody());

                if (data == null)
                {
                    throw new Exception("Request wasn't successful");
                }

                meta = data.getJsonObject("meta");

                recursion++;

                if(meta.getBool("status"))
                {
                    if (callback != null)
                    {
                        // return if callback returns false
                        if (!callback(data.toJson()))
                        {
                            return response;
                        }
                    }
                    else
                    {
                        response = Tools.array_merge(response, data.getJsonObject("data"));
                    }
                }
                else {
                    throw new Exception(Json.encode(data));
                }

                if(Tools.array_key_exists("next", meta) && !Tools.empty(data.getJsonObject("data")))
                {
                    string next = meta.getString("next");
                    if (next != null)
                    {
                        requestPacket.set("next", next);
                        securityPacket.remove("signature");
                    }
                    else
                    {
                        throw new Exception("data['meta']['next'] is not a string");
                    }
                }
                else
                {
                    requestPacket.remove("next");
                }
            }
            while(Tools.array_key_exists("next", requestPacket));

            return response;
        }
    }
}
