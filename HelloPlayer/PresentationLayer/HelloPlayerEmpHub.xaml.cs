﻿using DataObject;
using Logic;
using LogicInterfaces;
using PresentationLayer;
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
    /// Interaction logic for HelloPlayerEmpHub.xaml
    /// </summary>
    public partial class HelloPlayerEmpHub : Window
    {
        #region ClassObjects
        private Employee _emp;
        private IEmployeeManager _employeeManager;
        private IPlayerManager _playerManager;
        private IQuestManager _questManager = new QuestManager();
        private IPatchNoteManager _patchNoteManager = new PatchNoteManager();
        #endregion ClassObjects

        #region Constructors
        public HelloPlayerEmpHub(Employee emp, EmployeeManager employeeManager, PlayerManager playerManager)
        {
            InitializeComponent();
            _emp = emp;
            _employeeManager = employeeManager;
            _playerManager = playerManager;
            lblStatusMessage.Content = "Hello, " + emp.FirstName + " " + emp.LastName;
        }
        #endregion Constructors

        #region UIControls
        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            _emp = null;
            lblStatusMessage.Content = "You are not logged in!";
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void tabEmployees_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgEmployees.ItemsSource == null)
                {
                    dgEmployees.ItemsSource = _employeeManager.GetCurrentEmployees();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void BtnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var addEmployee = new AddEditEmployee(_employeeManager);
            if(addEmployee.ShowDialog() == true)
            {
                refreshEmployeeList();
            }
        }

        private void refreshEmployeeList()
        {
            dgEmployees.ItemsSource = _employeeManager.GetCurrentEmployees();
        }

        private void DgEmployees_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Employee selectedEmployee = (Employee)dgEmployees.SelectedItem;

            var empWindow = new AddEditEmployee(selectedEmployee, _employeeManager);
            if (empWindow.ShowDialog() == true)
            {
                refreshEmployeeList();
            }
        }

        private void ChkActive_Click(object sender, RoutedEventArgs e)
        {
            refreshEmployeeList();
        }

        private void tabPlayers_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgPlayers.ItemsSource == null)
                {
                    dgPlayers.ItemsSource = _playerManager.GetCurrentPlayers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }


        private void tabQuests_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgQuests.ItemsSource == null)
                {
                    bool active = true;
                    dgQuests.ItemsSource = _questManager.RetrieveActiveQuests(active);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void dgQuests_AutoGeneratedColumns(object sender, EventArgs e)
        {
            dgQuests.Columns.Remove(dgQuests.Columns[4]);
        }

        private void dgQuests_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Quest selectedQuest = (Quest)dgQuests.SelectedItem;

            var questDetails = new QuestDetails(selectedQuest, _questManager);
            if (questDetails.ShowDialog() == true)
            {
                refreshEmployeeList();
            }
        }

        private void tabPatchNotes_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgPatchNotes.ItemsSource == null)
                {
                    bool active = true;
                    dgPatchNotes.ItemsSource = _patchNoteManager.RetrievePatchNotes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void dgPatchNotes_AutoGeneratedColumns(object sender, EventArgs e)
        {
            dgPatchNotes.Columns.Remove(dgPatchNotes.Columns[4]);
        }
        #endregion UIControls
    }
}

