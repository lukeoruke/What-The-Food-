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

if(um.AdminCount() != 5)
{
    Account admin = new Account();
    admin.Email = "Admin";
    admin.Password = "pass";
    //admin.accessLevel = 2;
    admin.isActive = true;
    admin.Fname = "matt";
    admin.Lname = "q";

    um.UserSignUp(admin);
    //Console.WriteLine(admin.role.ToString());

}

bool loggedIn = false;

while (!loggedIn)
{
    Console.Write("Please sign in. \nEmail: ");
    string id = Console.ReadLine();
    Console.Write("\nPassword: ");
    string password = Console.ReadLine();
    Account currentUser = um.signIn(id, password);
    Environment.Exit(0);
}