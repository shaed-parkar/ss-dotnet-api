#!/bin/sh

# az devops configure --defaults organization=https://dev.azure.com/shaedparkar
# az devops configure --defaults project=ss-dotnet-api
# az pipelines create --name "ss-dotnet-api.ci"

# # Service connection required for non Azure Repos can be optionally provided in the command to run it non interatively
# # https://github.com/shaed-parkar/ss-dotnet-api.git
# az pipelines create --name 'ss-dotnet-api.ci' --description 'SS DotNet API - CI' --repository https://github.com/shaed-parkar/ss-dotnet-api --branch master --yml-path pipelines/ss-dotnet-api-ci.yaml

# az pipelines create --name 'ss-dotnet-api.ci' --description 'SS DotNet API - CI' --repository https://github.com/shaed-parkar/ss-dotnet-api --branch master

# $azDevopsInstalled=`az extension show --name azure-devops`
# if [ $azDevopsInstalled ];
# then
#     echo "Azure DevOps CLI installed"
# else
#     echo "Azure DevOps CLI NOT installed"
# fi

check_az_devops_cli_installed() {
    if az extension show --name azure-devops >/dev/null; then
        echo "Azure DevOps CLI installed"
    else
        echo "Installing Azure DevOps CLI"
        az extension add --name azure-devops
    fi
}

create_ci_pipeline() {
    if [ -z "$1" ]; then
        echo "The pipeline name is missing!"
        exit
    fi
    echo "Creating pipeline for $PipelineName"
    az pipelines create --name 'ss-dotnet-api-ci' --description 'CI pipeline for ss-dotnet-api-ci' --repository 'https://github.com/shaed-parkar/ss-dotnet-api' --repository-type github --skip-first-run --branch 'main' --yaml-path 'pipelines/ss-dotnet-api-ci.yaml'
}

create_infra_pipeline() {
    echo "Creating pipeline for ss-dotnet-api-infra"
    az pipelines create --name 'ss-dotnet-api-infra' --description 'CI pipeline for ss-dotnet-api-infra' --repository 'https://github.com/shaed-parkar/ss-dotnet-api' --repository-type github --skip-first-run --branch 'main' --yaml-path 'pipelines/ss-dotnet-api-infra.yaml'
}

check_az_devops_cli_installed
create_infra_pipeline
create_ci_pipeline ss-dotnet-api-ci
