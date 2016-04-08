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
            Remote remote = new Remote();
            return remote.post(url, parameters);
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
