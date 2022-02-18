# Infrastructure

The infrastructure is managed using Pulumi.

## Retrieving config

Execute the command below inside of a terminal to retrieve the config for a given stack

```console
pulumi config refresh -s <stack name>
```

## Setting up the pipeline

Create an access token for pulumi [here](https://app.pulumi.com/account/tokens)

Run the following command in a terminal

```console
az pipelines variable create --name pulumi.access.token --pipeline-name ss-dotnet-api-infra --value <access_token_goes_here>
```