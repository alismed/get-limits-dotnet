## Get Salesforce Limits

List information about [limits](https://developer.salesforce.com/docs/atlas.en-us.api_rest.meta/api_rest/resources_limits.htm) in a Salesforce Org with [rest api](https://developer.salesforce.com/docs/atlas.en-us.api_rest.meta/api_rest/intro_what_is_rest_api.htm)

This use OAuth 2.0 protocol and [connected app](https://help.salesforce.com/articleView?id=connected_app_overview.htm&type=5) to authenticate before consume the api.

### Environments

 In the environment file is necessary define a few values to authenticate. Change [configuration file](appsettings.json) before use.

 * baseUrl
 * clientId
 * clientSecret
 * userName
 * password
 * securityToken
 * apiVersion
 * basePath

This project was created with this commands:
```shell
mkdir get-limits-dotnet && cd get-limits-dornet
dotnet new console 
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.FileExtensions
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package RestSharp
dotnet add package Newtonsoft.Json
```
