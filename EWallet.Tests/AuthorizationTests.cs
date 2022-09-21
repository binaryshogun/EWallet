using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EWallet.Commands;
using EWallet.ViewModels;
using EWallet.Stores;

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
            authorizationViewModel = new AuthorizationViewModel(null, null, null, null);
            authorizeUserCommand = new AuthorizeUserCommand(authorizationViewModel, null, userStore);
        }

        public void ChangeUserDataAndTest(string login, string password)
        {
            InitializeData();
            authorizationViewModel.Login = login;
            authorizationViewModel.Password = password;
            authorizeUserCommand.Execute(null);
            AuthorizationTest();
        }
        
        public void AuthorizationTest() 
            => Assert.AreEqual(authorizeUserCommand.IsUserAuthorized, true);

        [TestMethod()]
        public void AuthorizeAlexanderTest() 
            => ChangeUserDataAndTest("alexander", "default");

        [TestMethod()]
        public void AuthorizeObiWanTest()
            => ChangeUserDataAndTest("obi-wan", "therehello");
    }
}
