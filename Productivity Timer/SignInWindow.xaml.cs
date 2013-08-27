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
using System.Security.Cryptography;

namespace Productivity_Timer
{
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        public string Username { set; get; }

        public SignInWindow()
        {
            InitializeComponent();

            UserNameBox.Focus();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow dlg = new RegisterWindow();
            dlg.ShowDialog();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {

            Database1Timerstuff ts = new Database1Timerstuff();

            Database1TimerstuffTableAdapters.UserInfoTableAdapter ad = new Database1TimerstuffTableAdapters.UserInfoTableAdapter();
            ad.Connection = new System.Data.SqlServerCe.SqlCeConnection("Data Source=\"C:\\Users\\Thad\\Documents\\Programming Practice\\Productivity Timer\\Productivity Timer\\Database1.sdf\"");

            ad.Fill(ts.UserInfo);

            HashAlgorithm alg = MD5.Create();
            alg.ComputeHash(Encoding.UTF8.GetBytes(PasswordBox.Text));

            byte[] pw = ad.GetPassword(UserNameBox.Text);

            byte[] pwHash = alg.Hash;

            

            if (pw.SequenceEqual(pwHash))
            {

                Username = UserNameBox.Text;

                this.DialogResult = true;
                this.Close();
            }





        }
    }
}
