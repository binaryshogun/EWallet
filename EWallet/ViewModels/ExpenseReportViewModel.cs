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
        public List<DateTime> Months
        {
            get => months;
            set
            {
                months = value;
                OnPropertyChanged(nameof(Months));
            }
        }
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

        public List<Service> Services
		{
			get => services;
			set
			{
				services = value;
				OnPropertyChanged(nameof(Services));
			}
		}
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

        public double? SumOfExpenses
        {
            get => sumOfExpenses;
            set
            {
                sumOfExpenses = value;
                OnPropertyChanged(nameof(SumOfExpenses));
            }
        }

        public SeriesCollection OperationsSeries
        {
            get => operationSeries;
            set
            {
                operationSeries = value;
                OnPropertyChanged(nameof(OperationsSeries));
            }
        }
        public bool IsThereOperations
        {
            get => isThereOperations;
            set
            {
                isThereOperations = value;
                OnPropertyChanged(nameof(IsThereOperations));
            }
        }

        public bool IsServiceselected
        {
            get => isServiceSelected;
            set
            {
                isServiceSelected = value;
                OnPropertyChanged(nameof(IsServiceselected));
            }
        }
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
        public ICommand NavigateAccountCommand { get; }
        #endregion

        #region Methods
        private List<DateTime> SetUpMonths()
        {
            List<DateTime> months = new List<DateTime>();
            for (int i = 1; i <= 12; i++)
                months.Add(new DateTime(DateTime.Now.Year, i, 1));
            return months;
        }
        private List<Service> SetUpServices()
        {
            List<Service> servicesList = database.Service.Where(s => s.Name == "Перевод" || s.Name == "Вывод средств").ToList();
            return servicesList.Count != 0 ? servicesList : null;
        }
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
            if (Operations.Count > 0)
                SumOfExpenses = Math.Round(operationsList.Sum(o => o.Sum), 2);

            UpdateChart();
        }

        private void UpdateChart()
        {
            OperationsSeries = new SeriesCollection();

            if (SelectedService == null)
                foreach (Service service in Services)
                    SetSeries(service);
            else
                SetSeries(SelectedService);
        }
        private void SetSeries(Service service)
        {
            ObservableValue observableValue = new ObservableValue(0);
            var operationsList = Operations.Where(o => o.ServiceID == service.ID).ToList();
            if (operationsList.Count > 0)
                    observableValue.Value = operationsList.Sum(o => o.Sum);

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

        private void ChangeIsMonthSelected(DateTime value)
        {
            if (value != DateTime.MinValue)
                IsMonthSelected = true;
            else
                IsMonthSelected = false;
        }
        private void ChangeIsSelectedService(Service selectedService)
        {
            if (selectedService != null)
                IsServiceselected = true;
            else
                IsServiceselected = false;
        }

        public override void Dispose()
        {
            database.Dispose();

            base.Dispose();
        }
        #endregion
    }
}
