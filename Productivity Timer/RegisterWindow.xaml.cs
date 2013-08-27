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
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Security.Cryptography;

namespace Productivity_Timer
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();

            UsernameBox.Focus();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string name = UsernameBox.Text;
            string pw = PasswordBox.Text;

            HashAlgorithm alg = MD5.Create();

            byte[] pwHash = alg.ComputeHash(Encoding.UTF8.GetBytes(pw));


            Database1TimerstuffTableAdapters.UserInfoTableAdapter ad = new Database1TimerstuffTableAdapters.UserInfoTableAdapter();
            ad.Connection = new SqlCeConnection(Properties.Settings.Default.Database1ConnectionString1);

            //"Data Source=\"C:\\Users\\Thad\\Documents\\Programming Practice\\Productivity Timer\\Productivity Timer\\Database1.sdf\""

            Database1Timerstuff timerstuff = new Database1Timerstuff();
            ad.Fill(timerstuff.UserInfo);

            try
            {
                ad.NewUserRow(name, pwHash);

                ad.Update(timerstuff);

                this.Close();
            }
            catch
            {
                MessageBox.Show("This username is already taken.");
            }




        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
