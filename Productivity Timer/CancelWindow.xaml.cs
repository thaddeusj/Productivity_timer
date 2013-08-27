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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CancelWindow : Window
    {

        public string CancelReason { set; get; }


        public CancelWindow()
        {
            InitializeComponent();

            cancelText.Text = "";
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (cancelText.Text != "" && cancelText.Text.Length < 4000)
            {
                CancelReason = cancelText.Text;
                this.Close();
            }
           
        }
    }
}
