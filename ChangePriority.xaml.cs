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

namespace Salimgareeva_Glazki_Save
{
    /// <summary>
    /// Логика взаимодействия для ChangePriority.xaml
    /// </summary>
    public partial class ChangePriority : Window
    {
        public ChangePriority(int maxPriority)
        {
            InitializeComponent();
            NewPriorityTB.Text = maxPriority.ToString();
        }

        private void Change_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        public int NewPriority
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NewPriorityTB.Text))
                    return 0;
                return Convert.ToInt32(NewPriorityTB.Text);
                
            }
        }
    }
}
