# Azure Durable Functions Demo

[![Open in vscode.dev](https://img.shields.io/badge/Open%20in-vscode.dev-blue)][1]

[1]: https://vscode.dev/github/lluppesms/durable.function.demo/

![azd Compatible](./Docs/images/AZD_Compatible.png)

[![deploy.infra.and.function](https://github.com/lluppesms/durable.function.demo/actions/workflows/deploy-infra-function.yml/badge.svg)](https://github.com/lluppesms/durable.function.demo/actions/workflows/deploy-infra-function.yml)

[![Build Status](https://dev.azure.com/lyleluppes/GitHubDevOps/_apis/build/status%2Fdurable.Demo%2Fdurable.demo.infra.and.app?branchName=main)](https://dev.azure.com/lyleluppes/GitHubDevOps/_build/latest?definitionId=32&branchName=main)

---

## Overview

This project is example of using an Azure Durable Functions to perform a long running action.  It simulates an Multi-Factor Authentication trigger in which a Durable Function program is triggered when a user logs in, the user is sent a code via an SMS from that Durable Function, the user  enters that code on a web page, and then the web page would call the long-running Durable Function to validate the code is correct, and then the Durable Function would return that validation to the web page, allowing the user to log in. We don't know how long that interaction would take, hence the use of a Durable Function.

This program is written as a C# .NET 6.0 application.

---

## Deployment Options

There are several ways that this application can be deployed.  For testing purposes, a developer can do an "azd up" command to deploy the app to their test subscription immediately.  The Azure DevOps Pipelines and GitHub Actions are also set up to deploy the application to Azure. All of the resources needed are created via Bicep in one resource group.

1. [Deploy using AZD Command Line Tool](./Docs/AzdDeploy.md)

2. [Deploy using Azure DevOps](./.azdo/readme.md)

3. [Deploy using GitHub Actions](./.github/workflows-readme.md)

---

## Running the Application

[How to run the application](./Docs/RunApplication.md)
