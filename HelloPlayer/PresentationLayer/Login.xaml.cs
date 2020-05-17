using DataObject;
using Logic;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        #region ClassObjects
        private Employee _emp = null;
        private EmployeeManager _employeeManager;
        private PlayerManager _playerManager;
        #endregion ClassObjects

        #region Constructors
        public Login()
        {
            InitializeComponent();
            Uri uri = new Uri("F:\\Spring2020\\net 3\\HelloPlayer\\Logos\\logo_transparent.png", UriKind.Absolute);
            PngBitmapDecoder decoder = new PngBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];
            Logo.Source = bitmapSource;
            _employeeManager = new EmployeeManager();
            _playerManager = new PlayerManager();
            _emp = new Employee();
        }
        #endregion Constructors

        #region UIControls
        private void deleteEmail(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == "Email")
            {
                txtEmail.Text = "";
            }
        }

        private void deletePassword(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password == "newuser")
            {
                txtPassword.Password = "";
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var email = txtEmail.Text;
            var password = txtPassword.Password;
            bool authenticated = false;

            if (email.Length < 7 || password.Length < 7)
            {
                MessageBox.Show("Bad Username or Password.",
                "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                txtEmail.Text = "";
                txtPassword.Password = "";
                txtEmail.Focus();
                return;
            }
            try
            {
                _emp = _employeeManager.AuthenticateUser(email, password);
                //Force new user to change password
                if (txtPassword.Password == "newuser")
                {
                    var updatePassword = new UpdatePassword(_emp, _employeeManager);
                    if (updatePassword.ShowDialog() == false)
                    {
                        //Code to log the user back out and display an error message
                    }
                }
                authenticated = true;
                if (authenticated == true)
                {
                    HelloPlayerEmpHub empHub = new HelloPlayerEmpHub(_emp, _employeeManager, _playerManager);
                    empHub.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n",
                    "Login Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion UIControls
    }
}
