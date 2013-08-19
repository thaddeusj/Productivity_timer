using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Productivity_Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public Timer t;
        private int seconds;
        public int Seconds
        {
            set
            {
                seconds = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Seconds"));
            }
            get { return seconds; }
        }

        private int minutes;
        public int Minutes
        {
            set
            {
                minutes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Minutes"));
            }
            get { return minutes; }
        }




        public int MinutesToGo { set; get; }
        public int SecondsToGo { set; get; }




        public int tickcount;


        public MainWindow()
        {
            InitializeComponent();

            timerGrid.DataContext = this;
            MinutesSetBox.DataContext = this;
            SecondsSetBox.DataContext = this;
            CustomRadio.IsChecked = true;

            #if DEBUG
            t = new Timer(10);
            #else
            t = new Timer(1000);
            #endif


            t.AutoReset = true;

            Seconds = 0;
            Minutes = 0;
            tickcount = 0;
            

            t.Elapsed += new ElapsedEventHandler(UpdateTimes);

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (((MinutesToGo > 0 || SecondsToGo > 0) && (bool)CustomRadio.IsChecked) || (bool)ThirtyMinsPreset.IsChecked || (bool)OneHourPreset.IsChecked || (bool)TwoHoursPreset.IsChecked)
            {
                if ((bool)ThirtyMinsPreset.IsChecked)
                {
                    MinutesToGo = 30;
                    SecondsToGo = 0;
                }
                if ((bool)OneHourPreset.IsChecked)
                {
                    MinutesToGo = 60;
                    SecondsToGo = 0;
                }
                if ((bool)TwoHoursPreset.IsChecked)
                {
                    MinutesToGo = 120;
                    SecondsToGo = 0;
                }


                t.Start();


                tickcount = 0;
                Seconds = 0;
                Minutes = 0;

                MinutesSetBox.Focusable = false;
                SecondsSetBox.Focusable = false;

                ThirtyMinsPreset.Focusable = false;
                OneHourPreset.Focusable = false;
                TwoHoursPreset.Focusable = false;
                CustomRadio.Focusable = false;
            }
        }

        private void UpdateTimes(object sender, ElapsedEventArgs e)
        {
            Seconds = Seconds + 1;
            if (Seconds == 60)
            {
                Seconds = 0;
                Minutes = Minutes + 1;
            }
            tickcount++;

            if (Seconds == SecondsToGo && Minutes == MinutesToGo)
            {
                t.Stop();
                

                

                if (this.Dispatcher.CheckAccess())
                {
                    this.WindowState = WindowState.Normal;
                    MinutesSetBox.Focusable = true;
                    SecondsSetBox.Focusable = true;
                }
                else
                {
                    this.Dispatcher.Invoke((Action)(() => this.WindowState = WindowState.Normal));
                    this.Dispatcher.Invoke((Action)(() => MinutesSetBox.Focusable = true));
                    this.Dispatcher.Invoke((Action)(() => SecondsSetBox.Focusable = true));
                    this.Dispatcher.Invoke((Action)(() => ThirtyMinsPreset.Focusable = true));
                    this.Dispatcher.Invoke((Action)(() => TwoHoursPreset.Focusable = true));
                    this.Dispatcher.Invoke((Action)(() => OneHourPreset.Focusable = true));
                    this.Dispatcher.Invoke((Action)(() => CustomRadio.Focusable = true));
                }

            }
            else
            {
                if (tickcount == 5)
                {
                    if (this.Dispatcher.CheckAccess())
                    {
                        this.WindowState = WindowState.Minimized;
                    }
                    else
                    {
                        this.Dispatcher.Invoke((Action)(() => this.WindowState = WindowState.Minimized));
                    }

                }
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);

        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            t.Stop();
            Seconds = 0;
            Minutes = 0;
        }

        private void Window1_Activated(object sender, EventArgs e)
        {
            if (t.Enabled) tickcount = -5;
        }

        private void SecondsSetBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            int parseResult;
            if (((TextBox)sender).Text.Length > 0 && int.TryParse(((TextBox)sender).Text, out parseResult))
            { 
                
                if(parseResult >= 60)
                {
                    ((TextBox)sender).Text = 59.ToString();
                }
            }
        }

        
    }
}
