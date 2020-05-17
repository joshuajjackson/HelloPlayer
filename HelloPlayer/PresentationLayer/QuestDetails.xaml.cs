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
    /// Interaction logic for QuestDetails.xaml
    /// </summary>
    public partial class QuestDetails : Window
    {
        private Quest _selectedQuest;
        private IQuestManager _questManager;
        private bool _addMode = false;

        public QuestDetails()
        {
            InitializeComponent();
        }

        public QuestDetails(Quest selectedQuest, IQuestManager questManager)
        {
            InitializeComponent();
            _selectedQuest = selectedQuest;
            _questManager = questManager;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_addMode == false)
            {
                txtQuestID.Text = _selectedQuest.QuestID.ToString();
                txtQuestNane.Text = _selectedQuest.QuestDescription;
                txtQuestDescription.Text = _selectedQuest.QuestDescription;
                txtWorthExp.Text = _selectedQuest.WorthExp.ToString();
                chkActive.IsChecked = _selectedQuest.Active;
            }
            else
            {
                //SetEditMode();
                chkActive.IsChecked = true;
                chkActive.IsEnabled = false;
            }
        }
    }
}
