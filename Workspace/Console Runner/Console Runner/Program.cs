﻿// See https://aka.ms/new-console-template for more information
/*
 * CURRENTLY ONLY A DEMO FILE
 */
using Microsoft.Extensions.DependencyInjection;
using Console_Runner;
using Console_Runner.AccountService;

/*
IAccountGateway accountGateway = new EFAccountGateway();
IAuthorizationGateway authorizationGateway = new EFAuthorizationGateway();
IFlagGateway flagGateway = new EFFlagGateway();
AccountDBOperations accountService = new AccountDBOperations(accountGateway, authorizationGateway, flagGateway);
await accountService.UserSignUpAsync(new Account() { Email = "email@example.com", Password = "password", FName = "Guy", LName = "DudeBro"});
*/

[STAThread]
static void Main()
{
    Console.Write("application starting...");
}


/**
IDataAccess dal = new DummyDaL();
ILogger log = new Logging(dal);
UM um = new UM(dal, log); //UM has been removed from this scope
**/