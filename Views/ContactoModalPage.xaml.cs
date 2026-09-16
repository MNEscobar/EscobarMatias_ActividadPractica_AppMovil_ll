using EscobarMatias_ActividadPractica_AppMovil_ll.ViewModels;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Views
{
    // Debe ser partial para coincidir con la declaración generada por XAML.
    public partial class ContactoModalPage : ContentPage
    {
        // Igual que ContactosPage, el ViewModel llega inyectado desde el
        // contenedor de DI armado en MauiProgram.cs.
        public ContactoModalPage(ContactoDetalleViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}