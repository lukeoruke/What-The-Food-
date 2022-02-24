// See https://aka.ms/new-console-template for more information
/*
 * CURRENTLY ONLY A DEMO FILE
 */
using Microsoft.Extensions.DependencyInjection;
using User;
using Class1;
using Console_Runner;
using LogAndArchive;
using Console_Runner.DAL;
using Console_Runner.AMRModel;

IDataAccess dal = new DummyDaL();
ILogger log = new Logging();
UM um = new UM(dal, log);
Archiving archiver = new Archiving();
archiver.archiveStartThread();
//Role_User user = new Role_User();
//Console.WriteLine(user.ToString());


//Console.WriteLine(adam.ToString());

    Account admin = new Account();
    admin.Email = "Admin";
    admin.Password = "pass";
    //admin.accessLevel = 2;
    admin.isActive = true;
    admin.Fname = "matt";
    admin.Lname = "q";
    user_permissions permissions = new user_permissions(dal);
    permissions.defaultAdminPermissions(admin.Email);
    um.UserSignUp(admin);
    AMR adminAMR = new AMR(admin, true, 63, 175, 25, ActivityLevel.Daily);
Console.WriteLine(adminAMR.CalculateAMR());
    Environment.Exit(0);