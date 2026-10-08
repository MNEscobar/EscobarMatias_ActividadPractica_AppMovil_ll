using EscobarMatias_ActividadPractica_AppMovil_ll.ViewModels;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Views;

// ============================================================================================
// Code-behind de la pantalla "Usuarios API".
// Su única responsabilidad es inicializar los componentes visuales y asignar el ViewModel
// como BindingContext. No contiene lógica: la carga se dispara desde el botón (comando).
// ============================================================================================
public partial class UsuariosPage : ContentPage
{
    public UsuariosPage(UsuariosViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}