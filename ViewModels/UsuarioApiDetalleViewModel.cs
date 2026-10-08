using CommunityToolkit.Mvvm.ComponentModel;
using EscobarMatias_ActividadPractica_AppMovil_ll.Models.Api;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.ViewModels
{
    // ========================================================================================
    // ViewModel de la pantalla de detalle de un usuario de la API.
    // Implementa IQueryAttributable para recibir el UsuarioApi que envía UsuariosViewModel
    // (mismo mecanismo que ya usa ContactoDetalleViewModel).
    // ========================================================================================
    public partial class UsuarioApiDetalleViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private UsuarioApi? usuario;

        // Shell llama a este método al navegar, pasando el diccionario de parámetros.
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Usuario", out var valor) && valor is UsuarioApi usuarioRecibido)
                Usuario = usuarioRecibido;
        }
    }
}