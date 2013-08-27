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

using System.Data.Sql;
using System.Data.SqlServerCe;
using System.Data;

namespace Productivity_Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public TimeUpWindow donedlg;

        public string task;

        public Database1Timerstuff dataset;
        public DataTable displaySet;
        public int MinutesToGo { set; get; }
        public int HoursToGo { set; get; }
        public int SecondsToGo { set; get; }

        public int tickcount;


        public DateTime currentTime;

        #region Databinding_Properties_and_Handlers
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);

        }
        private bool signedIn;
        public bool SignedIn
        {
            set
            {
                signedIn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SignedIn"));
            }
            get { return signedIn; }
        }

        private bool notSignedIn;
        public bool NotSignedIn
        {
            set
            {
                notSignedIn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NotSignedIn"));
            }
            get { return notSignedIn; }
        }


        private string user;
        public string User
        {
            set
            {
                user = value;
                OnPropertyChanged(new PropertyChangedEventArgs("User"));
            }
            get { return user; }
        }


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

        private int hours;
        public int Hours
        {
            set
            {
                hours = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Minutes"));
            }
            get { return hours; }
        }

        #endregion

        public MainWindow()
        {
            
            
            InitializeComponent();

            usernameLabel.DataContext = this;
            fileMenu.DataContext = this;

            timerGrid.DataContext = this;
            MinutesSetBox.DataContext = this;
            SecondsSetBox.DataContext = this;
            CustomRadio.IsChecked = true;

            SignedIn = false;
            NotSignedIn = true;
            User = null;

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

        #region Timer_Update

        private void UpdateTimes(object sender, ElapsedEventArgs e)
        {
            Seconds = Seconds + 1;
            if (Seconds == 60)
            {
                Seconds = 0;
                Minutes = Minutes + 1;
            }
            if (Minutes == 60)
            {
                Minutes = 0;
                Hours = Hours + 1;
            }
            if (tickcount < 5) tickcount++;

            if (Seconds == SecondsToGo && Minutes == MinutesToGo && Hours == HoursToGo)
            {
                t.Stop();

                if (this.Dispatcher.CheckAccess())
                {
                    this.WindowState = WindowState.Normal;
                    MinutesSetBox.Focusable = true;
                    SecondsSetBox.Focusable = true;
                    ThirtyMinsPreset.Focusable = true;
                    TwoHoursPreset.Focusable = true;
                    OneHourPreset.Focusable = true;
                    CustomRadio.Focusable = true;
                    StopButton.Focusable = false;
                    taskBox.Focusable = true;
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
                    this.Dispatcher.Invoke((Action)(() => StopButton.Focusable = false));
                    this.Dispatcher.Invoke((Action)(() => taskBox.Focusable = true));
                }

                if (signedIn == true)
                {

                    if (this.Dispatcher.CheckAccess())
                    {

                        donedlg.ShowDialog();
                        
                    }
                    else
                    {
                        this.Dispatcher.Invoke((Action)(() => donedlg.ShowDialog()));
                        
                    }



                    Database1TimerstuffTableAdapters.TimerInfoTableAdapter ad = new Database1TimerstuffTableAdapters.TimerInfoTableAdapter();
                    ad.Connection = new System.Data.SqlServerCe.SqlCeConnection(Properties.Settings.Default.Database1ConnectionString1);
                    Database1TimerstuffTableAdapters.TimerInfo1TableAdapter ad1 = new Database1TimerstuffTableAdapters.TimerInfo1TableAdapter();
                    ad1.Connection = new SqlCeConnection(Properties.Settings.Default.Database1ConnectionString1);

                    
                    TimeSpan tspan = new TimeSpan(Hours, Minutes, Seconds);

                    ad.Fill(dataset.TimerInfo);
                    try
                    {
                        ad.Insert(User, currentTime, donedlg.Completed, null, donedlg.failReason, tspan.ToString(), tspan.ToString(), task);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    ad.Update(dataset);

                    ad.FillByUser(dataset.TimerInfo, User);

                    ad1.Fill(dataset.TimerInfo1,User);


                    if (this.Dispatcher.CheckAccess())
                    {
                        taskBox.Text = null;
                    }
                    else { this.Dispatcher.Invoke((Action)(() => taskBox.Text = null)); }
                    task = null;
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

        

        #endregion

        #region timer_buttons

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (taskBox.Text.Length > 0 && taskBox.Text.Length < 4000)
            {
                if (((MinutesToGo > 0 || HoursToGo > 0) && (bool)CustomRadio.IsChecked) || (bool)ThirtyMinsPreset.IsChecked || (bool)OneHourPreset.IsChecked || (bool)TwoHoursPreset.IsChecked)
                {
                    donedlg = new TimeUpWindow();

                    if ((bool)ThirtyMinsPreset.IsChecked)
                    {
                        MinutesToGo = 30;
                        HoursToGo = 0;

                    }
                    if ((bool)OneHourPreset.IsChecked)
                    {
                        MinutesToGo = 0;
                        HoursToGo = 1;
                    }
                    if ((bool)TwoHoursPreset.IsChecked)
                    {
                        MinutesToGo = 0;
                        HoursToGo = 2;
                    }

                    currentTime = DateTime.Now;

                    task = taskBox.Text;

                    t.Start();


                    tickcount = 0;
                    Seconds = 0;
                    Minutes = 0;

                    taskBox.Focusable = false;
                    MinutesSetBox.Focusable = false;
                    SecondsSetBox.Focusable = false;

                    ThirtyMinsPreset.Focusable = false;
                    OneHourPreset.Focusable = false;
                    TwoHoursPreset.Focusable = false;
                    CustomRadio.Focusable = false;

                    StopButton.IsEnabled = true;
                }
            }
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            t.Stop();


            StopButton.IsEnabled = false;


            CancelWindow dlg = new CancelWindow();
            dlg.ShowDialog();

            if (SignedIn == true)
            {
                Database1TimerstuffTableAdapters.TimerInfoTableAdapter ad = new Database1TimerstuffTableAdapters.TimerInfoTableAdapter();
                Database1TimerstuffTableAdapters.TimerInfo1TableAdapter ad1 = new Database1TimerstuffTableAdapters.TimerInfo1TableAdapter();
                ad.Connection = new SqlCeConnection(Properties.Settings.Default.Database1ConnectionString1);
                ad1.Connection = new SqlCeConnection(Properties.Settings.Default.Database1ConnectionString1);

                //"Data Source=\"C:\\Users\\Thad\\Documents\\Programming Practice\\Productivity Timer\\Productivity Timer\\Database1.sdf\""

                TimeSpan tspan = new TimeSpan(HoursToGo, MinutesToGo, SecondsToGo);
                TimeSpan elapsedtspan = new TimeSpan(Hours, Minutes, Seconds);

                ad.Fill(dataset.TimerInfo);
                ad.Insert(User, currentTime, false, dlg.CancelReason, null, tspan.ToString(), elapsedtspan.ToString(), taskBox.Text);
                ad.Update(dataset);
                ad.FillByUser(dataset.TimerInfo, User);

                ad1.Fill(dataset.TimerInfo1, User);


            }



            Seconds = 0;
            Minutes = 0;
        }
        #endregion

        #region fileMenu_Click_Handlers
        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            if (t.Enabled == false)
            {
                SignInWindow dlg = new SignInWindow();
                if (dlg.ShowDialog() == true)
                {
                    SignedIn = true;
                    NotSignedIn = false;

                    User = dlg.Username;


                    dataset = new Database1Timerstuff();
                    Database1TimerstuffTableAdapters.TimerInfo1TableAdapter ad1 = new Database1TimerstuffTableAdapters.TimerInfo1TableAdapter();
                    ad1.Connection = new SqlCeConnection(Properties.Settings.Default.Database1ConnectionString1);
                    Database1TimerstuffTableAdapters.TimerInfoTableAdapter ad = new Database1TimerstuffTableAdapters.TimerInfoTableAdapter();
                    ad.Connection = new SqlCeConnection(Properties.Settings.Default.Database1ConnectionString1);

                    ad.FillByUser(dataset.TimerInfo, User);
                    ad1.Fill(dataset.TimerInfo1,User);


                    DataTab.DataContext = dataset.TimerInfo1;

                }

            }
        }
        
        private void SignOut_Click(object sender, RoutedEventArgs e)
        {

            if (t.Enabled == false)
            {
                user = "";
                user = null;
                SignedIn = false;
                NotSignedIn = true;

                timerDataGrid.DataContext = null;
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            if (t.Enabled == true) t.Stop();


            this.Close();
        }
        #endregion

        #region UI_Validation
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((TabControl)sender).SelectedItem == DataTab && signedIn == false) tabCtrl.SelectedItem = TimerTab;
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
        
        private void Window1_Activated(object sender, EventArgs e)
        {
            if (t.Enabled) tickcount = 6;
        }

        #endregion
    }
}
