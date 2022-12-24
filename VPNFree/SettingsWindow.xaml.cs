using FireSharp.Config;
using FireSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Window = System.Windows.Window;
using FirebaseAdmin.Auth;
using FireSharp.Interfaces;
using FirebaseAdmin;
using FireSharp.Response;

namespace VPNFree
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        FirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "TJBURMVXjCkdLPnhQoqOLkx2K8vG8fIdWxZmcm5K",
            BasePath = "https://vpnusersdb-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        FirebaseClient client;
        private MainWindow _window;
        public User user { get; set; }
        public SettingsWindow(MainWindow window, User user)
        {
            client = new FirebaseClient(config);
            _window = window;
            this.user = user;
            this.DataContext = user;
            InitializeComponent();
            if(user.IsPremium != true)
            {
                premiumText.Text = "Simple";
            }
            else
            {
                premiumText.Text = "Premium";
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            _window.Show();
            
        }

   

        private void changePassword_Click(object sender, RoutedEventArgs e)
        {
            //client.ChangePassword(user.Name, "123bohdan", "RABOETA");
        }

        private void changeEmail_Click(object sender, RoutedEventArgs e)
        {

        }

        private void exitAccount_Click(object sender, RoutedEventArgs e)
        {
            using(StreamWriter writer = new StreamWriter("cookieFiles.txt", false))
            {
                Close();
                LogForm logForm = new LogForm();
                logForm.Show();
                _window.Close();
            }
        }

        private void deleteAccount_Click(object sender, RoutedEventArgs e)
        {
            client.DeleteAsync("vpnusersdb/" + user.Name);
            using (StreamWriter writer = new StreamWriter("cookieFiles.txt", false))
            {
                Close();
                LogForm logForm = new LogForm();
                logForm.Show();
                _window.Close();
            }
        }
    }
}
