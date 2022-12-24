using FireSharp.Config;
using FireSharp;
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
using VPNFree;
using System.Drawing;

namespace VPNFree
{
    /// <summary>
    /// Логика взаимодействия для RegForm.xaml
    /// </summary>
    public partial class RegForm : Window
    {
        FirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "TJBURMVXjCkdLPnhQoqOLkx2K8vG8fIdWxZmcm5K",
            BasePath = "https://vpnusersdb-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        FirebaseClient client;


        public RegForm()
        {
            InitializeComponent();
            client = new FirebaseClient(config);
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (txtUser.Text.Length != 0 && txtUser.Text.Length >= 3)
            {
                if (txtPass.Password.Length != 0 && txtPass.Password.Length >= 3)
                {
                    if (txtConfirmPass.Password.Length != 0 && txtPass.Password.Length >= 3)
                    {
                        if (txtConfirmPass.Password == txtPass.Password)
                        {
                            try
                            {
                                var one = client.GetAsync(@"vpnusersdb/" + txtUser.Text);
                                var two = one.Result.ResultAs<User>();
                                if (two == null)
                                {
                                    User user = new User { Name = txtUser.Text, Created = DateTime.Now.ToShortDateString(), IsPremium = false, Password = txtConfirmPass.Password };
                                    var result = client.SetAsync(@"vpnusersdb/" + txtUser.Text, user);
                                    textInvalid.Text = "Succefull!";
                                    textInvalid.Foreground = System.Windows.Media.Brushes.GreenYellow;
                                    textInvalid.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    textInvalid.Text = "This login is already taken";
                                    txtUser.BorderBrush = System.Windows.Media.Brushes.Red;
                                    textInvalid.Visibility = Visibility.Visible;
                                }
                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            txtConfirmPass.BorderBrush = System.Windows.Media.Brushes.Red;
                            textInvalid.Text = "Passwords do not match";
                            textInvalid.Visibility = Visibility.Visible;
                            textInvalid.Foreground = System.Windows.Media.Brushes.Red;
                        }
                    }
                    else
                    {
                        txtConfirmPass.BorderBrush = System.Windows.Media.Brushes.Red;
                        textInvalid.Text = "Confirm password must contain at least 3 characters";
                        textInvalid.Visibility = Visibility.Visible;
                        textInvalid.Foreground = System.Windows.Media.Brushes.Red;
                    }
                }
                else
                {
                    txtPass.BorderBrush = System.Windows.Media.Brushes.Red;
                    textInvalid.Text = "Password must contain at least 3 characters";
                    textInvalid.Visibility = Visibility.Visible;
                    textInvalid.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                txtUser.BorderBrush = System.Windows.Media.Brushes.Red;
                textInvalid.Text = "Login must contain at least 3 characters";
                textInvalid.Visibility = Visibility.Visible;
                textInvalid.Foreground = System.Windows.Media.Brushes.Red;
                if (txtPass.Password.Length < 3)
                {
                    txtPass.BorderBrush = System.Windows.Media.Brushes.Red;
                    textInvalid.Text = "Password must contain at least 3 characters";
                    textInvalid.Visibility = Visibility.Visible;
                    textInvalid.Foreground = System.Windows.Media.Brushes.Red;
                }
                if (txtConfirmPass.Password.Length < 3)
                {
                    txtPass.BorderBrush = System.Windows.Media.Brushes.Red;
                    textInvalid.Text = "Confirm password must contain at least 3 characters";
                    textInvalid.Visibility = Visibility.Visible;
                    textInvalid.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
            if(txtConfirmPass.Password.Length == 0)
            {
                txtConfirmPass.BorderBrush = System.Windows.Media.Brushes.Red;
            }
            if (txtPass.Password.Length == 0)
            {
                txtPass.BorderBrush = System.Windows.Media.Brushes.Red;
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
            Application.Current.Shutdown();
        }
        private void TextBlock_LogIn(object sender, MouseButtonEventArgs e)
        {
            LogForm logForm = new LogForm();
            logForm.Show();
            Close();
        }

        private void txtUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            textInvalid.Foreground = System.Windows.Media.Brushes.Red;
            txtUser.BorderBrush = System.Windows.Media.Brushes.DarkGray;
            textInvalid.Visibility = Visibility.Hidden;
        }

        private void txtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            textInvalid.Foreground = System.Windows.Media.Brushes.Red;
            txtPass.BorderBrush = System.Windows.Media.Brushes.DarkGray;
            textInvalid.Visibility = Visibility.Hidden;
        }

        private void txtConfirmPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            textInvalid.Foreground = System.Windows.Media.Brushes.Red;
            txtConfirmPass.BorderBrush = System.Windows.Media.Brushes.DarkGray;
            textInvalid.Visibility = Visibility.Hidden;
        }
    }
}
