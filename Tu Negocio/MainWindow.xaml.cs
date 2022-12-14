using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using System;
using WinRT.Interop;
using Tu_Negocio.Entities;
using Tu_Negocio.Json;
using Tu_Negocio.Pages;
using System.Linq;
using System.Collections.Generic;

namespace Tu_Negocio
{
    public sealed partial class MainWindow : Window
    {
        public AppViewModel ViewModel { get; set; } = new AppViewModel();
        public static MainWindow Current { get; set; }

        private AppWindow m_AppWindow;
        Settings settings;
        StorageManager dataStorage = new StorageManager();

        public MainWindow()
        {
            this.InitializeComponent();
            Current = this;
            m_AppWindow = GetAppWindowForCurrentWindow();


            if (dataStorage.ReadSettingsFile(ref settings) && settings.SelectedBusinessName != "No Business Selected")
            {
                dataStorage = new StorageManager(settings.SelectedBusinessName);
                dataStorage.GetSelectedBusiness(settings.SelectedBusinessName);
                ProductsCb.ItemsSource = ViewModel.SelectedBusiness.Inventory;
                SaleClientCb.ItemsSource = ViewModel.SelectedBusiness.Clients;
            }
            else
            {
                dataStorage.CreateSettingsFile();
                RegBusinessDialog.Visibility = Visibility.Visible;

                dataStorage.ReadSettingsFile(ref settings);
                SetBusiness();
            }

            MainNv.PaneTitle = ViewModel.SelectedBusiness.Data.Name;
            m_AppWindow.Title = ViewModel.SelectedBusiness.Data.Name;
        }

        private void SetBusiness()
        {
            dataStorage = new StorageManager(settings.SelectedBusinessName);
            ProductsCb.ItemsSource = ViewModel.SelectedBusiness.Inventory;
            SaleClientCb.ItemsSource = ViewModel.SelectedBusiness.Clients;
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
            provider.City = PCityTb.Text;
            provider.DNI = PDniTb.Text;
            provider.Cuit = PCuitTb.Text;
            provider.Adi = PAdiTb.Text;
            provider.Added = DateTime.Now;

            dataStorage.SaveSupplier(provider);
        }

        private void RegBusiness()
        {
            Business bus = new Business();
            bus.Data = new BusinessData();

            bus.Data.Name = BNameTb.Text;
            bus.Data.TelNum = BtelTb.Text;
            bus.Data.Dir = BDirTb.Text;
            bus.Data.City = BCityTb.Text;
            bus.Data.Document = BDocTb.Text;
            bus.Data.Aditional = CAdiTb.Text;
            bus.Data.Email = BEmailTb.Text;
            DateTimeOffset offset = (DateTimeOffset)BFundDateDp.Date;
            bus.Data.FundationDate = offset.UtcDateTime;

            ViewModel.SelectedBusiness = bus;

            dataStorage.SaveBussines();

            settings.SelectedBusinessName = bus.Data.Name;
            SetBusiness();
            dataStorage.ModifySettingsFile(ref settings);
            MainNv.PaneTitle = ViewModel.SelectedBusiness.Data.Name;
            m_AppWindow.Title = ViewModel.SelectedBusiness.Data.Name;
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

        private void CommitCategory_Click(object sender, RoutedEventArgs e)
        {
            settings.Categories.Add(CategoryNameTb.Text);
            dataStorage.WriteCategories(settings.Categories);
        }

        private void OpenCategoryDialog_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryDialog.Visibility = Visibility.Visible;
        }

        private void OpenProductDialog_Click(object sender, RoutedEventArgs e)
        {
            RegProDialog.Visibility = Visibility.Visible;
        }

        private void HideCaDialogBtn_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryDialog.Visibility = Visibility.Collapsed;
        }

        private void HideProDialogBtn_Click(object sender, RoutedEventArgs e)
        {
            RegProDialog.Visibility = Visibility.Collapsed;
        }

        private void CommitProduct_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product();

            product.Name = ProNameTb.Text;
            product.ID = ViewModel.SelectedBusiness.Inventory.Count();
            product.Barcode = ProCodTb.Text;
            product.Price = decimal.Parse(ProPriceTb.Text);
            product.Cost = decimal.Parse(ProCostTb.Text);
            product.Description = ProDesTb.Text;
            product.Atributes = "Not implemented";
            product.Amount = int.Parse(ProDisAmountTb.Text);
            dataStorage.CreateStorageFile(product);
        }

        #region auto suggest
        private void ProCatTb_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var cat in settings.Categories)
                {
                    var found = splitText.All((key) =>
                    {
                        return cat.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(cat);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }

        }

        private void ProCatTb_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            ProCatTb.Text = args.SelectedItem.ToString();
        }
        #endregion

        private void OpenSaleDialog_Click(object sender, RoutedEventArgs e)
        {
            AddSaleDialog.Visibility = Visibility.Visible;
        }

        private void CommitSale_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProductsCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PmCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void HideSaleDialog_Click(object sender, RoutedEventArgs e)
        {
            AddSaleDialog.Visibility = Visibility.Collapsed;
        }

        private void SaleClientCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
