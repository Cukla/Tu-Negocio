using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using Tu_Negocio.Entities;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Tu_Negocio.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClientsPage : Page
    {
        public AppViewModel ViewModel => MainWindow.Current.ViewModel;
        Client SelectedClient = new Client();
        public ClientsPage()
        {
            this.InitializeComponent();
            ClientList.ItemsSource = ViewModel.SelectedBusiness.Clients;
        }

        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedClient = ViewModel.SelectedBusiness.Clients[ClientList.SelectedIndex];

            Ntb.Text = $"Nombre: {SelectedClient.Name}";
            TNtb.Text = $"Numero de celular: {SelectedClient.TelNum}";
            Ctb.Text = $"Ciudad: {SelectedClient.City}";
            Dtb.Text = $"Dirección: {SelectedClient.Dir}";
            Dnitb.Text = $"DNI: {SelectedClient.DNI}";
            Cuittb.Text = $"Cuit: {SelectedClient.Cuit}";
            Aditb.Text = $"Información adicional: {SelectedClient.Adi}";
            Addedtb.Text = $"Agregado en: {SelectedClient.Added.ToString("MM/dd/yyyy HH:mm")}";
        }
    }
}
