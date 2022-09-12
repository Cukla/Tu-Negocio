using Microsoft.UI.Xaml.Controls;
using Tu_Negocio.Entities;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Tu_Negocio.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SupplierPage : Page
    {
        public AppViewModel ViewModel => MainWindow.Current.ViewModel;
        Supplier SelectedSupplier = new Supplier();

        public SupplierPage()
        {
            this.InitializeComponent();
            SuppliersList.ItemsSource = ViewModel.SelectedBusiness.Suppliers;
        }

        private void SuppliersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedSupplier = ViewModel.SelectedBusiness.Suppliers[SuppliersList.SelectedIndex];

            Ntb.Text = $"Nombre: {SelectedSupplier.Name}";
            TNtb.Text = $"Numero de celular: {SelectedSupplier.TelNum}";
            Ctb.Text = $"Ciudad: {SelectedSupplier.City}";
            Dtb.Text = $"Dirección: {SelectedSupplier.Dir}";
            Dnitb.Text = $"DNI: {SelectedSupplier.DNI}";
            Cuittb.Text = $"Cuit: {SelectedSupplier.Cuit}";
            Aditb.Text = $"Información adicional: {SelectedSupplier.Adi}";
            Addedtb.Text = $"Agregado en: {SelectedSupplier.Added.ToString("MM/dd/yyyy HH:mm")}";
        }
    }
}
