# GitHub Actions Deploy

This project has GitHub Actions workflows that will build and deploy the application. The workflows are defined in the `.github/workflows` folder.

## Steps to Deploy App to Azure

- [Create Repository Secrets](/Docs/CreateGitHubSecrets.md)
- Trigger the workflow "deploy.infra" in GitHub to deploy the infrastructure
- Trigger the workflow "deploy.app" in GitHub to deploy the app
