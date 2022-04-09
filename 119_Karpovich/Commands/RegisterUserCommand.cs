using _119_Karpovich.Models;
using _119_Karpovich.Services;
using _119_Karpovich.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _119_Karpovich.Commands
{
    public class RegisterUserCommand : CommandBase
    {
        private readonly RegistrationViewModel _viewModel;
        private readonly NavigationService<AuthorizationViewModel> _navigationService;

        public RegisterUserCommand(RegistrationViewModel viewModel, NavigationService<AuthorizationViewModel> navigationService)
        {
            _viewModel = viewModel;
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            using (var db = new WalletEntities())
            {
                try
                {
                    int length = 16;
                    if (_viewModel.Password.Length < 16)
                        length = _viewModel.Password.Length;

                    User user = new User()
                    {
                        Login = _viewModel.Login,
                        Password = _viewModel.Password.GetHashCode().ToString().Substring(0, length),
                        RoleID = 1,
                        Balance = 0
                    };

                    db.User.Add(user);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                MessageBox.Show("Пользователь успешно зарегистрирован!\n" +
                        "Перенаправление на страницу авторизации...", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            _navigationService.Navigate();
        }
    }
}
