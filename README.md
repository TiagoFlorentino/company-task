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

### DB Section

dotnet ef migrations add InitialCreate
dotnet ef database update
SELECT name FROM sys.databases;



## Troubleshooting Section

####  System.Globalization.CultureNotFoundException: Only the invariant culture is supported in globalization-invariant mode.

Change the following properties -> build_property.InvariantGlobalization = false

### Issue with the migrations not being listed

Solution https://stackoverflow.com/questions/67286637/migration-not-listed-by-net-core-cli

