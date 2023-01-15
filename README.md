# QuickBooksOnlineAspNetCore7Minimal
Minimal ASP.NET Core 7.0 application using QuickBooks Online API v3 and OpenID Connect

This is a minimal ASP.NET Core 7.0 application using QuickBooks Online API.

## Prerequisites
ASP .NET Core 7.0 SDK
QuickBooks App & Keys

## Setup
1. Clone the repository
2. Open the solution in Visual Studio 2019 or later
3. Create project secrets for the following
```   
"QuickBooksOnline": {
	"clientid": "[from your QuickBooks App]",
	"clientsecret": "[from your QuickBooks App]"
}
```

## Of Note
This is not production ready but serves as an example of how you can create a QBO customer in ASP.Net Core 7

## References
https://developer.intuit.com/app/developer/qbo/docs/develop/authentication-and-authorization/openid-connect

## Roadmap
- [ ] Use factory pattern for HttpClient
- [ ] Add more API calls
- [ ] Add more examples
- [ ] Add more documentation

