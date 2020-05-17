using DataObject;
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

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for AddEditEmployee.xaml
    /// </summary>
    public partial class AddEditEmployee : Window
    {
        #region ClassObjects
        private Employee _employee = null;
        private IEmployeeManager _employeeManager = null;
        private bool _addMode = false;
        #endregion ClassObjects

        #region Constructors
        public AddEditEmployee(IEmployeeManager employeeManager)
        {
            InitializeComponent();
            _employeeManager = employeeManager;
            _addMode = true;
        }

        public AddEditEmployee(Employee emp, IEmployeeManager employeeManager)
        {
            InitializeComponent();
            _employee = emp;
            _employeeManager = employeeManager;
        }

        #endregion Constructors

        #region UIControls
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtFirstName.Text == "")
            {
                MessageBox.Show("You must enter a first name.");
                txtFirstName.Focus();
                return;
            }
            if (txtLastName.Text == "")
            {
                MessageBox.Show("You must enter a last name.");
                txtLastName.Focus();
                return;
            }
            if (txtPhoneNumber.Text.ToString().Length < 10)
            {
                MessageBox.Show("You must enter a valid phone number.");
                txtPhoneNumber.Focus();
                return;
            }
            if (!(txtEmailAddress.Text.ToString().Length > 6
                  && txtEmailAddress.Text.ToString().Contains("@")
                  && txtEmailAddress.Text.ToString().Contains(".")))
            {
                MessageBox.Show("You must enter a valid email address.");
                txtEmailAddress.Focus();
                return;
            }

            Employee newHire = new Employee()
            {
                FirstName = txtFirstName.Text.ToString(),
                LastName = txtLastName.Text.ToString(),
                UserName = txtUserName.Text.ToString(),
                PhoneNumber = txtPhoneNumber.Text.ToString(),
                Email = txtEmailAddress.Text.ToString(),
                Active = (bool)chkActive.IsChecked
            };

            if (_addMode)
            {
                try
                {
                    if (_employeeManager.InsertEmployee(newHire))
                    {
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n"
                        + ex.InnerException.Message);
                }
            }
            else
            {
                try
                {
                    if (_employeeManager.UpdateEmployee(_employee, newHire))
                    {
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        throw new ApplicationException("Data not Saved.",
                            new ApplicationException("User may no longer exist.")); ;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n"
                        + ex.InnerException.Message);
                }
            }

        }
        #endregion UIControls

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_addMode == false)
            {
                txtEmployeeID.Text = _employee.EmployeeID.ToString();
                txtFirstName.Text = _employee.FirstName;
                txtLastName.Text = _employee.LastName;
                txtUserName.Text = _employee.UserName;
                txtEmailAddress.Text = _employee.Email;
                txtPhoneNumber.Text = _employee.PhoneNumber;
                chkActive.IsChecked = _employee.Active;
                populateRoles();
            }
            else
            {
                SetEditMode();
                chkActive.IsChecked = true;
                chkActive.IsEnabled = false;
                lstUnassignedRoles.IsEnabled = false;
                lstAssignedRoles.IsEnabled = false;
            }
        }

        private void populateRoles()
        {
            try
            {
                var eRoles = _employeeManager.GetEmployeeRoles(_employee.EmployeeID);
                lstAssignedRoles.ItemsSource = eRoles;

                var roles = _employeeManager.GetEmployeeRoles();
                foreach (string r in eRoles)
                {
                    roles.Remove(r);
                }
                lstUnassignedRoles.ItemsSource = roles;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void SetEditMode()
        {
            txtFirstName.IsReadOnly = false;
            txtLastName.IsReadOnly = false;
            txtUserName.IsReadOnly = false;
            txtEmailAddress.IsReadOnly = false;
            txtPhoneNumber.IsReadOnly = false;
            chkActive.IsEnabled = true;

            lstAssignedRoles.IsEnabled = true;
            lstUnassignedRoles.IsEnabled = true;

            btnEdit.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Visible;
            txtFirstName.Focus();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            SetEditMode();
        }

        private void ChkActive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string caption = (bool)chkActive.IsChecked ? "Reactivate Employee" :
                    "Deactivate Employee";
                if (MessageBox.Show("Are you sure?", caption,
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.No)
                {
                    chkActive.IsChecked = !(bool)chkActive.IsChecked;
                    return;
                }

                _employeeManager.ChangeEmployeeActiveStatus((bool)chkActive.IsChecked, _employee.EmployeeID);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n"
                    + ex.InnerException.Message);
            }
        }

        private void LstUnassignedRoles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_addMode || lstUnassignedRoles.SelectedItems.Count == 0)
            {
                return;
            }
            if (MessageBox.Show("Are you sure?", "Change Role Assignment",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                if (_employeeManager.AddUserRole(_employee.EmployeeID,
                    (string)lstUnassignedRoles.SelectedItem))
                {
                    populateRoles();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void LstAssignedRoles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_addMode || lstAssignedRoles.SelectedItems.Count == 0)
            {
                return;
            }
            if (MessageBox.Show("Are you sure?", "Change Role Assignment",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.No)
            {
                return;
            }
            try
            {
                if (_employeeManager.DeleteUserRole(_employee.EmployeeID,
                    (string)lstAssignedRoles.SelectedItem))
                {
                    populateRoles();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }
    }
}
