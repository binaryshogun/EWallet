using EWallet.Commands;
using EWallet.Stores;
using EWallet.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace EWallet.Tests
{
    [TestClass]
    public class RegistrationTests
    {
        private RegisterUserCommand registerUserCommand;
        private RegistrationViewModel viewModel;
        private UserStore userStore;

        public void InitializeData()
        {
            userStore = new UserStore();
            viewModel = new RegistrationViewModel(userStore, null, null, null);
            registerUserCommand = new RegisterUserCommand(viewModel, null, userStore);
        }

        public void ChangeUserDataAndTest(string login, string password)
        {
            InitializeData();
            viewModel.Login = login;
            viewModel.Password = password;
            viewModel.RepeatedPassword = password;
            AlreadyRegistredTest();
        }
        public async void PassRegistrationTest()
        {
            await Task.Run(registerUserCommand.RegisterUserInDatabase);
            Assert.AreEqual(userStore.CurrentUser != null, true);
        }

        public async void AlreadyRegistredTest()
        {
            await Task.Run(registerUserCommand.RegisterUserInDatabase);
            Assert.AreEqual(userStore.CurrentUser == null, true);
        }

        [TestMethod()]
        public void RegisterAlexanderTest()
            => ChangeUserDataAndTest("alexander", "default");

        [TestMethod()]
        public void RegisterObiWanTest()
            => ChangeUserDataAndTest("obi-wan", "therehello");
    }
}
