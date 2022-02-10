using Class1;
using Console_Runner;
using User;
using Xunit;
using Console_Runner.DAL;
using LogAndArchive;
//---------------------------------NOTICE, THIS FILE NOT BEEN UPDATED WITH THE CHANGES TO AUTHORIZATION AND WILL NOT WORK AS EXPECTED-----------------------------------
namespace UnitTest
{
    public class UnitTest1
    {


        [Fact]
        public void CreateUserSuccess()
        {

            //Arange
            IDataAccess dal = new DummyDaL();
            ILogger log = new Logging();
            UM um = new UM(dal, log);

            string tester = "unitTester";
            Account acc = new Account();
            acc.Email = tester;
            acc.Password = "password1";
            acc.Fname = "fname";
            acc.Lname = "lname";
            //Act
            um.UserSignUp(acc);
            //Assert
            Assert.True(dal.accountExists(acc.Email));
                
        }

        [Fact]
        // trys to delete a user, while acting as if an admin executed the command
        public void deleteUserSuccess()
        {
            //Arange
            IDataAccess dal = new DummyDaL();
            ILogger log = new Logging();
            UM um = new UM(dal, log);

            Account admin = new Account();
            admin.Email = "deleteUserSuccessAdminEmail";
            admin.Password = "password1";
            admin.Fname = "fname";
            admin.Lname = "lname";
            user_permissions permissions = new();
            permissions.defualtAdminPermissions(admin.Email);
            dal.addAccount(admin);

            Account acc = new Account();
            acc.Email = "deleteUserSuccessEmail";
            acc.Password = "t";
            dal.addAccount(acc);
            //Act
            um.UserDelete(admin, acc.Email);
            //Assert
            Assert.True(dal.accountExists(acc.Email) == false);
        }
        [Fact]
        public void updateSuccess()
        {
            //Arange
            IDataAccess dal = new DummyDaL();
            ILogger log = new Logging();
            UM um = new UM(dal, log);

            Account admin = new Account();
            admin.Email = "updateUserSuccessAdminEmail";
            admin.Password = "password1";
            admin.Fname = "fname";
            admin.Lname = "lname";
            user_permissions permissions = new();
            permissions.defualtAdminPermissions(admin.Email);
            um.UserSignUp(admin);


            Account acc = new Account();
            acc.Email = "updateUserSuccessEmail";
            acc.Password = "t";
            um.UserSignUp(acc);

            string nName = "new name";
            string nlName = "new last name";
            string nPassword = "NewPassword";

            //act
            um.UserUpdateData(admin, acc.Email, nName, nlName, nPassword);
            acc = dal.getAccount(acc.Email);
            //Assert
            Assert.True(acc.Fname == nName && acc.Lname == nlName && acc.Password == nPassword);
        }

    }
}
