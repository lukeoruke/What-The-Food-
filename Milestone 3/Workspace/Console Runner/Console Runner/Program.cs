// See https://aka.ms/new-console-template for more information
/*
 * CURRENTLY ONLY A DEMO FILE
 */
using Microsoft.Extensions.DependencyInjection;
using User;
using Class1;
using Console_Runner;

UM um = new UM();
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

    um.UserSignUp(admin);

    Environment.Exit(0);