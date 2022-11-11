using EWallet.Commands;
using EWallet.Components;
using EWallet.Models;
using EWallet.Services;
using EWallet.Stores;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel страницы с отчетом о расходах.
    /// </summary>
    public sealed class ExpenseReportViewModel : ViewModelBase
    {
        #region Fields
        private readonly UserStore userStore;

        private readonly WalletEntities database;

        private List<DateTime> months;
		private DateTime selectedMonth;
		
		private List<Service> services;
		private Service selectedService;

		private List<Operation> operations;

		private double? sumOfExpenses;

		private SeriesCollection operationSeries;
        private bool isThereOperations;

        private bool isMonthSelected;
        private bool isServiceSelected;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ExpenseReportViewModel"/>.
        /// </summary>
        /// <param name="userStore"><see cref="UserStore"/>,
        /// содержащий данные о текущем пользователе.</param>
        /// <param name="accountNavigationService"><see cref="INavigationService"/>,
        /// совершающий переход на <see cref="AccountViewModel"/>.</param>
        public ExpenseReportViewModel(UserStore userStore, INavigationService accountNavigationService)
        {
            this.userStore = userStore;

            try
            {
                database = new WalletEntities();

                Months = SetUpMonths();
                Services = SetUpServices();

                SetUpViewModel();
            }
            catch (Exception e)
            {
                ErrorMessageBox.Show(e);
                accountNavigationService?.Navigate();
            }

            NavigateAccountCommand = new NavigateCommand(accountNavigationService);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Список месяцев для выбора.
        /// </summary>
        public List<DateTime> Months
        {
            get => months;
            set
            {
                months = value;
                OnPropertyChanged(nameof(Months));
            }
        }
        /// <summary>
        /// Выбранный пользователем месяц.
        /// </summary>
        public DateTime SelectedMonth
        {
            get => selectedMonth;
            set
            {
                selectedMonth = value;
                ChangeIsMonthSelected(value);

                SetUpViewModel();
                OnPropertyChanged(nameof(SelectedMonth));
            }
        }
        /// <summary>
        /// Список услуг.
        /// </summary>
        public List<Service> Services
		{
			get => services;
			set
			{
				services = value;
				OnPropertyChanged(nameof(Services));
			}
		}
        /// <summary>
        /// Выбранная пользователем услуга.
        /// </summary>
        public Service SelectedService
        {
            get => selectedService;
            set
            {
                selectedService = value;

                ChangeIsSelectedService(value);
                SetUpViewModel();
                OnPropertyChanged(nameof(SelectedService));
            }
        }
        /// <summary>
        /// Список операций.
        /// </summary>
        public List<Operation> Operations
		{
			get => operations;
			set
			{
				operations = value;

                IsThereOperations = operations.Count > 0;
				OnPropertyChanged(nameof(Operations));
			}
		}
        /// <summary>
        /// Общая сумма расходов.
        /// </summary>
        public double? SumOfExpenses
        {
            get => sumOfExpenses;
            set
            {
                sumOfExpenses = value;
                OnPropertyChanged(nameof(SumOfExpenses));
            }
        }
        /// <summary>
        /// Серии графика о расходах.
        /// </summary>
        public SeriesCollection OperationsSeries
        {
            get => operationSeries;
            set
            {
                operationSeries = value;
                OnPropertyChanged(nameof(OperationsSeries));
            }
        }
        /// <summary>
        /// Указывает на наличие операций.
        /// </summary>
        public bool IsThereOperations
        {
            get => isThereOperations;
            set
            {
                isThereOperations = value;
                OnPropertyChanged(nameof(IsThereOperations));
            }
        }
        /// <summary>
        /// Указывает, выбрана ли тип услуга.
        /// </summary>
        public bool IsServiceSelected
        {
            get => isServiceSelected;
            set
            {
                isServiceSelected = value;
                OnPropertyChanged(nameof(IsServiceSelected));
            }
        }
        /// <summary>
        /// Указывает, выбран ли месяц.
        /// </summary>
        public bool IsMonthSelected
        {
            get => isMonthSelected;
            set
            {
                isMonthSelected = value;
                OnPropertyChanged(nameof(IsMonthSelected));
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Комманда перехода на <see cref="AccountViewModel"/>.
        /// </summary>
        public ICommand NavigateAccountCommand { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Заполняет <see cref="Months"/> значениями от января до декабря.
        /// </summary>
        /// <returns><see cref="List{T}"/>, содержащий 12 месяцев с января до декабря.</returns>
        private List<DateTime> SetUpMonths()
        {
            List<DateTime> months = new List<DateTime>();
            for (int i = 1; i <= 12; i++)
                months.Add(new DateTime(DateTime.Now.Year, i, 1));
            return months;
        }
        /// <summary>
        /// Заполняет список услуг из базы данных.
        /// </summary>
        /// <returns><see cref="List{T}"/>, содержащий услуги <see cref="Service"/>.</returns>
        private List<Service> SetUpServices()
        {
            List<Service> servicesList = database.Service.Where(s => s.Name == "Перевод" || s.Name == "Вывод средств").ToList();
            return servicesList.Count != 0 ? servicesList : null;
        }
        /// <summary>
        /// Устанавливает <see cref="Operations"/> и настраивает отображение графика.
        /// </summary>
        private void SetUpViewModel()
        {
            List<Operation> operationsList = database.Operation.Where(
                    o => (o.Service.Name == "Вывод средств" || o.Service.Name == "Перевод")
                    && o.UserID == userStore.CurrentUser.ID).ToList();

            Operations = operationsList;

            if (SelectedMonth != DateTime.MinValue)
                Operations = Operations
                    .Where(o => o.Date.Month == SelectedMonth.Month)
                    .ToList();

            if (SelectedService != null)
                Operations = Operations
                    .Where(o => o.ServiceID == SelectedService.ID)
                    .ToList();

            SumOfExpenses = 0;
            UpdateChart();
        }
        /// <summary>
        /// Обновляет график.
        /// </summary>
        private void UpdateChart()
        {
            OperationsSeries = new SeriesCollection();

            if (SelectedService == null)
                foreach (Service service in Services)
                    SetSeries(service);
            else
                SetSeries(SelectedService);
        }
        /// <summary>
        /// Добавляет серию для услуги на график.
        /// </summary>
        /// <param name="service">Сущность <see cref="Service"/>, добавляемая на график в качестве серии.</param>
        private void SetSeries(Service service)
        {
            ObservableValue observableValue = new ObservableValue(0);
            var operationsList = Operations.Where(o => o.ServiceID == service.ID).ToList();
            if (operationsList.Count > 0)
                    observableValue.Value = operationsList.Sum(o => o.Sum);

            if (operationsList.Count > 0)
                SumOfExpenses += Math.Round(operationsList.Sum(o => o.Sum), 2);

            var series = new PieSeries()
            {
                Title = service.Name,
                Values = new ChartValues<ObservableValue>
                {
                    observableValue
                },
                DataLabels = true
            };
            OperationsSeries.Add(series);
        }
        /// <summary>
        /// Изменяет свойство <see cref="IsMonthSelected"/> 
        /// в зависимости от <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Значение <see cref="DateTime"/>, 
        /// определяющее свойство <see cref="IsMonthSelected"/>.</param>
        private void ChangeIsMonthSelected(DateTime value)
        {
            if (value != DateTime.MinValue)
                IsMonthSelected = true;
            else
                IsMonthSelected = false;
        }
        /// <summary>
        /// Изменяет свойство <see cref="IsServiceSelected"/> 
        /// в зависимости от <paramref name="selectedService"/>.
        /// </summary>
        /// <param name="selectedService">Значение <see cref="Service"/>,
        /// определяющее свойство <see cref="IsServiceSelected"/>.</param>
        private void ChangeIsSelectedService(Service selectedService)
        {
            if (selectedService != null)
                IsServiceSelected = true;
            else
                IsServiceSelected = false;
        }
        /// <summary>
        /// Освобождает ресурсы <see cref="ExpenseReportViewModel"/>.
        /// </summary>
        public override void Dispose()
        {
            database.Dispose();

            base.Dispose();
        }
        #endregion
    }
}
