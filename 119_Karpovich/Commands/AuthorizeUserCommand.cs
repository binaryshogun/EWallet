using _119_Karpovich.Models;
using _119_Karpovich.Stores;
using _119_Karpovich.ViewModels;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace _119_Karpovich.Commands
{
    public class AuthorizeUserCommand : CommandBase
    {
        private readonly AuthorizationViewModel _viewModel;
        private readonly NavigationStore _navigationStore;
        private readonly WalletEntities dataBase;

        public AuthorizeUserCommand(AuthorizationViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
            dataBase = new WalletEntities();
        }

        public override void Execute(object parameter)
        {
            using (dataBase)
            {
                int length = 16;
                if (_viewModel.Password.Length < 16)
                    length = _viewModel.Password.Length;

                string tempPassword = GetHash(_viewModel.Password, length);

                User user = dataBase.User
                    .AsNoTracking()
                    .FirstOrDefault(
                    u => u.Login == _viewModel.Login
                    && (u.Password == tempPassword || u.Password == "default"));

                if (user != null)
                    _navigationStore.CurrentViewModel = new AccountViewModel(user, _navigationStore);
                else
                    MessageBox.Show("Неверно введён логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public static string GetHash(string password, int length)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2"))).Substring(0, length);
            }
        }
    }
}
