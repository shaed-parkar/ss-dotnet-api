# ss-dotnet-api
DotNet API with EF Core and some handy middleware

[![Build Status](https://shaedparkar.visualstudio.com/ss-dotnet-api/_apis/build/status/ss-dotnet-api-ci?branchName=main)](https://shaedparkar.visualstudio.com/ss-dotnet-api/_build/latest?definitionId=9&branchName=main)

## Adding Migrations

### Install EF Core CLI

Run the following command in a terminal window

```console
dotnet tool install --global dotnet-ef
```

### Add a migration

Open a terminal in the same directory as the SS.DAL project. To add a migration run the following statement

```console
dotnet ef migrations add <Migration Name>
```

### Update the database with latest migrations

Open a terminal in the same directory as the SS.DAL project. To add a migration run the following statement

```console
dotnet ef database update
```

## Tests

### Running all tests via CLI

Open a terminal in the same directory as the SSApi Solution and run the following command

```console
dotnet test --no-build --logger:trx --results-directory Coverage \
"/p:CollectCoverage=true" \
"/p:CoverletOutput=../Coverage/" \
"/p:MergeWith=../Coverage/coverage.json" \
"/p:CoverletOutputFormat=\"json,cobertura\"" 
```

### Running all tests in Docker

Open a terminal at the root level of the repository and run the following command

```console
docker-compose -f "tests/docker-compose.tests.yml" up --build --abort-on-container-exit
```

## Creating a report

Ensure the report generator tool is installed

```console
dotnet tool install -g dotnet-reportgenerator-globaltool
```

From inside a console, run the following command in the same directory as the SSApi Solution

```console
reportgenerator -reports:"./Coverage/coverage.cobertura.xml" -targetdir:"./Coverage/Report" -reporttypes:Html
```