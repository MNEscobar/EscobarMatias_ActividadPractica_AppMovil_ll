using EscobarMatias_ActividadPractica_AppMovil_ll.ViewModels;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Views;

public partial class UsuarioApiDetallePage : ContentPage
{
    public UsuarioApiDetallePage(UsuarioApiDetalleViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}