using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EscobarMatias_ActividadPractica_AppMovil_ll.Models.Api;
using EscobarMatias_ActividadPractica_AppMovil_ll.Services;
using EscobarMatias_ActividadPractica_AppMovil_ll.Views;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.ViewModels
{
    // =============================================================================
    // ViewModel de la pantalla "Usuarios API".
    // Carga la lista desde la API, informa el estado en un mensaje y navega al detalle.
    // No conoce HttpClient ni JSON: todo eso lo resuelve IUsuarioApiService.
    // =============================================================================
    public partial class UsuariosViewModel : ObservableObject
    {
        private readonly IUsuarioApiService _usuarioApiService;
        private readonly IDialogService _dialogService;

        public ObservableCollection<UsuarioApi> Usuarios { get; } = new();

        // [NotifyCanExecuteChangedFor] vuelve a evaluar el CanExecute del botón cada vez que
        // cambia EstaCargando: el botón se deshabilita mientras hay una carga en curso.
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CargarUsuariosCommand))]
        private bool estaCargando;

        // Texto del Label de estado de la pantalla.
        [ObservableProperty]
        private string mensajeEstado = "Presioná el botón para cargar los usuarios.";

        // true cuando el último intento falló: la vista usa este valor para pintar el mensaje de rojo.
        [ObservableProperty]
        private bool hayError;

        public UsuariosViewModel(IUsuarioApiService usuarioApiService, IDialogService dialogService)
        {
            _usuarioApiService = usuarioApiService;
            _dialogService = dialogService;
        }

        [RelayCommand(CanExecute = nameof(PuedeCargar))]
        private async Task CargarUsuariosAsync()
        {
            EstaCargando = true;
            HayError = false;
            MensajeEstado = "Cargando usuarios...";
            Usuarios.Clear();

            try
            {
                var resultado = await _usuarioApiService.ObtenerUsuariosAsync();

                if (resultado.EsExitoso)
                {
                    foreach (var usuario in resultado.Datos!)
                        Usuarios.Add(usuario);

                    MensajeEstado = $"Carga exitosa (HTTP {resultado.CodigoHttp}): {Usuarios.Count} usuarios.";
                }
                else
                {
                    HayError = true;
                    MensajeEstado = ObtenerMensajeDeError(resultado.Error, resultado.CodigoHttp);
                }
            }
            catch (Exception ex)
            {
                // Red de seguridad: el servicio ya captura sus errores, pero esto evita
                // que una excepción no prevista cierre la aplicación.
                System.Diagnostics.Debug.WriteLine($"[UsuariosViewModel] {ex}");
                HayError = true;
                MensajeEstado = "Ocurrió un error inesperado al cargar los usuarios.";
            }
            finally
            {
                EstaCargando = false;
            }
        }

        private bool PuedeCargar() => !EstaCargando;

        // Navega a la pantalla de detalle enviando el usuario seleccionado como parámetro.
        [RelayCommand]
        private async Task VerDetalleAsync(UsuarioApi usuario)
        {
            if (usuario is null)
                return;

            try
            {
                var parametrosNavegacion = new Dictionary<string, object>
                {
                    { "Usuario", usuario }
                };

                await Shell.Current.GoToAsync(nameof(UsuarioApiDetallePage), parametrosNavegacion);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UsuariosViewModel] {ex}");
                await _dialogService.MostrarAlertaAsync("Error", "No se pudo abrir el detalle del usuario.");
            }
        }

        // Traduce cada tipo de error a un mensaje claro para el usuario final.
        private static string ObtenerMensajeDeError(TipoErrorApi error, int? codigoHttp) => error switch
        {
            TipoErrorApi.SinConexion => "Sin conexión: revisá tu conexión a internet e intentá de nuevo.",
            TipoErrorApi.TiempoAgotado => "El servidor tardó demasiado en responder. Intentá de nuevo.",
            TipoErrorApi.SolicitudInvalida => $"Solicitud incorrecta (HTTP {codigoHttp}).",
            TipoErrorApi.NoAutorizado => $"No tenés autorización para acceder a este recurso (HTTP {codigoHttp}).",
            TipoErrorApi.NoEncontrado => $"No se encontró el recurso solicitado (HTTP {codigoHttp}).",
            TipoErrorApi.ErrorServidor => $"Error del servidor (HTTP {codigoHttp}). Intentá más tarde.",
            TipoErrorApi.RespuestaInvalida => "La respuesta del servidor no tiene el formato esperado.",
            TipoErrorApi.SinDatos => "El servidor respondió correctamente, pero no devolvió usuarios.",
            _ => codigoHttp is null
                ? "Ocurrió un error desconocido."
                : $"Ocurrió un error desconocido (HTTP {codigoHttp})."
        };
    }
}