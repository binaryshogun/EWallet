using Microsoft.VisualStudio.TestTools.UnitTesting;
using EWallet.Commands;
using EWallet.ViewModels;
using EWallet.Stores;
using System.Threading.Tasks;

namespace EWallet.Tests
{
    [TestClass]
    public sealed class AuthorizationTests
    {
        private AuthorizationViewModel authorizationViewModel;
        private AuthorizeUserCommand authorizeUserCommand;
        private UserStore userStore;

        public void InitializeData()
        {
            userStore = new UserStore();
            authorizationViewModel = new AuthorizationViewModel(userStore, null, null, null);
            authorizeUserCommand = new AuthorizeUserCommand(authorizationViewModel, null, userStore);
        }

        public void ChangeUserDataAndTest(string login, string password)
        {
            InitializeData();
            authorizationViewModel.Login = login;
            authorizationViewModel.Password = password;
            PassAuthorizationTest();
        }
        
        public async void PassAuthorizationTest()
        {
            await Task.Run(() => authorizeUserCommand.Execute(null));
            Assert.AreEqual(userStore.CurrentUser != null, true);
        }

        [TestMethod()]
        public void AuthorizeAlexanderTest() 
            => ChangeUserDataAndTest("alexander", "default");

        [TestMethod()]
        public void AuthorizeObiWanTest()
            => ChangeUserDataAndTest("obi-wan", "therehello");
    }
}
