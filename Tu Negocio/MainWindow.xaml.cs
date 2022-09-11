using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using System;
using WinRT.Interop;
using Tu_Negocio.Entities;
using Tu_Negocio.Json;

namespace Tu_Negocio
{
    public sealed partial class MainWindow : Window
    {
        private AppWindow m_AppWindow;
        Business SelectedBusiness;
        StorageManager dataStorage = new StorageManager("La Casa de Juguetes");

        public MainWindow()
        {
            this.InitializeComponent();

            m_AppWindow = GetAppWindowForCurrentWindow();
            m_AppWindow.Title = "Tu Negocio";
            Expenditure expenditure = new Expenditure();
            if (SelectedBusiness == null)
            {

            }
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }

        private void RegClient()
        {
            Client cli = new Client();

            cli.Name = CNameTb.Text;
            cli.TelNum = CtelTb.Text;
            cli.Dir = CDirTb.Text;
            cli.City = CCityTb.Text;
            cli.DNI = Int32.Parse(CDniTb.Text);
            cli.Cuit = CCuitTb.Text;
            cli.Adi = CAdiTb.Text;
            cli.Added = DateTime.Now;

            dataStorage.SaveClient(cli);
        }

        private void RegProvider()
        {
            Supplier provider = new Supplier();

            provider.Name = PNameTb.Text;
            provider.TelNum = PtelTb.Text;
            provider.Dir = PDirTb.Text;
            provider.DNI = PDniTb.Text;
            provider.Cuit = PCuitTb.Text;
            provider.Adi = PAdiTb.Text;
            provider.Added = DateTime.Now;

            dataStorage.SaveSupplier(provider);
        }

        private void RegBusiness()
        {
            Business bus = new Business();

            bus.Data.Name = BNameTb.Text;
            bus.Data.TelNum = BtelTb.Text;
            bus.Data.Dir = BDirTb.Text;
            bus.Data.City = BCityTb.Text;
            bus.Data.Document = BDocTb.Text;
            bus.Data.Aditional = CAdiTb.Text;
            bus.Data.Email = BEmailTb.Text;
            DateTimeOffset offset = (DateTimeOffset)BFundDateDp.Date;
            bus.Data.FundationDate = offset.UtcDateTime;

            dataStorage.SaveBussines(bus.Data);
        }

        #region Navigation
        private void MainNv_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            SetCurrentNavigationViewItem(args.SelectedItemContainer as NavigationViewItem);
        }

        public void SetCurrentNavigationViewItem(NavigationViewItem item)
        {
            try
            {
                contentFrame.Navigate(Type.GetType(item.Tag.ToString()), item.Content);
                MainNv.Header = item.Content;
                MainNv.SelectedItem = item;
                MainNv.IsPaneOpen = false;
            }
            catch
            {
                ShowMessage("Error", "Ha ocurrido un error inesperado.");
            }
        }

        private async void ShowMessage(string title, string content)
        {
            ContentDialog messageDialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "Ok"
            };
            //set the XamlRoot property
            messageDialog.XamlRoot = MainNv.XamlRoot;

            ContentDialogResult result = await messageDialog.ShowAsync();
        }
        #endregion

        private void CommitClient_Click(object sender, RoutedEventArgs e)
        {
            RegClient();
        }

        private void CommitBussines_Click(object sender, RoutedEventArgs e)
        {
            RegBusiness();
        }

        private void HideBDialogBtn_Click(object sender, RoutedEventArgs e)
        {
            RegBusinessDialog.Visibility = Visibility.Collapsed;
        }

        private void HideCDialogBtn_Click(object sender, RoutedEventArgs e)
        {
            RegClientDialog.Visibility = Visibility.Collapsed;
        }

        private void OpenClientDialog_Click(object sender, RoutedEventArgs e)
        {
            RegClientDialog.Visibility = Visibility.Visible;
        }

        private void OpenBusinessDialog_Click(object sender, RoutedEventArgs e)
        {
            RegBusinessDialog.Visibility = Visibility.Visible;
        }

        private void OpenProviderDialog_Click(object sender, RoutedEventArgs e)
        {
            RegProviderDialog.Visibility = Visibility.Visible;
        }

        private void CommitProvider_Click(object sender, RoutedEventArgs e)
        {
            RegProvider();
        }

        private void HidePDialogBtn_Click(object sender, RoutedEventArgs e)
        {
            RegProviderDialog.Visibility = Visibility.Collapsed;
        }
    }
}
