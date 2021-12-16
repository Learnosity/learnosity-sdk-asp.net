# Learnosity ASP.NET / C# SDK: Reference guide

This repository contains a full .NET solution.

The LearnositySDK library helps you to generate the necessary request JSON for the following Learnosity APIs:

 * Assess API
 * Author API
 * Data API
 * Items API
 * Questions API
 * Reports API
 * Schemas API

In addition, data and schemas services use `Request/Remote` class to perform HTTP Request, and fetch needed data. The data service can also use `Request/DataApi` wrapper making pagination requests easier.

## Contents

The solution has three folders:
 * LearnositySDK
 * LearnositySDKUnitTests
 * LearnositySDKIntegrationTests

### LearnositySDK

This project is a class library built in compatibility with .NET Standard 2.0.

This project uses only one external dependency: [Newtonsoft.Json](https://www.newtonsoft.com/json)

## Usage

### Init class

The `Request/Init` class is used to generate the necessary security and request data (in the correct format) to integrate with any of the Learnosity API services.

The Init constructor takes up to 5 arguments:

 * [string]               service type
 * [string/JsonObject]    security details (**no secret**)
 * [string]               secret
 * [string/JsonObject]    request details *(optional)*
 * [string]               action *(optional)*

```
// prepare all the params
string service = "items";

JsonObject security = new JsonObject();
security.set("consumer_key", "yis0TYCu7U9V4o7M");
security.set("domain", "localhost");
security.set("user_id", "$ANONYMIZED_USER_ID");

string secret = "74c5fd430cf1242a527f6223aebd42d30464be22";

JsonObject request = new JsonObject();
request.set("activity_template_id", "demo-activity-1");
request.set("activity_id", "my-demo-activity");
request.set("name", "Demo Activity");
request.set("course_id", "demo_yis0TYCu7U9V4o7M");
request.set("session_id", Uuid.generate());
request.set("user_id", "$ANONYMIZED_USER_ID");

// Instantiate Init class
Init init = new Init(service, security, secret, request);

// Call the generate() method to retrieve a JavaScript object
string JavaScriptObject = init.generate();
```

On the JavaScript side:

```
// Pass the object to the initialisation of any Learnosity API
LearnosityApp.init(JavaScriptObject);
```

#### Arguments

**service**<br>
A string representing the Learnosity service (API) you want to integrate with. Valid options are:

* assess
* author
* data
* items
* questions
* reports

**security**<br>
An associative array^ that includes your *consumer_key* but does not include your *secret*. The SDK sets defaults for you, but valid options are:

* consumer_key
* domain (optional)
* timestamp (optional)
* user_id (optional)

^Note – the SDK accepts JSON strings or JsonObject objects.

**secret**<br>
Your secret key, as provided by Learnosity.

**request**<br>
An optional associative array^ of data relevant to the API being used. This will be any data minus the security details that you would normally use to initialise an API.

^Note – the SDK accepts JSON strings or JsonObject objects.

**action**<br>
An optional string used only if integrating with the Data API. Valid options are:

* get
* set
* update
* delete

<hr>

### Remote class

The Remote class is used to make server side, cross domain requests. Think of it as a HTTPWebRequest or WebClient wrapper.

You'll call either get() or post() with the following arguments:

 * [string]                             URL
 * [string/Dictionary<string, string>]  Data payload
 * [JsonObject]                         Options

```
string url = "http://schemas.learnosity.com/v1/questions/templates";
Remote remote = new Remote();
remote.get(url);
string body = remote.getBody();
```

#### Arguments

**URL**<br>
A string URL, including schema and path. Eg:

```
https://schemas.learnosity.com/v1/questions/templates
```

**Data**<br>
An optional associative array^ of data to be sent as a payload. For GET it will be a URL encoded query string.

^Note – the SDK accepts query strings or Dictionary<string, string> objects.

**Options**<br>
An optional associative array^ of request parameters. Valid options are: int timeout (seconds), JsonObject headers, string encoding

^Note – the SDK accepts JsonObject objects.

### Remote methods
The following methods are available after making a `get()` or `post()`.

**getBody()**<br>
Returns the body of the response payload.

**getError()**<br>
Returns an array that includes the error code and message (if an error was thrown)

**getHeader()**<br>
Currently only returns the *content_type* header of the response.

**getSize()**<br>
Returns the size of the response payload in bytes.

**getStatusCode()**<br>
Returns the HTTP status code of the response.

### DataApi class

This class serves as a wrapper for data API calls. It has two public methods:

**request()**<br>

Allows you to perform single request. For now data API calls are limited to 1000 records, which means, that subsequent calls can be required.

You can pass `data.meta.next` token in your requestPacket to load next set of records. See also `requestRecursive` method.

Check `Examples.Data.DataApi()` for an example.

**requestRecursive()**<br>

Allows you to perform subsequent requests recursively, returning the chunk of data after each recursion.

Chunks are passed into callback function defined by following delegate:

```
public delegate bool ProcessData(string data);
```

Check `Examples.Data.DataApiRecursive()` for an example.

### JsonObject class

Serves as an simple implementation of PHP associative arrays. It provides many overloaded `get()` and `set()` methods to allow you create your flexible objects.

Constructor accepts only one parameter: `bool isArray = false`. It's required to set this flag to `true` if your object should behave like an array.

Example with both - objects and arrays:

```
JsonObject session_ids = new JsonObject(true);
session_ids.set("AC023456-2C73-44DC-82DA28894FCBC3BF");

JsonObject report = new JsonObject();
report.set("id", "report-1");
report.set("type", "sessions-summary");
report.set("user_id", "$ANONYMIZED_USER_ID");
report.set("session_ids", session_ids);

JsonObject reports = new JsonObject(true);
reports.set(report);

JsonObject request = new JsonObject();
request.set("reports", reports);
```

### JsonObjectFactory class

If you already have your data in JSON string, simply run `fromString()` method, to convert it to JsonObject instance:

```
string json = "[\"a\", \"b\"]";
JsonObject jo = JsonObjectFactory.fromString(json);
```

## Version
You can find the latest version of the SDK as a self-contained ZIP file in the [GitHub Releases](https://github.com/Learnosity/learnosity-sdk-asp.net/releases).

## Examples

Each service has its own example - you can find them in the `Examples` folder of the LearnositySDK project. To run them simply invoke static `Simple` method. There is also the quick-start guide and tutorial, covered in [README.md](README.md).

## Further reading
Thanks for reading to the end! Find more information about developing an app with Learnosity on our documentation sites: 
<ul>
<li><a href="http://help.learnosity.com">help.learnosity.com</a> -- general help portal and tutorials,
<li><a href="http://reference.learnosity.com">reference.learnosity.com</a> -- developer reference site, and
<li><a href="http://authorguide.learnosity.com">authorguide.learnosity.com</a> -- authoring documentation for content creators.
</ul>

Back to [README.md](README.md)