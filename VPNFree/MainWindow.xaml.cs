using FireSharp.Config;
using FireSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ContextMenu = System.Windows.Controls.ContextMenu;
using MessageBox = System.Windows.Forms.MessageBox;

namespace VPNFree
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "TJBURMVXjCkdLPnhQoqOLkx2K8vG8fIdWxZmcm5K",
            BasePath = "https://vpnusersdb-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        private FirebaseClient client;

        public User user { get; set; }

        private System.Windows.Forms.ContextMenuStrip contextMenu = new System.Windows.Forms.ContextMenuStrip();
        private System.Windows.Forms.NotifyIcon notify = new NotifyIcon();

        private static MainWindow Window;
      
        private static string FolderPath => string.Concat(Directory.GetCurrentDirectory(),"\\VPN");
        private string host = "PL226.vpnbook.com";
       
        private DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Send);
        private DateTime dateTime = new DateTime(0,0);
      
        public MainWindow(User user)
        {
            InitializeComponent();
            this.user = user;

            #region Check Premium User
            if(user != null)
            {
                client = new FirebaseClient(config);
                var response = client.GetAsync((@"vpnusersdb/" + user.Name));
                user = response.Result.ResultAs<User>();
                if (user.IsPremium == true)
                {
                    premiumHost1.Visibility = Visibility.Visible;
                    premiumHost2.Visibility = Visibility.Visible;
                }
                else
                {
                    premiumHost1.Visibility = Visibility.Collapsed;
                    premiumHost2.Visibility = Visibility.Collapsed;
                }
                #endregion
            }


            #region Other
            Window = this;

            comboBoxHostName.SelectedIndex = 0;
            #endregion
            
            #region Timer
            timer.Interval = new TimeSpan(0,0,0,1);
            timer.Tick += Timer_Tick;
            #endregion
           
            #region ContextMenu
            contextMenu.Items.Add("Show",System.Drawing.Image.FromFile("visual.png"), new EventHandler(showMenuItem));
            contextMenu.Items.Add("Disconnect", System.Drawing.Image.FromFile("disconnect.png"), new EventHandler(disconnectMenuItem));
            contextMenu.Items.Add("Exit", System.Drawing.Image.FromFile("exit.png"), new EventHandler(exitMenuItem));
            notify.ContextMenuStrip = contextMenu;
            #endregion
        }

        #region Timer
        private void Timer_Tick(object sender, EventArgs e)
        {
            dateTime = dateTime.AddSeconds(1);
            timerLabel.Content = dateTime.ToLongTimeString();
        }
        #endregion
    
        #region Buttons
        private void OnOffVpn_Click(object sender, RoutedEventArgs e)
        {
            if(onOffButton.IsChecked == true)
            {
                connectRadioBtn.Background = new SolidColorBrush(Color.FromRgb(5, 196, 33));
                disconnectRadioBtn.Background = new SolidColorBrush(Color.FromRgb(56, 56, 56));
                disconConnecLabel.Content = "Connected";
                disconConnecLabel.Foreground = new SolidColorBrush(Color.FromRgb(5, 196, 33));

                Task.Factory.StartNew(new Action(Connect));
                timer.Start();
            }
            else
            {
                connectRadioBtn.Background = new SolidColorBrush(Color.FromRgb(56, 56, 56));
                disconnectRadioBtn.Background = Brushes.Red;
                disconConnecLabel.Content = "Disconnected";
                disconConnecLabel.Foreground = Brushes.Red;
                timerLabel.Content = "0:00:00";
                dateTime = new DateTime(0, 0);
                timer.Stop();
                Task.Factory.StartNew(new Action(Disconnect));

            }

        }
        private void menuBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(this,user);
            Hide();
            settingsWindow.ShowDialog();
            try
            {
                Show();
            }
            catch
            {

            }
    
        }
        #endregion

        #region VPN Functions

        public void Connect()
        {


            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            var sb = new StringBuilder();
            sb.AppendLine("[VPN]");
            sb.AppendLine("MEDIA=rastapi");
            sb.AppendLine("Port=VPN2-0");
            sb.AppendLine("Device=WAN Miniport (IKEv2)");
            sb.AppendLine("DEVICE=vpn");
            sb.AppendLine("PhoneNumber=" + host);

            File.WriteAllText(FolderPath + "\\VpnConnection.pbk", sb.ToString());

            sb = new StringBuilder();
            sb.AppendLine("rasdial \"VPN\" " + "vpnbook" + " " + "rxtasfh" + " /phonebook:\"" + FolderPath +
                          "\\VpnConnection.pbk\"");

            File.WriteAllText(FolderPath + "\\VpnConnection.bat", sb.ToString());

            var newProcess = new Process
            {
                StartInfo =
                {
                    FileName = FolderPath + "\\VpnConnection.bat",
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            newProcess.Start();
            newProcess.WaitForExit();

        }
        public void Disconnect()
        {
            File.WriteAllText(FolderPath + "\\VpnDisconnect.bat", "rasdial /d");

            var newProcess = new Process
            {
                StartInfo =
                {
                    FileName = FolderPath + "\\VpnDisconnect.bat",
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };

            newProcess.Start();
            newProcess.WaitForExit();
        }

        private void comboBoxHostName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TYT POMENYAT
            if (comboBoxHostName.SelectedIndex == 0)
            {
                host = "PL226.vpnbook.com";

            }
            if (comboBoxHostName.SelectedIndex == 1)
            {
                host = "DE4.vpnbook.com";
            }
            if (comboBoxHostName.SelectedIndex == 2)
            {
                host = "us1.vpnbook.com";
            }
            if (comboBoxHostName.SelectedIndex == 3)
            {
                host = "us2.vpnbook.com";
            }
            if (comboBoxHostName.SelectedIndex == 4)
            {
                host = "ca222.vpnbook.com";
            }
            if (comboBoxHostName.SelectedIndex == 5)
            {
                host = "ca198.vpnbook.com";
            }
            if (comboBoxHostName.SelectedIndex == 6)
            {
                host = "fr1.vpnbook.com";
            }
            if (comboBoxHostName.SelectedIndex == 7)
            {
                host = "fr8.vpnbook.com";
            }
        }
        #endregion

        #region MenuItem Task bar
        private void showMenuItem(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            Show();
        }
        private void disconnectMenuItem(object sender, EventArgs e)
        {
            connectRadioBtn.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            disconnectRadioBtn.Background = Brushes.Red;
            disconConnecLabel.Content = "Disconnected";
            disconConnecLabel.Foreground = Brushes.Red;
            timerLabel.Content = "0:00:00";
            dateTime = new DateTime(0, 0);
            onOffButton.IsChecked = false;
            timer.Stop();
            Task.Factory.StartNew(Disconnect);

        }
        private void exitMenuItem(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void Notify_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Show();
                ShowInTaskbar = true;

            }
        }
        #endregion

        #region Window Button

        private void Window_Closed(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void hideBtn_Click(object sender, RoutedEventArgs e)
        {
            //help
            notify.Icon = new System.Drawing.Icon("/ResourcesImage/ic_vpn_lock_128_28770.ico");
            notify.Visible = true;
            notify.Text = "Free VPN";
            ShowInTaskbar = false;
            notify.MouseClick += Notify_MouseClick; ;
            notify.ShowBalloonTip(2, "Free VPN", "VPN will run in the background", ToolTipIcon.Info);
            Hide();
        }
        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {

            Environment.Exit(0);
        }
        private void Drag(object sender, RoutedEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                MainWindow.Window.DragMove();
            }
        }
        #endregion

        private void buyDonate_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://pay.fondy.eu/merchants/63d764e2ef27ae135c592cdd8630978998c7f723/default/index.html?button=e9534f267a441d0b6bd218543f432a2be9501158");
        }
    }
}
