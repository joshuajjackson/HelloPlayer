using DataObject;
using Logic;
using LogicInterfaces;
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
    /// Interaction logic for UpdatePassword.xaml
    /// </summary>
    public partial class UpdatePassword : Window
    {
        #region ClassObjects
        private Employee _emp = null;
        private IEmployeeManager _employeeManager = null;
        #endregion ClassObjects

        #region PasswordManagement
        public UpdatePassword(Employee emp, IEmployeeManager employeeManager)
        {
            InitializeComponent();
            _emp = emp;
            _employeeManager = employeeManager;
        }
        #endregion PasswordManagement

        #region UIControls
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (pwdCurrentPassword.Password.Length < 7)
            {
                MessageBox.Show("Current password is incorrect. Try again.");
                pwdCurrentPassword.Password = "";
                pwdCurrentPassword.Focus();
                return;
            }
            if (pwdNewPassword.Password.Length < 7 ||
                pwdNewPassword.Password == pwdCurrentPassword.Password)
            {
                MessageBox.Show("New password is incorrect. Try again.");
                pwdNewPassword.Password = "";
                pwdNewPassword.Focus();
                return;
            }
            if (pwdRetypePassword.Password != pwdNewPassword.Password)
            {
                MessageBox.Show("New Password and Retype must match.");
                pwdNewPassword.Password = "";
                pwdRetypePassword.Password = "";
                pwdNewPassword.Focus();
                return;
            }
            try
            {
                if (_employeeManager.UpdatePassword(_emp.EmployeeID,
                    pwdNewPassword.Password.ToString(),
                    pwdCurrentPassword.Password.ToString()))
                {
                    MessageBox.Show("Password Updated");
                    this.DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                this.DialogResult = false;
            }
        }
        #endregion UIControls
    }
}
