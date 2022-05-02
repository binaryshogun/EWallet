using _119_Karpovich.Models;
using _119_Karpovich.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace _119_Karpovich.Commands
{
    /// <summary>
    /// Команда выполнения денежной операции.
    /// </summary>
    internal class DoOperationCommand : CommandBase
    {
        #region Fields
        private readonly AccountViewModel viewmodel;
        private readonly Dictionary<string, Action<string, double>> operations;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует команду выполнения денежной операции.
        /// </summary>
        /// <param name="viewmodel">ViewModel страницы аккаунта пользователя.</param>
        public DoOperationCommand(AccountViewModel viewmodel)
        {
            this.viewmodel = viewmodel;
            operations = new Dictionary<string, Action<string, double>>()
            {
                { "Перевод пользователю", Remmitance },
                { "Вывод средств", Withdraw},
                { "Пополнение баланса", TopUp }
            };
        }
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter)
        {
            using (var dataBase = new WalletEntities())
            {
                string messageBoxMessage = $"Вы хотите выполнить {viewmodel.SelectedService.ToLower()}?\n" +
                    $"Карта получателя: {viewmodel.CardNumber}?\n" +
                    $"Сумма операции: {viewmodel.OperationBalance}\n";

                if (MessageBox.Show(messageBoxMessage, "Подтвердите операцию", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        operations[viewmodel.SelectedService]?.Invoke(viewmodel.CardNumber.Replace(" ", ""), viewmodel.OperationBalance);
            }        
        }

        /// <summary>
        /// Перевод денег другому пользователю системы.
        /// </summary>
        /// <param name="cardNumber">Номер карты пользователя системы.</param>
        /// <param name="operationBalance">Сумма операции.</param>
        private void Remmitance(string cardNumber, double operationBalance)
        {
            if (operationBalance > viewmodel.Balance)
            {
                MessageBox.Show("На счёте недостаточно средств!", "Отмена операции", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            using (var dataBase = new WalletEntities())
            {
                Card card = dataBase.Card.AsNoTracking().FirstOrDefault(c => c.Number == cardNumber);
                if (card != null)
                {
                    User user = dataBase.User.AsNoTracking().FirstOrDefault(u => u.ID == card.UserID);
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

        /// <summary>
        /// Вывод средств из системы.
        /// </summary>
        /// <param name="cardNumber">Номер карты зачисления.</param>
        /// <param name="operationBalance">Сумма операции.</param>
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

        /// <summary>
        /// Пополнение счёта в системе.
        /// </summary>
        /// <param name="cardNumber">Номер карты списания.</param>
        /// <param name="operationBalance">Сумма операции.</param>
        private void TopUp(string cardNumber, double operationBalance)
        {
            viewmodel.Balance += operationBalance;
            viewmodel.user.UpdateUserBalance(operationBalance);
        }
        #endregion
    }
}
