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

namespace OutlookAddIn1.UserControls
{
    /// <summary>
    /// Interaction logic for ConfirmationUser.xaml
    /// </summary>
    public partial class ConfirmationUser : Window
    {
        public MessageBoxResult _result = MessageBoxResult.No;
        public ConfirmationUser()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            YesOrNoAction(false);
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            YesOrNoAction(true);
        }

        private void YesOrNoAction(bool isDone)
        {
            if(isDone)
            {
                _result = MessageBoxResult.Yes;
            }
            else
            {
                _result = MessageBoxResult.No;
            }
            this.Close();
        }

        private void btnAddActivityClose_Click(object sender, RoutedEventArgs e)
        {
            YesOrNoAction(false);
        }
    }
}
