## Introduction

This project contains the source for a custom workflow connector. Custom connectors allow us to build logic application functionality we can reuse, and allow us to write C# that we can use 
to perform any actions we like. 

Connectors perform either _triggers_ or _actions_. A _trigger_ can be used to start a sequence of _actions_ when an event is detected, an action is generally some work to perform. 

## How to run the solution locally.
Detailed information is in README.md.
When contributing to this repository, please discuss the change you wish to make with the maintainers before making your contribution - details on how to get in touch are below - then we can discuss:
1. Planned implementation - what is this feature? How will it be implemented?
2. Scope of the change - what will be affected? Are there changes required to the pipeline or architecture?
3. Test plan - how will this feature be tested? Does it impact existing acceptance tests? Does it require additional load testing?

## Maintainers
DTC Integration team

## How to get in touch
📧 Email: dtcintegration@asos.com

## Testing
Project contains Unit tests and Integration tests. To be able to run them locally you need to install Azure CosmosDb emulator and update the CosmosConnectionString in Asos.Azure.Workflows.ServiceProviders.Cosmos.Extension.IntegrationTests/ConnectorParameters.cs.

## CI/CD Pipeline
Simple Build-Test pipeline is set up using Github actions. Currently it only runs for main branch.

## PullRequest
1. Create a feature/bug branch from main branch with convention `feature/my_change` prefix. 
2. Ensure you have latest code from main branch.
3. Pull Requests for every feature branch require        
  - Review and approval by one of the team members     
  - Passing tests
4. If required update the [README.md](../README.md) with details behind major changes, links and useful file locations.
5. Squash merge, branch delete.
