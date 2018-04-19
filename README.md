# Soap WS Client
Soap WS Client is a dynamic soap webservice client for .net applications written in C#.

# Example
To use the client you have to add the dependency using nuget
```
Package name: empalacios.soap_ws_client.dll
Version: 1.0.1
```

Then import the namespaces needed:
```
using soap_ws_client;
using System.Collections.Generic;
```

Then create an instace of the client, passing the service url and the namespace of the content (you can get this by getting the wsdl):
```
string ws_url = "https://www.w3schools.com/xml/tempconvert.asmx";
string ws_namespace = ""https://www.w3schools.com/xml/"";
Soap1_1WsClient client = new Soap1_1WsClient(ws_url, ws_namespace);
```

Call the function or method you want and pass the parameters:
```
Dictionary<string, object> parameters = new Dictionary<string, object>();
parameters.Add("Fahrenheit", "10");
string result = client.callServiceFunction("FahrenheitToCelsius", parameters);
client.callServiceMethod("FahrenheitToCelsius", parameters);
```

Functions receive the response and methods don't.
