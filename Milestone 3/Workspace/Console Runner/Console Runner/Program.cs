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
    admin.accessLevel = 2;
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

    if (currentUser != null)
    {
        loggedIn = true;
    }
    string input = "";
    
    while (input != "exit" && loggedIn)
    {
        

        Console.WriteLine("Enter one of the following commands to demo functionality: \nusersignup\nUserDelete\nUserReadData\nShowAllUsers\nDisableAccount\nEnableAccount\npromoteToAdmin\nsignOut\nExit");

        Console.WriteLine("..................................................................");
        input = Console.ReadLine();
        input = input.ToLower();
        if (input == "usersignup")// Create account
        {
            um.UserSignUp();
        }
        else
        if (input == "userdelete")//Deletes an account
        {
            um.UserDelete(currentUser);
        }
        else
        if (input == "userreaddata") //reads data from a specified account
        {
            um.getUserAcc();
        }
        else
        if (input == "userupdatedata") //Updates data of a specified account
        {
            um.UserUpdateData(currentUser);
        }
        else
        if (input == "showallusers") // prints out all user data in the database.
        {
            um.GetAllUsers();
        }
        else
        if (input == "disableaccount") // will prompt user for targetPK and disable said account if conditions allow
        {
            um.DisableAccount(currentUser);
        }
        else
        if (input == "enableaccount") // will ask for targetPK and enable said account if conditions allow
        {
            um.EnableAccount(currentUser);
        }
        else
        if(input == "promotetoadmin") // will ask for targetPK and promote a user to admin if conditions allow
        {
            um.promoteToAdmin(currentUser);
        }
        if (input == "signout") // signs out of current ccount session
        {
            loggedIn = false;
            currentUser = null;
            break;
        }
        else
            if(input != "exit")
            Console.WriteLine("not valid command");

        /* checks if the current sessions email is null. This is to catch cases where user information was modified mid session 
         * checks if user permissions have changed.
         */
        if (currentUser.Email == null)
        {
            Console.WriteLine("email was null");
            loggedIn = false;
        }
        else
        if (!(um.getUserAcc(currentUser.Email).isAdmin() == currentUser.isAdmin()) || !(um.getUserAcc(currentUser.Email).isActive == currentUser.isActive))
        {
            Console.WriteLine((!um.getUserAcc(currentUser.Email).Equals(currentUser)));
            Console.WriteLine("account object was modified");
            loggedIn = false;
        }
    }

    Environment.Exit(0);
}