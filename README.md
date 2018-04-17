# Soap WS Client
Soap WS Client is a dynamic soap webservice client for .net applications written in C#.

# Example
To use the client you have to add the dependency and import it first:
```
using soap_ws_client;
using System.Collections.Generic;
```

Then create an instace of the class, set the parameters and you're ready
```
string ws_url = "https://www.w3schools.com/xml/tempconvert.asmx";
string ws_namespace = ""https://www.w3schools.com/xml/"";
Soap1_1WsClient client = new Soap1_1WsClient(ws_url, ws_namespace);
Dictionary<string, object> parameters = new Dictionary<string, object>();
parameters.Add("Fahrenheit", "10");
string result = client.callService("FahrenheitToCelsius", parameters);
```