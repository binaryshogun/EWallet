using _119_Karpovich.Models;
using _119_Karpovich.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace _119_Karpovich.Commands
{
    internal class DoOperationCommand : CommandBase
    {
        private AccountViewModel viewmodel;
        private Dictionary<string, Action<string, double>> operations;
        private readonly WalletEntities dataBase;

        public DoOperationCommand(AccountViewModel viewmodel)
        {
            this.viewmodel = viewmodel;

            operations = new Dictionary<string, Action<string, double>>()
            {
                { "Перевод пользователю", Remmitance },
                { "Вывод средств", Withdraw},
                { "Пополнение баланса", TopUp }
            };

            dataBase = new WalletEntities();
        }

        public override void Execute(object parameter)
        {
            if (MessageBox.Show($"Вы хотите выполнить {viewmodel.SelectedService.ToLower()} на сумму {viewmodel.OperationBalance}?",
                "Подтвердите операцию", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            operations[viewmodel.SelectedService]?.Invoke(viewmodel.CardNumber.Replace(" ", ""), viewmodel.OperationBalance);
        }

        private void Remmitance(string cardNumber, double operationBalance)
        {
            if (operationBalance > viewmodel.Balance)
            {
                MessageBox.Show("На счёте недостаточно средств!", "Отмена операции", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            using (WalletEntities db = new WalletEntities())
            {
                Card card = db.Card.AsNoTracking().FirstOrDefault(c => c.Number == cardNumber);
                if (card != null)
                {
                    User user = db.User.AsNoTracking().FirstOrDefault(u => u.ID == card.UserID);
                    user.UpdateUserBalance(operationBalance);
                    viewmodel.user.UpdateUserBalance(-operationBalance);
                    viewmodel.Balance -= operationBalance;
                }
                else
                    MessageBox.Show("Пользователь с данным номером карты не найден в системе!", 
                        "Ошибка операции", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Information);
            }
        }

        private void Withdraw(string cardNumber, double operationBalance)
        {
            if (operationBalance > viewmodel.Balance)
            {
                MessageBox.Show("На счёте недостаточно средств!", "Отмена операции", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            viewmodel.Balance -= operationBalance;
            viewmodel.user.UpdateUserBalance(-operationBalance);
            return;
        }

        private void TopUp(string cardNumber, double operationBalance)
        {
            viewmodel.Balance += operationBalance;
            viewmodel.user.UpdateUserBalance(operationBalance);
        }
    }
}
