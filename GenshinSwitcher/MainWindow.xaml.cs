using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GenshinSwitcher.Helpers;

namespace GenshinSwitcher
{
    public partial class MainWindow : Window
    {
        ServerSwitcher serverSwitcher;
        CertificateManager certificateManager;

        public MainWindow()
        {
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            InitializeComponent();
            // base init
            certificateManager = new CertificateManager();
            switchButton.Content = "Retrieving IP address ...";
            certButton.Content = "Getting the certificate status ...";
            statusLabel.Content = Constants.UiUpdatingStatus;
            DisableSwitching();
            InitSwitcher();
        }

        private async void InitSwitcher()
        {
            // certificate init
            await CheckSertificate();

            // load server ip
            var serverIp = await GeneralHelper.GetRealistikAddressAsync();
            if (serverIp == string.Empty)
            {
                MessageBox.Show("You Shouldn't see this" + Environment.NewLine +
                    "Stored IP address will be used");
                serverIp = Constants.RealistikHardcodedIp;
            }
            serverSwitcher = new ServerSwitcher(serverIp);

            // switcher init
            await CheckServer();
        }

        private async Task CheckSertificate()
        {
            certButton.IsEnabled = false;
            var certificateStatus = await certificateManager.GetStatusAsync();
            certButton.Content = certificateStatus ? Constants.UiUninstallCertificate : Constants.UiInstallCertificate;
            certButton.IsEnabled = true;
        }

        private async Task CheckServer()
        {
            switchButton.IsEnabled = false;
            var currentServer = await serverSwitcher.GetCurrentServerAsync();
            statusLabel.Content = (currentServer == Server.Realistik)
                ? Constants.UiYouArePlayingOnRealistik : Constants.UiYouArePlayingOnOfficial;
            switchButton.Content = (currentServer == Server.Official)
                ? Constants.UiSwitchToRealistik : Constants.UiSwitchToOfficial;
            switchButton.IsEnabled = true;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void switchButton_Click(object sender, RoutedEventArgs e)
        {
            Server serv = await serverSwitcher.GetCurrentServerAsync();

            try
            {
                if (serv == Server.Official)
                {
                    serverSwitcher.SwitchToRealistik();
                }
                else
                {
                    serverSwitcher.SwitchToOfficial();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while connecting to RealistikGenshin. If you still can't connect and have tried disabling your antivirus, please ask for help either in our discord or by checking the contact block on VK"
                + string.Format("\r\n\r\nDetails:\r\n{0}", ex.Message));
                Logger.Log(ex);
            }

            await CheckServer();
        }

        private async void sertButton_Click(object sender, RoutedEventArgs e)
        {
            var status = await certificateManager.GetStatusAsync();

            try
            {
                if (status)
                {
                    certificateManager.Uninstall();
                }
                else
                {
                    certificateManager.Install();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while installing / removing the certificate."
                    + string.Format("\r\n\r\nDetails:\r\n{0}", ex.Message));
                Logger.Log(ex);
            }

            await CheckSertificate();
        }

        private void DisableSwitching()
        {
            switchButton.IsEnabled = false;
            certButton.IsEnabled = false;
        }

        private void websiteText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://c.ussr.pl");
        }

        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Fatal(e.Exception);
        }
    }
}
