using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomControls.Controls
{
    /// <summary>
    /// Interaction logic for ucDateTimeUpDown.xaml
    /// </summary>
    public partial class UcDateTimeUpDown : UserControl, INotifyPropertyChanged
    {
        #region Private Member variable

        private DateTime _currentTime = DateTime.UtcNow;
        //private bool _adHours;
        //private bool _addMinutes;
        private ObservableCollection<string> _amPmTypes
    = new ObservableCollection<string>();
        private string _displayAmPm;

        #endregion

        #region Constructors

        public UcDateTimeUpDown()
        {
            InitializeComponent();
            this.DataContext = this;
            AmPmTypes.Add("AM");
            AmPmTypes.Add("PM");
            CurrentTime = DateTime.UtcNow.ToLocalTime();
            SelectedTime = CurrentTime.ToLocalTime().ToString("t");
        }

        #endregion

        #region Public Properties

        public ObservableCollection<string> AmPmTypes
        {
            get { return _amPmTypes; }
            set { _amPmTypes = value; }
        }

        public string DisplayTime
        {
            get { return _currentTime.ToLocalTime().ToString("t"); }
        }


        public string DisplayAmPm
        {
            get
            {
                if (_currentTime.Hour >= 0 && _currentTime.Hour < 12)
                    _displayAmPm = AmPmTypes.FirstOrDefault(s => s.Equals("AM"));
                else
                {
                    if (_currentTime.Hour >= 12)
                    {
                        _displayAmPm = AmPmTypes.FirstOrDefault(s => s.Equals("PM"));
                    }
                }

                return _displayAmPm;
            }
            set
            {
                if (!value.Equals(_displayAmPm))
                {
                    if (value.Equals("PM"))
                        CurrentTime = CurrentTime.AddHours(12);
                    else
                    {
                        CurrentTime = CurrentTime.AddHours(-12);
                    }
                }
                _displayAmPm = value;
            }
        }

        public string DisplayTimeHours
        {
            get
            {
                var hours = _currentTime.Hour;
                return hours > 12 ? (hours - 12).ToString("00") : hours.ToString("00");
                //return hours.ToString();
            }
            set
            {
                var hour = 0;
                Int32.TryParse(value, out hour);
                CurrentTime = CurrentTime.ToLocalTime().AddHours(hour);
                OnPropertyChanged("DisplayTime");
                OnPropertyChanged("DisplayTimeHours");
                OnPropertyChanged("DisplayTimeMinutes");
            }
        }

        public string DisplayTimeMinutes
        {
            get { return _currentTime.Minute.ToString("00"); }
            set
            {
                var minutes = 0;
                Int32.TryParse(value, out minutes);
                CurrentTime = CurrentTime.ToLocalTime().AddMinutes(minutes);
                OnPropertyChanged("DisplayTime");
                OnPropertyChanged("DisplayTimeHours");
                OnPropertyChanged("DisplayTimeMinutes");
            }
        }

        public DateTime CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;

                OnPropertyChanged("CurrentTime");
                OnPropertyChanged("DisplayTime");
                OnPropertyChanged("DisplayTimeHours");
                OnPropertyChanged("DisplayTimeMinutes");
                OnPropertyChanged("DisplayAmPm");
                SelectedTime = value.ToLocalTime().ToString("t");
            }
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty SelectedTimeProperty
    = DependencyProperty.Register(
          "SelectedTime", typeof(string), typeof(UcDateTimeUpDown),
    new PropertyMetadata(default(string)));

        public string SelectedTime
        {
            get
            {
                return ((DateTime)GetValue(SelectedTimeProperty)).ToLocalTime().ToString("t");
            }
            set { SetValue(SelectedTimeProperty, value); }
        }

        #endregion

        #region Methods

        private void MinutesUpButton_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentTime = CurrentTime.AddMinutes(1);
            SelectedTime = CurrentTime.ToLocalTime().ToString("t");
        }

        private void MinutesDownButton_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentTime = CurrentTime.AddMinutes(-1);
            SelectedTime = CurrentTime.ToLocalTime().ToString("t");
        }

        private void HourUpButton_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentTime = CurrentTime.AddHours(1);
            SelectedTime = CurrentTime.ToLocalTime().ToString("t");
        }

        private void HourDownButton_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentTime = CurrentTime.AddHours(-1);
            SelectedTime = CurrentTime.ToLocalTime().ToString("t");
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void AddHoursTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AddHoursTextBox.Text))
            {

                int userhr = Convert.ToInt32(AddHoursTextBox.Text);
                string struserhr = userhr > 23 ? (23).ToString("00") : userhr.ToString("00");
                CurrentTime = new DateTime(CurrentTime.Year, CurrentTime.Month, CurrentTime.Day, Convert.ToInt32(struserhr), CurrentTime.Minute, CurrentTime.Second);
            }

        }

        private void AddMinutesTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AddMinutesTextBox.Text))
            {
                int usermin = Convert.ToInt32(AddMinutesTextBox.Text);
                string struserhr = usermin > 59 ? (59).ToString("00") : usermin.ToString("00");
                CurrentTime = new DateTime(CurrentTime.Year, CurrentTime.Month, CurrentTime.Day, CurrentTime.Hour, usermin, CurrentTime.Second);
            }
        }

        private void AddHoursTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AddHoursTextBox.Text))
            {
                AddHoursTextBox.SelectAll();
            }
        }

        private void AddMinutesTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AddMinutesTextBox.Text))
            {
                AddMinutesTextBox.SelectAll();
            }
        }
    }
}
