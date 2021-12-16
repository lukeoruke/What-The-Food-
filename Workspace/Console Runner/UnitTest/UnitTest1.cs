using Class1;
using Console_Runner;
using User;
using Xunit;

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
                Account acc = makeTestUser(tester);
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
                Account currentUser = makeAdmin();
                string tester = "unitTester";
                makeTestUser(tester);
                //Act
                um.UserDelete(currentUser, tester);
                //Assert
                Assert.True(context.accounts.Find("UnitTestUser") == null);
            }
            [Fact]
            public void updateSuccess()
            {
                //Arange
                Account currentUser = makeAdmin();
                string newTester = "newTester";
                Account testUser = makeTestUser(newTester);
                string nName = "new name";
                string nlName = "new last name";
                string nPassword = "NewPassword";
                //act
                um.UserUpdateData(currentUser, testUser.Email, nName, nlName, nPassword);
                testUser = context.accounts.Find(testUser.Email);
                //Assert
                Assert.True(testUser.Fname == nName && testUser.Lname == nlName && testUser.Password == nPassword);
            }

            public Account makeAdmin()
            {
                if (context.accounts.Find("UnitTestUser") != null)
                {
                    context.accounts.Remove(context.accounts.Find("UnitTestUser"));
                }
                Account currentUser = new Account();
                currentUser.Email = "UnitTestAdmin";
                currentUser.Password = "password1LetsMakeThisReallyLong4Security";
                currentUser.Fname = "fname";
                currentUser.Lname = "lname";
                currentUser.accessLevel = 2;
                um.UserSignUp(currentUser);
                return currentUser;
            }
            public Account makeTestUser(string pk)
            {
                if (context.accounts.Find(pk) != null)
                {
                    context.accounts.Remove(context.accounts.Find(pk));
                }
                Account currentUser = new Account();
                currentUser.Email = pk;
                currentUser.Password = "password1";
                currentUser.Fname = "fname";
                currentUser.Lname = "lname";
                currentUser.accessLevel = 1;
                um.UserSignUp(currentUser);
                return currentUser;
            }
        }

    }
}
