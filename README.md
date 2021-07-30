## Introduction

This project contains the source for a custom workflow connector. Custom connectors allow us to build logic application functionality we can reuse, and allow us to write C# that we can use 
to perform any actions we like. 

Connectors perform either _triggers_ or _actions_. A _trigger_ can be used to start a sequence of _actions_ when an event is detected, an action is generally some work to perform. 

## Contributing
Please see our [Contributing] (./docs/CONTRIBUTING.md) guide to understand our process (maintainer list, PR Process) before making contributions to this repository.

## Installing locally

Locally Workflow Designer is using host.json from workflow-designtime folder. And loads dlls from ExtensionBundle specified there. host.json example:

```json
{
  "version": "2.0",
  "extensionBundle": {
    "id": "Microsoft.Azure.Functions.ExtensionBundle.Workflows",
    "version": "[1.*, 2.0.0)"
  }
}
```

Microsoft.Azure.Functions.ExtensionBundle.Workflows loads dlls from:
C:\Users\<username>\.azure-functions-core-tools\Functions\ExtensionBundles\Microsoft.Azure.Functions.ExtensionBundle.Workflows\<version>\bin.

So to make designer work we need to copy our Connecter dll there and update extensions.json in the same location to include the Connector.

Update extensions.json here: 
C:\Users\<username>\.azure-functions-core-tools\Functions\ExtensionBundles\Microsoft.Azure.Functions.ExtensionBundle.Workflows\<version>\bin.
To include new Cosmos connector add at the end of the file the following (check the namespace and connector Startup class):

``` json
{
    "name":  "CosmosDbServiceProvider",
    "typeName":  "Microsoft.Azure.Workflows.ServiceProvider.Extensions.CosmosDB.Action.CosmosDbTriggerStartup, Microsoft.Azure.Workflows.ServiceProvider.Extensions.CosmosDB.Action, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
}
```

2. To make runtime work locally the func should be run from the following place:
<GIT repos>\<logicApp repo>\src\bin\Debug\netcoreapp3.1.

NOTE:
When building connector locally:
1. Pack the connector as Nuget package (*.nupkg).
2. Publish the package locally to "C:\test\nuget" source. For example:
nuget.exe add Asos.Azure.Workflows.ServiceProviders.Cosmos.Extension.1.0.9.nupkg -source C:\test\nuget
3. Update global NuGet.config to use local nuget source (local project nuget.config can be used too).
4. Build the Logic App project to restore dependencies.


## Installing from Nuget

If you want to install the compiled nuget package into a project, follow these steps

1. Go to VSTS, generate a PAT that has rights to Read from Package sources

2. Copy the PAT that is generated

3. Run the following command to add the nuget source locally, switching the PAT token

dotnet nuget add source https://pkgs.dev.azure.com/<Organisation>/<ProjectId>/_packaging/<FeedName>/nuget/v3/index.json -n <FeedName> -u <userName> -p <your-pat-token> --store-password-in-clear-text