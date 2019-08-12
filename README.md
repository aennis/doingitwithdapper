# doingitwithdapper
A simple .Net Core MVC Application using Dapper based Services

An example on how a light weight ORM like Dapper may be used over something like Entity Framework to give your application a performance boost when working sql databases.


It shows Dappers simplicity for executing it's core features, like mapping the results of a sql query or stored procedure to a typed collection without the use of sqlreader etc. 
This example also shows how dapper can be used in dontnet core, using task based programming to perform common MVC controllers actions.

It uses a simple custom interface for setting up IDb connection, this is injected into each service and allows execution of the dapper methods.

This is a simple application to show dappers useful features instead of writing EF models and linq statement, main advantage is dapper is much fater than Entity Framework.

This application also contains startup code to be hosted in Azure using Azure AD Authentication settings, (values not included for obvious reasons), alternatively you can register your applcication with Azure AD in the azure portal.


Infrastructure folder contains the core reusable code, you would normally find this code its own layer but in this example everything is contained within the one .net core application. 

Services folder contains the application specific services that call the dapper methods using the Infrastructure code, these services can easily be called by web api.

NOTE:
Dapper does not require this Infrastructure code to function, as shown on the dapper home page, a simple IDbConnection implemented conntection is all that's required.



Link to the dapper repo
https://github.com/StackExchange/Dapper

Dapper can also be found using nuget package manager
Install-Package Dapper -Version 1.50.5


