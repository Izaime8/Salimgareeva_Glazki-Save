using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
using static System.Net.Mime.MediaTypeNames;

namespace Salimgareeva_Glazki_Save
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    /// 


    public partial class AddEditPage : Page
    {



        private Agent _currentAgent = new Agent();
        private ProductSale _currentProductSale = new ProductSale();
         /// <summary>
         /// //////////////////////
         /// </summary>


        public AddEditPage(Agent selectedAgent)
        {
            InitializeComponent();

            AgentTypeComboBox.SelectedIndex = 0;

            if(selectedAgent != null)
            {
                _currentAgent = selectedAgent;
                AgentTypeComboBox.SelectedIndex = _currentAgent.AgentTypeID - 1;
                HistoryOfRealisationListView.Visibility = Visibility.Visible;
            }
            else
            {
                HistoryOfRealisationListView.Visibility = Visibility.Hidden;
            }
            //
            //ComboProductSaleTitle.SelectedIndex = 0;

            var allProducts = Salimgareeva_GlazkiSaveEntities.GetContext().Product.ToList();

            var currentProductSales = Salimgareeva_GlazkiSaveEntities.GetContext().ProductSale.ToList();
            currentProductSales = currentProductSales.Where(p => p.AgentID == _currentAgent.ID).ToList();

            HistoryOfRealisationListView.ItemsSource = currentProductSales;

            ComboProductSaleTitle.ItemsSource = allProducts;


            DataContext = _currentProductSale;

            //DataContext = currentProductSales;
            //DataContext = allProductSales;

            DataContext = _currentAgent;


        }

        private void ChangeLogoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                _currentAgent.Logo = openFileDialog.FileName.ToString();
                Logo.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void Savebutton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentAgent.Title))
                errors.AppendLine("Укажите наименование агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Address))
                errors.AppendLine("Укажите адрес агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.DirectorName))
                errors.AppendLine("Укажите ФИО директора");
            if (AgentTypeComboBox.SelectedItem == null)
                errors.AppendLine("Укажите тип агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Priority.ToString()))
                errors.AppendLine("Укажите приоритет агента");
            if (_currentAgent.Priority <= 0)
                errors.AppendLine("Укажите положительный приоритет агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.INN))
                errors.AppendLine("Укажите ИНН агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.KPP))
                errors.AppendLine("Укажите КПП агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Phone))
                errors.AppendLine("Укажите Телефон агента");
            else
            {
                string strPhone = _currentAgent.Phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("+", "");
                if (((strPhone[1] =='9' ||  strPhone[1] == '4' || strPhone[1] == '8') && strPhone.Length != 11)
                    || strPhone[1] == '3' && strPhone.Length != 12)
                {
                    errors.AppendLine("Укажите правильно телефон агента");
                }
            }

            if (string.IsNullOrWhiteSpace(_currentAgent.Email))
                errors.AppendLine("Укажите почту агента");
            

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            
            _currentAgent.AgentTypeID = AgentTypeComboBox.SelectedIndex + 1;

            if (_currentAgent.ID == 0)
                Salimgareeva_GlazkiSaveEntities.GetContext().Agent.Add(_currentAgent);

            try
            {
                Salimgareeva_GlazkiSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var _currentAgent = (sender as Button).DataContext as Agent;

            var currentProductSale = Salimgareeva_GlazkiSaveEntities.GetContext().ProductSale.ToList();
            currentProductSale = currentProductSale.Where(p => p.AgentID == _currentAgent.ID).ToList();

            var currentAgentPriorityHistory = Salimgareeva_GlazkiSaveEntities.GetContext().AgentPriorityHistory.ToList();
            var currentShop = Salimgareeva_GlazkiSaveEntities.GetContext().Shop.ToList();
            currentAgentPriorityHistory = currentAgentPriorityHistory.Where(p => p.AgentID == _currentAgent.ID).ToList();
            currentShop = currentShop.Where(p => p.AgentID == _currentAgent.ID).ToList();


            if (currentProductSale.Count != 0)
                MessageBox.Show("Невозможно выполнить удаление, так как существует история реализации продуктов");
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Salimgareeva_GlazkiSaveEntities.GetContext().Agent.Remove(_currentAgent);

                        if (currentAgentPriorityHistory.Count != 0)
                        {
                            for (int i = 0; currentAgentPriorityHistory.Count == i; i++)
                                Salimgareeva_GlazkiSaveEntities.GetContext().AgentPriorityHistory.Remove(currentAgentPriorityHistory[i]);
                        }
                        if (currentShop.Count != 0)
                        {
                            for (int i = 0; currentShop.Count == i; i++)
                                Salimgareeva_GlazkiSaveEntities.GetContext().Shop.Remove(currentShop[i]);
                        }
                        Salimgareeva_GlazkiSaveEntities.GetContext().SaveChanges();

                        MessageBox.Show("Информация удалена!");
                        Manager.MainFrame.GoBack();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void AddProductSale_Click(object sender, RoutedEventArgs e) ////////////////////////////////////////////////////////////////////////////////////////////////
        {
            StringBuilder errors = new StringBuilder();
            if (ComboProductSaleTitle.SelectedItem == null)
                errors.AppendLine("Укажите продукт");
            if (string.IsNullOrWhiteSpace(ProductCountTB.Text))
                errors.AppendLine("Укажите количество продуктов");
            bool isProductCountDigits = true;
            for(int i = 0; i < ProductCountTB.Text.Length; i++)
            {
                if (ProductCountTB.Text[i] < '0' || ProductCountTB.Text[i] > '9')
                {
                    isProductCountDigits = false;
                }
            }
            if (!isProductCountDigits)
                errors.AppendLine("Укажите численное положительное продуктов");

            if (string.IsNullOrWhiteSpace(SaleDateDatePicker.Text))
                errors.AppendLine("Укажите дату продажи");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            _currentProductSale.AgentID = _currentAgent.ID;
            _currentProductSale.ProductID = ComboProductSaleTitle.SelectedIndex + 1;
            _currentProductSale.ProductCount = Convert.ToInt32(ProductCountTB.Text);
            _currentProductSale.SaleDate = Convert.ToDateTime(SaleDateDatePicker.Text);
            if (_currentProductSale.ID == 0)
                Salimgareeva_GlazkiSaveEntities.GetContext().ProductSale.Add(_currentProductSale);



            try
            {
                Salimgareeva_GlazkiSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DeleteProductSale_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (ProductSale history in HistoryOfRealisationListView.SelectedItems)
                    {
                        Salimgareeva_GlazkiSaveEntities.GetContext().ProductSale.Remove(history);

                    }
                    Salimgareeva_GlazkiSaveEntities.GetContext().SaveChanges();

                    MessageBox.Show("Информация удалена!");
                    Manager.MainFrame.GoBack();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        
    }
}
