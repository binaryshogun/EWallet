using EWallet.Commands;
using EWallet.Stores;
using EWallet.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EWallet.Tests
{
    [TestClass]
    public class RegistrationTests
    {
        private RegisterUserCommand registerUserCommand;
        private RegistrationViewModel viewModel;

        public void InitializeData()
        {
            viewModel = new RegistrationViewModel(null, null);
            registerUserCommand = new RegisterUserCommand(viewModel, null);
        }

        public void ChangeUserDataAndTest(string login, string password)
        {
            InitializeData();
            viewModel.Login = login;
            viewModel.Password = password;
            viewModel.RepeatedPassword = password;
            registerUserCommand.Execute(null);
            AlreadyRegistredTest();
        }
        public void PassRegistrationTest()
            => Assert.AreEqual(registerUserCommand.IsUserRegistered, true);

        public void AlreadyRegistredTest()
            => Assert.AreEqual(registerUserCommand.IsUserRegistered, false);

        [TestMethod()]
        public void RegisterAlexanderTest()
            => ChangeUserDataAndTest("alexander", "default");

        [TestMethod()]
        public void RegisterObiWanTest()
            => ChangeUserDataAndTest("obi-wan", "therehello");
    }
}
