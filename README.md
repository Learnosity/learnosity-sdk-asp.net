# Learnosity SDK - ASP.NET / C#

Include this package into your own codebase to ease integration with any of the Learnosity APIs.

## Installation
Installation should be as simple as possible, there are no external dependancies and this package should integrate with any existing codebase.

### git clone

```
git clone git@github.com:Learnosity/learnosity-sdk-php.git
```

If you don't have an SSH key loaded into github you can clone via HTTPS (not recommended)

```
git clone https://github.com/Learnosity/learnosity-sdk-php.git
```

### Examples
To load any of the example, it's easier to use the PHP local server (assumes PHP >= 5.4)

```
cd learnosity-sdk-php examples
php -S localhost:5555
```

You can play with the `examples` folder, and review the code to get a feel for how things work. In production you'll likely just use the contents of `src` in your own project. Eg:

```
/yourProjectRoot
  /src
  /vendor
    /learnosity-sdk-php/src/LearnositySdk
```

With this in mind you'll likely have this in your `.gitignore`

```
learnosity-sdk-php/src/examples
learnosity-sdk-php/src/*.md
```

### Autoload

This packages follows the PSR code convention which includes namespaces and import statements. Add the LearnositySdk autoloader or use your own (use *LearnositySdk* as the namespace):

```
require_once __DIR__ . '/LearnositySdk/autoload.php';
```


## Usage

### Init

The Init class is used to create the necessary *security* and *request* details used to integrate with a Learnosity API. Most often this will be a JavaScript object.

The Init constructor takes up to 5 arguments:

 * [string]  service type
 * [array]   security details (**no secret**)
 * [string]  secret
 * [request] request details *(optional)*
 * [string]  action *(optional)*

```
// Instantiate the SDK Init class with your security and request data:
$Init = new Init(
   'questions',
   [
       'consumer_key' => 'yis0TYCu7U9V4o7M',
       'domain'       => 'localhost',
       'user_id'      => 'demo_student'
   ],
   'superfragilisticexpialidocious',
   [
       'type'      => 'local_practice',
       'state'     => 'initial',
       'questions' => [
           [
               "response_id"        => "60005",
               "type"               => "association",
               "stimulus"           => "Match the cities to the parent nation.",
               "stimulus_list"      => ["London", "Dublin", "Paris", "Sydney"],
               "possible_responses" => ["Australia", "France", "Ireland", "England"],
               "validation" => [
                   "valid_responses" => [
                       ["England"], ["Ireland"], ["France"], ["Australia"]
                   ]
               ]
           ]
       ]
   ]
);

// Call the generate() method to retrieve a JavaScript object
$request = $Init->generate();

// Pass the object to the initialisation of any Learnosity API
LearnosityApp.init($request);
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
An array^ that includes your *consumer_key* but does not include your *secret*. The SDK sets defaults for you, but valid options are:

* consumer_key
* domain (optional)
* timestamp (optional)
* user_id (optional)

^Note – the SDK accepts JSON and native PHP arrays.

**secret**<br>
Your secret key, as provided by Learnosity.

**request**<br>
An optional associative array^ of data relevant to the API being used. This will be any data minus the security details that you would normally use to initialise an API.

^Note – the SDK accepts JSON and native PHP arrays.

**action**<br>
An optional string used only if integrating with the Data API. Valid options are:

* get
* set
* update
* delete

<hr>

### Remote

The Remote class is used to make server side, cross domain requests. Think of it as a cURL wrapper.

You'll call either get() or post() with the following arguments:

* [string] URL
* [array]  Data payload
* [array]  Options

```
// Instantiate the SDK Remote class:
$Remote = new Remote();
// Call get() or post() with a URL:
$response = $Remote->get('http://schemas.learnosity.com/stable/questions/templates');

// getBody() gives you to body of the request
$requestPacket = $response->getBody();
```

#### Arguments

**URL**<br>
A string URL, including schema and path. Eg:

```
https://schemas.learnosity.com/stable/questions/templates
```

**Data**<br>
An optional array of data to be sent as a payload. For GET it will be a URL encoded query string.

**Options**<br>
An optional array of [cURL parameters](http://www.php.net/manual/en/curl.constants.php).

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

## Version
Version v0.1.0 - June 2014