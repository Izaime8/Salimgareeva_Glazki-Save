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
        int CountRecords;
        int CountPage;
        int CurrentPage;

        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;



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

            //if (Filter.SelectedIndex == 0)
            //{
            //    currentAgents = currentAgents;
            //}


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
                currentAgents = currentAgents.Where(p => (p.Title.ToLower().Contains(SearchText.ToLower())) || p.PhoneDigits.ToLower().Contains(SearchText.ToLower()) || p.Email.ToLower().Contains(SearchText.ToLower())).ToList(); /////////////////////////////////////////////////////////////////////////////////////////
            }

            AgentsListView.ItemsSource = currentAgents;

            TableList = currentAgents;
            ChangePage(0, 0);
        }

        private int Min()
        {
            return CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            PagesNavigation.Visibility = CountRecords <= 10 ? Visibility.Hidden : Visibility.Visible;

            CountPage = CountRecords % 10 > 0 ? (CountRecords / 10 + 1) : (CountRecords / 10);

            Boolean Ifupdate = true;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    for (int i = CurrentPage * 10; i < Min(); i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            for (int i = CurrentPage * 10; i < Min(); i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            for (int i = CurrentPage * 10; i < Min(); i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }

            if (Ifupdate)
            {
                PageListBox.Items.Clear();

                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                TBCount.Text = Min().ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();
                AgentsListView.ItemsSource = CurrentPageList;
                AgentsListView.Items.Refresh();
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
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

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (Visibility == Visibility.Visible)
            {
                Salimgareeva_GlazkiSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                AgentsListView.ItemsSource = Salimgareeva_GlazkiSaveEntities.GetContext().Agent.ToList();
                UpdateAgents();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));

        }
    }
}

