## This is the README for the CompanyAPI

## Requirements

Create a REST API application with the .NET Core framework using C# as your programming
language. 

Assume the database is a SQL Server database.

The application's database is as described by the entity relational diagram attached.
As for the REST API, these are the endpoints that we would like to see implemented:

- Insertion of a new employee
- Editing an existing employee
- Removing a employee
- List of all employees

![database requirements](Database.png)

### Tools used on development

- Windows machine
- IDE - JetBrain Rider
- Microsoft SQL Server Management
- SQL 2022 Express edition

## Requirements to run the project

The project requires a local DB in order for it to be executed.
The SQL server and DB name are set on the appsettings.json - this can be adapted for your solution.

Once the SQL server is up and running, there are already migrations available for you to run.
You can find the migration in the migrations folder and execute them by:

``dotnet ef database update``

If for some reason you want to run new migrations, you can do so by running 

``dotnet ef migrations add [Name of Migration]``

the migration needs to be included in the CompanyApi.csproj - you can check the MainMigrations example there.

Now you can build and run the dotnet project by 

``dotnet build``

``dotnet run``

As soon as the project is up and running you can find the swagger file on either

http://localhost:8000/swagger/index.html or https://localhost:7043/swagger/index.html

Given that we want to populate the DB with JobTitle and Status, I have create an endpoint
for demo purposes which will populate this values. 

This is a **GET** on **Employee/GenerateData**.

The endpoint will generate the following data points:


| Job Titles  |
|-------------|
| Developer   |
| PM          |
| HR Partner  |
| CEO         |
| Ex-Employee |
| No relation |
| Undefined   |


| Status      |
|-------------|
| Working     |
| Vacation    |
| Ex-Employee |
| No relation |
| Undefined   |


## Troubleshooting Section

####  1) System.Globalization.CultureNotFoundException: Only the invariant culture is supported in globalization-invariant mode.

Change the following properties -> build_property.InvariantGlobalization = false

### 2) Issue with the migrations not being listed

Solution https://stackoverflow.com/questions/67286637/migration-not-listed-by-net-core-cli

