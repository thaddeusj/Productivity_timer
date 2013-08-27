using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Productivity_Timer
{
    /// <summary>
    /// Interaction logic for TimeUpWindow.xaml
    /// </summary>
    public partial class TimeUpWindow : Window
    {
        public string failReason;
        public bool Completed;


        public TimeUpWindow()
        {
            InitializeComponent();

            OKButton.IsEnabled = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (YesButton.IsChecked == true)
            {
                Completed = true;
                failReason = null;

                this.Close();
            }
            else
            {
                Completed = false;
                failReason = reasonBox.Text;

                this.Close();
            }

        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            OKButton.IsEnabled = true;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            OKButton.IsEnabled = true;
        }
    }
}
