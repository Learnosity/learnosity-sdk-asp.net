using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnositySDK.Utils;
using System.Web;

namespace LearnositySDK.Request
{
    /// <summary>
    /// Learnosity SDK - Init
    /// 
    /// Used to generate the necessary security and request data (in the correct format) to integrate with any of the Learnosity API services.
    /// </summary>
    public class Init
    {
        /// <summary>
        /// Which Learnosity service to generate a request packet for.
        /// Valid values (see also `validServices`):
        ///  - assess
        ///  - author
        ///  - data
        ///  - items
        ///  - questions
        ///  - reports
        /// </summary>
        private string service;

        /// <summary>
        /// The consumer secret as provided by Learnosity. This is your private key known only by the client (you) and Learnosity, which must not be exposed either by sending it to the browser or across the network. It should never be distributed publicly.
        /// </summary>
        private string secret;

        /// <summary>
        /// An associative array of security details. This typically contains:
        /// - consumer_key
        /// - domain (optional depending on which service is being intialised)
        /// - timestamp (optional)
        /// - user_id (optional depending on which service is being intialised)
        /// 
        /// It's important that the consumer secret is NOT a part of this array.
        /// </summary>
        private JsonObject securityPacket;

        /// <summary>
        /// An optional associative array of request parameters used as part of the service (API) initialisation.
        /// </summary>
        private JsonObject requestPacket;

        /// <summary>
        /// If `requestPacket` is used, `requestString` will be the string (JSON) representation of that. It's used to create the signature and returned as part of the service initialisation data.
        /// </summary>
        private string requestString;

        /// <summary>
        /// An optional value used to define what type of request is being made. This is only required for certain requests made to the Data API (http://docs.learnosity.com/dataapi/)
        /// </summary>
        private string action;

        /// <summary>
        /// Most services add the request packet (if passed) to the signature for security reasons. This flag can override that behaviour for services that don't require this.
        /// </summary>
        private bool signRequestData;

        /// <summary>
        /// Keynames that are valid in the securityPacket, they are also in the correct order for signature generation.
        /// </summary>
        private string[] validSecurityKeys;

        /// <summary>
        /// Service names that are valid for `$service`
        /// </summary>
        private string[] validServices;

        /// <summary>
        /// The algorithm used in the hashing function to create the signature.
        /// </summary>
        private string algorithm;

        /// <summary>
        /// Instantiate this class with all security and request data. It will be used to create a signature.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="securityPacketJson"></param>
        /// <param name="secret"></param>
        /// <param name="requestPacketJson"></param>
        /// <param name="action"></param>
        public Init(string service, string securityPacketJson, string secret, string requestPacketJson = null, string action = null)
        {
            this.Initialize(service, securityPacketJson, secret, requestPacketJson, action);
        }

        /// <summary>
        /// Instantiate this class with all security and request data. It will be used to create a signature.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="securityPacket"></param>
        /// <param name="secret"></param>
        /// <param name="requestPacket"></param>
        /// <param name="action"></param>
        public Init(string service, JsonObject securityPacket, string secret, JsonObject requestPacket = null, string action = null)
        {
            this.Initialize(service, securityPacket, secret, requestPacket, action);
        }

        /// <summary>
        /// Convert JSON strings into JsonObjects
        /// </summary>
        /// <param name="service"></param>
        /// <param name="securityPacketJson"></param>
        /// <param name="secret"></param>
        /// <param name="requestPacketJson"></param>
        /// <param name="action"></param>
        private void Initialize(string service, string securityPacketJson, string secret, string requestPacketJson = null, string action = null)
        {
            JsonObject securityPacket = JsonObjectFactory.fromString(securityPacketJson);
            JsonObject requestPacket;

            if (requestPacketJson == null)
            {
                requestPacket = null;
            }
            else
            {
                requestPacket = JsonObjectFactory.fromString(requestPacketJson);
            }

            this.Initialize(service, securityPacket, secret, requestPacket, action);
        }

        /// <summary>
        /// Perform initialization actions
        /// </summary>
        /// <param name="service"></param>
        /// <param name="securityPacket"></param>
        /// <param name="secret"></param>
        /// <param name="requestPacket"></param>
        /// <param name="action"></param>
        private void Initialize(string service, JsonObject securityPacket, string secret, JsonObject requestPacket = null, string action = null)
        {
            this.service = service;
            this.securityPacket = (JsonObject)securityPacket.Clone();
            this.secret = secret;
            this.requestPacket = requestPacket;
            this.action = action;

            this.signRequestData = true;
            this.validSecurityKeys = new string[5] { "consumer_key", "domain", "timestamp", "expires", "user_id" };
            this.validServices = new string[7] { "assess", "author", "data", "events", "items", "questions", "reports" };
            this.algorithm = "sha256";

            // We don't catch this Exception, as we can't `die` as in PHP
            //try
            //{
                this.validate(this.service, ref this.securityPacket, this.secret, this.requestPacket, this.action);
                this.requestString = this.generateRequestString();
                this.setServiceOptions();
                this.securityPacket.set("signature", this.generateSignature());
            /*}
            catch (Exception e)
            {
                
            }*/
        }

        /// <summary>
        /// Generate a JSON string from the requestPacket (array) or null if no requestPacket is required for this request
        /// </summary>
        /// <returns></returns>
        private string generateRequestString()
        {
            if (Tools.empty(this.requestPacket))
            {
                return null;
            }

            string request = this.requestPacket.toJson();

            if (Tools.empty(request))
            {
                throw new Exception("Invalid data, please check you request packet.");
            }
            
            return request;
        }

        /// <summary>
        /// Generate a signature hash for the request, this includes:
        /// - the security credentials
        /// - the `request` packet (a JSON string) if passed
        /// - the `action` value if passed
        /// </summary>
        /// <returns>A signature hash for the request authentication</returns>
        private string generateSignature()
        {
            List<string> signatureList = new List<string>();
            string temp = null;

            foreach (string key in this.validSecurityKeys)
            {
                temp = this.securityPacket.getString(key);
                if (temp != null)
                {
                    signatureList.Add(temp);
                }
            }

            signatureList.Add(this.secret);

            if (this.signRequestData && !Tools.empty(this.requestString))
            {
                signatureList.Add(this.requestString);
            }

            if (!Tools.empty(this.action))
            {
                signatureList.Add(this.action);
            }

            return this.hashValue(signatureList.ToArray());
        }

        /// <summary>
        /// Generate the data necessary to make a request to one of the Learnosity products/services.
        /// </summary>
        /// <returns></returns>
        public string generate()
        {
            JsonObject output = new JsonObject();

            switch (this.service)
            {
                case "assess":
                    // fall through
                case "author":
                    // fall through
                case "data":
                    // fall through
                case "items":
                    // fall through
                case "reports":

                    output.set("security", this.securityPacket);

                    if (!Tools.empty(this.requestPacket))
                    {
                        output.set("request", this.requestPacket);
                    }

                    if (!Tools.empty(this.action))
                    {
                        output.set("action", this.action);
                    }

                    if (this.service == "data")
                    {
                        return this.generateData(output);
                    }
                    else if(this.service == "assess")
                    {
                        output = this.generateAssess(output);
                    }

                    break;
                case "questions":

                    output = this.generateQuestions(output);

                    break;
                case "events":

                    output.set("security", this.securityPacket);
                    output.set("config", this.requestPacket);

                    break;
                default:
                    // do nothing
                    break;
            }
            
            return output.toJson();
        }

        /// <summary>
        /// Generate the data necessary to make a request to data service.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        private string generateData(JsonObject output)
        {
            StringBuilder sb = new StringBuilder();
            JsonObject obj = null;
            string str = "";
            
            obj = output.getJsonObject("security");
            if (!Tools.empty(obj))
            {
                sb.Append("security=" + HttpUtility.UrlEncode(obj.toJson()));
            }
            else
            {
                throw new Exception("`security` key is required but doesn't exist");
            }

            obj = output.getJsonObject("request");
            if (!Tools.empty(obj))
            {
                sb.Append("&request=" + HttpUtility.UrlEncode(obj.toJson()));
            }

            str = output.getString("action");
            if (!Tools.empty(str))
            {
                sb.Append("&action=" + HttpUtility.UrlEncode(str));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generate the data necessary to make a request to assess service.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        private JsonObject generateAssess(JsonObject output)
        {
            string type = "";
            Object outputObject = output.get("request", ref type);

            if (type == "JsonObject" || type == "JsonArray")
            {
                output = (JsonObject)outputObject;
            }
            else
            {
                throw new Exception("");
            }

            return output;
        }

        /// <summary>
        /// Generate the data necessary to make a request to quesetions service.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        private JsonObject generateQuestions(JsonObject output)
        {
            output = this.securityPacket;

            output.remove("domain");

            if (!Tools.empty(this.requestPacket))
            {
                output = JsonObjectFactory.merge(output, this.requestPacket, false, false);
            }

            return output;
        }

        /// <summary>
        /// Hash an array value
        /// </summary>
        /// <param name="value">An array to hash</param>
        /// <returns>The hashed string</returns>
        private string hashValue(string[] value)
        {
            string implode = Tools.implode("_", value);
            string hash = Tools.hash(this.algorithm, implode);
            return hash;
        }

        /// <summary>
        /// Set any options for services that aren't generic
        /// </summary>
        private void setServiceOptions()
        {
            switch (this.service)
            {
                case "assess":
                    // fall-through
                case "questions":
                    
                    this.signRequestData = false;

                    if (this.service == "assess" && Tools.array_key_exists("questionsApiActivity", this.requestPacket))
                    {
                        JsonObject questionsApi = this.requestPacket.getJsonObject("questionsApiActivity");
                        if (questionsApi == null)
                        {
                            throw new Exception("requestPacket['questionsApiActivity'] is not an array");
                        }

                        string domain = "assess.learnosity.com";
                        if (Tools.array_key_exists("domain", this.securityPacket))
                        {
                            domain = this.securityPacket.getString("domain");
                        }
                        else if (Tools.array_key_exists("domain", questionsApi))
                        {
                            domain = questionsApi.getString("domain");
                        }

                        JsonObject questionsApiActivity = new JsonObject();

                        List<string> signatureList = new List<string>();

                        signatureList.Add(this.securityPacket.getString("consumer_key"));
                        signatureList.Add(domain);
                        signatureList.Add(this.securityPacket.getString("timestamp"));

                        if (Tools.array_key_exists("expires", this.securityPacket))
                        {
                            signatureList.Add(this.securityPacket.getString("expires"));
                            questionsApiActivity.set("expires", this.securityPacket.getString("expires"));
                            questionsApi.remove("expires");
                        }

                        signatureList.Add(this.securityPacket.getString("user_id"));
                        signatureList.Add(this.secret);

                        string signature = this.hashValue(signatureList.ToArray());

                        questionsApiActivity.set("consumer_key", this.securityPacket.getString("consumer_key"));
                        questionsApiActivity.set("timestamp", this.securityPacket.getString("timestamp"));
                        questionsApiActivity.set("user_id", this.securityPacket.getString("user_id"));
                        questionsApiActivity.set("signature", signature);

                        questionsApi.remove("domain");
                        questionsApi.remove("consumer_key");
                        questionsApi.remove("timestamp");
                        questionsApi.remove("user_id");
                        questionsApi.remove("signature");

                        questionsApiActivity = JsonObjectFactory.merge(questionsApiActivity, questionsApi);

                        this.requestPacket.set("questionsApiActivity", questionsApiActivity);
                    }

                    break;
                case "items":
                    // fall-through
                case "reports":

                    if (!Tools.array_key_exists("user_id", this.securityPacket) && Tools.array_key_exists("user_id", this.requestPacket))
                    {
                        this.securityPacket.set("user_id", this.requestPacket.getString("user_id"));
                    }

                    break;
                case "events":

                    string consumer_key = this.securityPacket.getString("consumer_key");
                    JsonObject hashedUsers;

                    this.signRequestData = false;

                    JsonObject requestPackageUsers = this.requestPacket.getJsonObject("users");
                    if (requestPackageUsers != null) {
                        string[] users =  requestPackageUsers.getValuesArray();
                        if (users != null && users.Length > 0) {
                            hashedUsers = new JsonObject();
                            for (int i = 0; i < users.Length; i++) {
                                string user_id = users[i];
                                hashedUsers.set(user_id, Tools.hash(this.algorithm, user_id + this.secret));
                            }
                            this.requestPacket.set("users", hashedUsers);
                        }
                    }
  
                    break;
                default:
                    // do nothing
                    break;
            }
        }

        /// <summary>
        /// Validate the arguments passed to the constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="securityPacket"></param>
        /// <param name="secret"></param>
        /// <param name="requestPacket"></param>
        /// <param name="action"></param>
        public void validate(string service, ref JsonObject securityPacket, string secret, JsonObject requestPacket, string action)
        {
            if (service == "")
            {
                throw new Exception("The `service` argument wasn't found or was empty");
            }
            else if (!Tools.in_array(service.ToLower(), this.validServices))
            {
                throw new Exception("Provided service (" + service + ") is not valid");
            }

            string[] securityPacketKeys = Tools.array_keys(securityPacket);
            foreach (string key in securityPacketKeys)
            {
                if (!Tools.in_array(key, this.validSecurityKeys))
                {
                    throw new Exception("Invalid key found in security packet: " + key);
                }
            }

            if (!Tools.in_array("timestamp", securityPacketKeys))
            {
                securityPacket.set("timestamp", Tools.gmdate("yyyyMMdd'-'HHmm"));
            }

            if (service == "questions" && !Tools.array_key_exists("user_id", securityPacket))
            {
                throw new Exception("If using the question api, a user id needs to be specified");
            }

            if (Tools.empty(this.secret))
            {
                throw new Exception("The `secret` argument must be a valid string");
            }
        }
    }
}
