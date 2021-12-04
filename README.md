# ss-dotnet-api
DotNet API with EF Core and some handy middleware

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