using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Console_Runner.AccountService;
using Console_Runner.Account.AccountInterfaces;

namespace Test.UM
{
    public  class NewsShould
    {

        private readonly IAccountNews _newsGateway = new MemNews();

        [Fact]
        public async void IncrementHealthBiasSuccess()
        {
            EFNews nm = new EFNews();
            Account acc = new Account();
            acc.UserID = 2;
            Assert.True(await nm.IncrementBias(acc.UserID));
            Console.WriteLine("Succesfully incremented");
        }
        [Fact]
        public async void DecrementHealthBiasSuccess()
        {
            EFNews nm = new EFNews();
            Account acc = new Account();
            acc.UserID = 2;
            Assert.True(await nm.DecrementBias(acc.UserID));
            Console.WriteLine("Succesfully decremented");
        }
        [Fact]
        public async void GettHealthBiasSuccess()
        {
            EFNews nm = new EFNews();
            Account acc = new Account();
            acc.UserID = 2;
            Assert.NotEqual(-1,await nm.GetBias(acc.UserID));
            Console.WriteLine("Succesfully retrived bias");
        }
    }
}
