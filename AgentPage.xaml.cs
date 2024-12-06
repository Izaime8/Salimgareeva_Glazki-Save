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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Salimgareeva_Glazki_Save
{
    /// <summary>
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public AgentPage()
        {
            InitializeComponent();

            var currentAgents = Salimgareeva_GlazkiSaveEntities.GetContext().Agent.ToList();

            AgentsListView.ItemsSource = currentAgents;

            Filter.SelectedIndex = 0;
            Sort.SelectedIndex = 0;
        }

        private void UpdateAgents()
        {
            var currentAgents = Salimgareeva_GlazkiSaveEntities.GetContext().Agent.ToList();

            if (Filter.SelectedIndex == 0)
            {
                AgentsListView.ItemsSource = currentAgents;
            }

            if (Filter.SelectedIndex == 1)
            {
                currentAgents = currentAgents.Where(p => (p.AgentTypeString == "МФО")).ToList();
            }
            if (Filter.SelectedIndex == 2)
            {
                currentAgents = currentAgents.Where(p => (p.AgentTypeString == "ООО")).ToList();
            }
            if (Filter.SelectedIndex == 3)
            {
                currentAgents = currentAgents.Where(p => (p.AgentTypeString == "ЗАО")).ToList();
            }
            if (Filter.SelectedIndex == 4)
            {
                currentAgents = currentAgents.Where(p => (p.AgentTypeString == "МКК")).ToList();
            }
            if (Filter.SelectedIndex == 5)
            {
                currentAgents = currentAgents.Where(p => (p.AgentTypeString == "ОАО")).ToList();
            }
            if (Filter.SelectedIndex == 6)
            {
                currentAgents = currentAgents.Where(p => (p.AgentTypeString == "ПАО")).ToList();
            }


            if (Sort.SelectedIndex == 1)
            {
                currentAgents = currentAgents.OrderBy(p => p.Title).ToList();
            }
            if (Sort.SelectedIndex == 2)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.Title).ToList();
            }
            if (Sort.SelectedIndex == 3) ///////////////////////////////////////////////
            {
                currentAgents = currentAgents.OrderBy(p => p.Title).ToList();
            }
            if (Sort.SelectedIndex == 4)///////////////////////////////////////////////
            {
                currentAgents = currentAgents.OrderBy(p => p.Title).ToList();
            }
            if (Sort.SelectedIndex == 5)
            {
                currentAgents = currentAgents.OrderBy(p => p.Priority).ToList();
            }
            if (Sort.SelectedIndex == 6)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.Priority).ToList();
            }


            string SearchText = Search.Text;
            if (SearchText != null)
            {
                currentAgents = currentAgents.Where(p => (p.Title.ToLower().Contains(SearchText.ToLower())) || p.Phone.Contains(SearchText.ToLower())).ToList(); /////////////////////////////////////////////////////////////////////////////////////////
            }




            AgentsListView.ItemsSource = currentAgents.ToList();
 

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        
        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();
        }
    }
}
