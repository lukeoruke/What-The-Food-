using Class1;
using Console_Runner;
using User;
using Xunit;
//---------------------------------NOTICE, THIS FILE NOT BEEN UPDATED WITH THE CHANGES TO AUTHORIZATION AND WILL NOT WORK AS EXPECTED-----------------------------------
namespace UnitTest
{
    public class UnitTest1
    {
        public class Testing
        {
            UM um = new UM();
            Context context = new Context();
            [Fact]
            public void CreateUserSuccess()
            {
                //Arange
                string tester = "unitTester";
                Account acc = new Account();
                acc.Email = tester;
                acc.Password = "password1";
                acc.Fname = "fname";
                acc.Lname = "lname";
                //Act
                um.UserSignUp(acc);
                //Assert
                Assert.True(context.accounts.Find(acc.Email) != null);
                
            }

            [Fact]
            // trys to delete a user, while acting as if an admin executed the command
            public void deleteUserSuccess()
            {
                //Arange
                string tester = "UnitTestUser";
                Account acc = new Account();
                acc.Email = tester;
                acc.Password = "password1";
                acc.Fname = "fname";
                acc.Lname = "lname";
                user_permissions permissions = new();
                permissions.defualtAdminPermissions(acc.Email);
                um.UserSignUp(acc);
                //Act
                um.UserDelete(acc, tester);
                //Assert
                Assert.True(context.accounts.Find("UnitTestUser") == null);
            }
            [Fact]
            public void updateSuccess()
            {
                //Arange
                Account admin = new();
                admin.Email = "admin email";
                admin.Password = "admin pass";
                admin.Fname = "Test";
                admin.Lname = "Test";
                user_permissions permissions = new();
                permissions.defualtAdminPermissions(admin.Email);
                um.UserSignUp(admin);
                
                string tester = "newdude";
                Account acc = new Account();
                acc.Email = tester;
                acc.Password = "password1";
                acc.Fname = "fname";
                acc.Lname = "lname";
                um.UserSignUp(acc);
                string nName = "new name";
                string nlName = "new last name";
                string nPassword = "NewPassword";
                //act
                um.UserUpdateData(admin, acc.Email, nName, nlName, nPassword);
                acc = context.accounts.Find(acc.Email);
                //Assert
                Assert.True(acc.Fname == nName && acc.Lname == nlName && acc.Password == nPassword);
            }

            public Account makeAdmin()
            {
                if (context.accounts.Find("Admin") != null)
                {
                    context.accounts.Remove(context.accounts.Find("Admin"));
                }
                Account currentUser = new Account();
                currentUser.Email = "Admin";
                currentUser.Password = "pass";
                currentUser.Fname = "fname";
                currentUser.Lname = "lname";
                um.UserSignUp(currentUser);
                return currentUser;
            }
        }

    }
}
