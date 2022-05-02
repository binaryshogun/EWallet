using _119_Karpovich.Commands;
using _119_Karpovich.Models;
using _119_Karpovich.Services;
using _119_Karpovich.Stores;
using System.Windows.Input;
using System.Linq;
using System.Windows.Threading;
using System;

namespace _119_Karpovich.ViewModels
{
    public class UserProfileViewModel : ViewModelBase
    {
        #region Fields
        private readonly NavigationStore navigationStore;
        private readonly Passport passport;

        private string firstName;
        private string lastName;
        private string patronymic;
        private int serialNumber;
        private int number;
        private int divisionCode;

        private readonly DispatcherTimer updateTimer;
        private string timeNow;
        #endregion

        #region Constructors
        public UserProfileViewModel(User user, NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;

            using (var dataBase = new WalletEntities())
            {
                passport = dataBase.Passport.AsNoTracking().
                    Where(p => p.UserID == user.ID).FirstOrDefault();
            }

            NavigateCommand = new NavigateCommand<AccountViewModel>(new NavigationService<AccountViewModel>(
                navigationStore, () => new AccountViewModel(user, navigationStore)));

            timeNow = DateTime.Now.ToString("g");

            updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            updateTimer.Tick += new EventHandler(UpdateTime);
            updateTimer.Start();
        }
        #endregion

        #region Properties
        public string FirstName
        {
            get => firstName;
            set 
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => lastName;
            set 
            { 
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string Patronymic
        {
            get => patronymic;
            set 
            { 
                patronymic = value;
                OnPropertyChanged(nameof(Patronymic));
            }
        }

        public int SerialNumber
        {
            get => serialNumber;
            set
            {
                serialNumber = value;
                OnPropertyChanged(nameof(SerialNumber));
            }
        }

        public int Number
        {
            get => number;
            set 
            { 
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        public int DivisionCode
        {
            get => divisionCode;
            set 
            { 
                divisionCode = value;
                OnPropertyChanged(nameof(DivisionCode));
            }
        }

        /// <summary>
        /// Текущее локальное время.
        /// </summary>
        /// <value>
        /// Строка, содержащее текущее время.
        /// </value>
        public string TimeNow 
        {
            get => timeNow;
            set
            {
                timeNow = value;
                OnPropertyChanged(nameof(TimeNow));
            }
        }
        #endregion

        #region Commands
        public ICommand NavigateCommand { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Обработчик события обновления времени в таймере.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Данные события.</param>
        private void UpdateTime(object sender, EventArgs e)
            => TimeNow = DateTime.Now.ToString("g");
        #endregion
    }
}
