# doingitwithdapper
A simple .Net Core Application using Dapper based Services

This is mainly an example on using dapper over entity framework.

It shows Dappers core features for mapping results to typed collections etc, task based programming from controllers to results

It uses a simple custom interface for setting up IDb connection, this is injected into each service and allows execution of the dapper methods.

This is a simple application to show dappers useful features instead of writing EF models and linq statement, main advantage is dapper is much fater than Entity Framework.

This application also contains startup code to be hosted in Azure using Azure AD Authentication settings, (values not included for obvious reasons), alternatively you can register your applcication with Azure AD in the azure portal.


Infrastructure folder contains the core code, you would normally find in its own layer but in this example everything is contained within the one .net core application. 

Services folder contains the code that calls the dapper methods, these services can easily be called by web api


