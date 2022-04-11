// See https://aka.ms/new-console-template for more information
/*
 * CURRENTLY ONLY A DEMO FILE
 */
using Microsoft.Extensions.DependencyInjection;
using Console_Runner;



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