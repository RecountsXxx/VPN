using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using FireSharp;
using FireSharp.Config;
using FireSharp.Response;

namespace VPNFree
{
    /// <summary>
    /// Логика взаимодействия для LogForm.xaml
    /// </summary>
    public partial class LogForm : Window
    {
        FirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "TJBURMVXjCkdLPnhQoqOLkx2K8vG8fIdWxZmcm5K",
            BasePath = "https://vpnusersdb-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        FirebaseClient client;
        public LogForm()
        {
            InitializeComponent();
            //что бь каждый раз не вводить пароль 
            client = new FirebaseClient(config);
            try
            {
                using (StreamReader reader = new StreamReader("cookieFiles.txt"))
                {
                    string boolean = reader.ReadLine();
                    string login = reader.ReadLine();
                    if (boolean == "True")
                    {
                        var response = client.GetAsync((@"vpnusersdb/" + login));
                        User user = response.Result.ResultAs<User>();
                        MainWindow window = new MainWindow(user);
                        window.Show();
                        Close();

                    }
                }
            }
            catch
            {

            }
        }


        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = client.GetAsync((@"vpnusersdb/" + txtUser.Text));
                User user = response.Result.ResultAs<User>();
                if(user != null)
                {
                    if(txtPass.Password == user.Password)
                    {
                        textInvalid.Visibility = Visibility.Collapsed;
                        using (StreamWriter writer = new StreamWriter("cookieFiles.txt",false))
                        {
                            writer.WriteLine("True");
                            writer.WriteLine(user.Name);
                        }

                        MainWindow main = new MainWindow(user);
                        main.Show();
                        Close();
                    }
                    else
                    {
                        txtPass.BorderBrush = System.Windows.Media.Brushes.Red;
                        txtUser.BorderBrush = System.Windows.Media.Brushes.Red;
                        textInvalid.Visibility = Visibility;
                    }
                }
                else
                {
                    txtPass.BorderBrush = System.Windows.Media.Brushes.Red;
                    txtUser.BorderBrush = System.Windows.Media.Brushes.Red;
                    textInvalid.Visibility = Visibility;
                }
            }
            catch
            {

            }

        }

        private void TextBlock_Register(object sender, MouseButtonEventArgs e)
        {
            RegForm form = new RegForm();
            form.Show();
            Close();
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
            Application.Current.Shutdown();
        }

        private void txtUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtUser.BorderBrush = System.Windows.Media.Brushes.DarkGray;
            textInvalid.Visibility = Visibility.Hidden;
        }

        private void txtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtPass.BorderBrush = System.Windows.Media.Brushes.DarkGray;
            textInvalid.Visibility = Visibility.Hidden;
        }
    }
}
