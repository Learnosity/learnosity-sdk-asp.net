using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using LearnositySDK.Utils;
using System.Diagnostics;

namespace LearnositySDK.Request
{
    /// <summary>
    /// Used to execute a request to a public endpoint. Useful as a cross domain proxy.
    /// </summary>
    public class Remote
    {
        private HttpWebRequest hr;
        private WebClient wc;
        private WebResponse result;

        private string responseBody = null;
        private JsonObject responseBodyJsonObject = null;
        private string errorMessage = null;
        private string errorCode = null;

        private string url = null;
        private string postStr = null;
        private JsonObject options = null;
        private string status = null;

        private int time = -1;

        /// <summary>
        /// Execute a resource request (GET) to an endpoint.
        /// </summary>
        /// <param name="url">Full URL of where to GET the request</param>
        /// <param name="data">Payload of request</param>
        /// <param name="options">Optional Curl options</param>
        /// <returns>The instance of this class</returns>
        public Remote get(string url, Dictionary<string, string> data = null, JsonObject options = null)
        {
            if (data != null)
            {
                if (url.IndexOf('?') != -1)
                {
                    url += toQueryString(data, "&");
                }
                else
                {
                    url += toQueryString(data);
                }
            }

            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                this.wc = new WebClient();
                this.responseBody = this.wc.DownloadString(url);
                this.status = "200";
                timer.Stop();
                this.time = timer.Elapsed.Seconds;
            }
            catch (WebException e)
            {
                this.status = e.Status.ToString();
                this.errorCode = this.status;
                this.errorMessage = e.Message;
            }

            this.process();
            return this;
        }

        /// <summary>
        /// Execute a resource request (POST) to an endpoint.
        /// </summary>
        /// <param name="url">Full URL of where to POST the request</param>
        /// <param name="data">Payload of request</param>
        /// <param name="options">Optional options. Valid items are: int timeout (seconds), JsonObject headers, string encoding</param>
        /// <returns>The instance of this class</returns>
        public Remote post(string url, string data = null, JsonObject options = null)
        {
            return this.request(url, data, options);
        }

        /// <summary>
        /// Execute a resource request (POST) to an endpoint.
        /// </summary>
        /// <param name="url">Full URL of where to POST the request</param>
        /// <param name="data">Payload of request</param>
        /// <param name="options">Optional options. Valid items are: int timeout (seconds), JsonObject headers, string encoding</param>
        /// <returns>The instance of this class</returns>
        public Remote post(string url, Dictionary<string, string> data, JsonObject options = null)
        {
            return this.request(url, data, options);
        }

        /// <summary>
        /// Converts set of key-value pairs into query string
        /// </summary>
        /// <param name="data"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private string toQueryString(Dictionary<string, string> data, string prefix = "?")
        {
            List<string> list = new List<string>();

            foreach (KeyValuePair<string, string> pair in data)
            {
                list.Add(string.Format("{0}={1}", HttpUtility.UrlEncode(pair.Key), HttpUtility.UrlEncode(pair.Value)));
            }

            return prefix + string.Join("&", list.ToArray());
        }

        /// <summary>
        /// Makes a POST request to an endpoint with an optional request payload and options.
        /// </summary>
        /// <param name="url">Full URL of where to POST the request</param>
        /// <param name="post">Payload of request</param>
        /// <param name="options">Optional options. Valid items are: int timeout (seconds), JsonObject headers, string encoding</param>
        /// <returns></returns>
        private Remote request(string url, Dictionary<string, string> post = null, JsonObject options = null)
        {
            string postStr = this.toQueryString(post, "");
            return this.request(url, postStr, options);
        }

        /// <summary>
        /// Makes a POST request to an endpoint with an optional request payload and options.
        /// </summary>
        /// <param name="url">Full URL of where to POST the request</param>
        /// <param name="post">Payload of request</param>
        /// <param name="options">Optional options. Valid items are: int timeout (seconds), JsonObject headers, string encoding</param>
        /// <returns></returns>
        private Remote request(string url, string post = null, JsonObject options = null)
        {
            this.url = url;
            this.postStr = post;
            this.options = options;

            if (options == null)
            {
                options = new JsonObject();
            }

            JsonObject defaults = new JsonObject();
            defaults.set("timeout", 10);
            defaults.set("readWriteTimeout", 50);
            defaults.set("headers", new JsonObject());
            defaults.set("encoding", "utf-8");

            options = Tools.array_merge(defaults, options, true);

            WebHeaderCollection headers = this.headersFromJsonObject(options.getJsonObject("headers"));

            this.hr = (HttpWebRequest)HttpWebRequest.Create(url);
            this.hr.Timeout = options.getInt("timeout") * 1000;
            this.hr.ReadWriteTimeout = options.getInt("readWriteTimeout") * 1000;
            this.hr.SendChunked = true;
            this.hr.TransferEncoding = options.getString("encoding");
            this.hr.AllowAutoRedirect = true;
            this.hr.MaximumAutomaticRedirections = 10;
            this.hr.Headers = headers;

            if (!Tools.empty(post))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(post);

                this.hr.Method = WebRequestMethods.Http.Post;
                this.hr.ContentType = "application/x-www-form-urlencoded";

                using (StreamWriter sw = new StreamWriter(this.hr.GetRequestStream()))
                {
                    sw.Write(post);
                }
            }

            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                using (this.result = this.hr.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(this.result.GetResponseStream()))
                    {
                        this.responseBody = sr.ReadToEnd();
                    }
                }
                this.status = "200";
                timer.Stop();
                this.time = timer.Elapsed.Seconds;
            }
            catch (WebException e)
            {
                using (this.result = e.Response)
                {
                    using (StreamReader sr = new StreamReader(this.result.GetResponseStream()))
                    {
                        this.responseBody = sr.ReadToEnd();
                    }
                }
                this.status = e.Status.ToString();
                this.errorCode = this.status;
                this.errorMessage = e.Message;
            }

            this.process();
            return this;
        }

        /// <summary>
        /// Processing responseBody into JsonObject.
        /// </summary>
        private void process()
        {
            this.responseBodyJsonObject = JsonObjectFactory.fromString(this.responseBody);
        }

        /// <summary>
        /// Returns the size of the request body
        /// </summary>
        /// <param name="bytes">in bytes or nicely formatted (default)</param>
        /// <returns></returns>
        public string getSize(bool inBytes = false)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(this.responseBody);

            if (inBytes)
            {
                return bytes.Length.ToString();
            }
            else
            {
                return Conversion.formatSizeUnits(bytes.Length);
            }
        }

        /// <summary>
        /// The HTTP status code of the request response
        /// </summary>
        /// <returns></returns>
        public string getStatusCode()
        {
            return this.status;
        }

        /// <summary>
        /// Returns part of the response headers
        /// </summary>
        /// <returns>Header from the response packet</returns>
        public string getHeader()
        {
            string type = "content_type";

            if (this.wc != null)
            {
                return this.wc.Headers.Get(type);
            }
            else if (this.result != null)
            {
                return this.result.Headers.Get(type);
            }

            return null;
        }

        /// <summary>
        /// Returns the body of the response payload as returned by the URL endpoint
        /// </summary>
        /// <returns>Typically a JSON string</returns>
        public string getBody()
        {
            return this.responseBody;
        }

        /// <summary>
        /// Returns an associative array detailing any errors that may have been throwing during an endpoint request
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> getError()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("code", this.errorCode);
            ret.Add("message", this.errorMessage);
            return ret;
        }

        /// <summary>
        /// Total transaction time in seconds for last transfer
        /// </summary>
        /// <returns></returns>
        public int getTimeTaken()
        {
            return this.time;
        }

        /// <summary>
        /// Returns a decoded JSON array
        /// </summary>
        /// <returns></returns>
        public JsonObject json()
        {
            return this.responseBodyJsonObject;
        }

        /// <summary>
        /// Converting JsonObject into WebHeaderCollection, to be able to use it in the request
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private WebHeaderCollection headersFromJsonObject(JsonObject obj)
        {
            List<string> values = obj.getValuesList();
            WebHeaderCollection headers = new WebHeaderCollection();

            foreach (string header in values)
            {
                headers.Add(header);
            }

            return headers;
        }
    }
}
