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

namespace WPFchart3DV2
{
    /// <summary>
    /// Logique d'interaction pour LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if(txtUserName.Text != "admin" || txtPassword.Password != "1234")
            {
                MyLabel.Visibility = Visibility.Visible;   
            }
            else
            {
                MyLabel.Visibility = Visibility.Hidden;
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
        }
    }
}
