using EscobarMatias_ActividadPractica_AppMovil_ll.ViewModels;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Views;

// ============================================================================================
// Code-behind de la pantalla principal.
// Su única responsabilidad es inicializar los componentes visuales,
// asignar el ViewModel como BindingContext y disparar la carga inicial de datos.
// ============================================================================================

public partial class ContactosPage : ContentPage
{
    private readonly ContactosViewModel _viewModel;
    public ContactosPage(ContactosViewModel viewModel)
     {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
        {
        base.OnAppearing();

        if (_viewModel.CargarContactosCommand.CanExecute(null))
            _viewModel.CargarContactosCommand.Execute(null);
    }
}