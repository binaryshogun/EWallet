using _119_Karpovich.Models;
using _119_Karpovich.Services;
using _119_Karpovich.Stores;
using _119_Karpovich.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _119_Karpovich.Commands
{
    public class AuthorizeUserCommand : CommandBase
    {
        private readonly AuthorizationViewModel _viewModel;
        private readonly NavigationStore _navigationStore;

        public AuthorizeUserCommand(AuthorizationViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            using (var db = new WalletEntities())
            {
                int length = 16;
                if (_viewModel.Password.Length < 16)
                    length = _viewModel.Password.Length;

                string tempPassword = _viewModel.Password.GetHashCode().ToString().Substring(0, length);

                User user = db.User
                    .AsNoTracking()
                    .FirstOrDefault(
                    u => u.Login == _viewModel.Login
                    && u.Password == tempPassword);

                if (user != null)
                    _navigationStore.CurrentViewModel = new AccountViewModel(user, _navigationStore);
                else
                    MessageBox.Show("Неверно введён логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            
        }
    }
}
