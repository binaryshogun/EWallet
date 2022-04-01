using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace _119_Karpovich.ViewModel
{
    class DisplayViewModel : ViewModelBase
    {
        public DisplayViewModel()
        {
            timeNow = DateTime.Now.ToString("G");

            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromSeconds(1);
            updateTimer.Tick += new EventHandler(UpdateTime);
            updateTimer.Start();
        }

        private void UpdateTime(object sender, EventArgs e)
        {
            TimeNow = DateTime.Now.ToString("G");
        }

        public string TimeNow
        {
            get { return timeNow; }
            set
            {
                timeNow = value;
                OnPropertyChanged(nameof(TimeNow));
            }
        }

        DispatcherTimer updateTimer;
        private string timeNow;
    }
}
