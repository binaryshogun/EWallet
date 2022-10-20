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
    public class ExpenseReportViewModel : ViewModelBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly INavigationService accountNavigationService;

        private readonly WalletEntities database;

        private List<DateTime> months;
		private DateTime selectedMonth;
		
		private List<Service> services;
		private Service selectedService;

		private List<Operation> operations;

		private double? sumOfExpenses;

		private SeriesCollection operationSeries;
        private bool dataFetching;
        private bool isThereOperations;
        #endregion

        #region Constructors
        public ExpenseReportViewModel(UserStore userStore, INavigationService accountNavigationService)
        {
            this.userStore = userStore;
            this.accountNavigationService = accountNavigationService;

            try
            {
                database = new WalletEntities();
            }
            catch (Exception e)
            {
                ErrorMessageBox.Show(e);
                accountNavigationService?.Navigate();
            }

            NavigateCommand = new NavigateCommand(accountNavigationService);

            Months = SetUpMonths();
            Services = SetUpServices();

            SetUpViewModel();
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

        public bool DataFetching
        {
            get => dataFetching;
            set
            {
                dataFetching = value;
                OnPropertyChanged(nameof(DataFetching));
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
        #endregion

        #region Commands
        public ICommand NavigateCommand { get; }
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
            try
            {
                using (var database = new WalletEntities())
                {
                    List<Service> servicesList = database.Service.Where(s => s.ID == 1 || s.ID == 2).ToList();
                    return servicesList;
                }
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
            return null;
        }
        private void SetUpViewModel()
        {
            DataFetching = true;
            try
            {
                List<Operation> operationsList = database.Operation.Where(
                        o => (o.ServiceID == 1 || o.ServiceID == 2)
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

                UpdateChart(database);
            }
            catch (Exception e) 
            { 
                ErrorMessageBox.Show(e);
                accountNavigationService?.Navigate();
            }
            finally
            {
                DataFetching = false;
            }
        }

        private void UpdateChart(WalletEntities database)
        {
            OperationsSeries = new SeriesCollection();

            if (SelectedService == null)
                foreach (Service service in Services)
                    SetSeries(service, database);
            else
                SetSeries(SelectedService, database);
        }
        private void SetSeries(Service service, WalletEntities database)
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

        public override void Dispose()
        {
            database.Dispose();

            base.Dispose();
        }
        #endregion
    }
}
