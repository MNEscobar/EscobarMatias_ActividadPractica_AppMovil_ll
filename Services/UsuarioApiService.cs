using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using EscobarMatias_ActividadPractica_AppMovil_ll.Models.Api;
using Microsoft.Maui.Networking;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Services
{
    // ==============================================================================================
    // Única clase de la app que conoce HTTP y JSON.
    // El HttpClient llega inyectado (su URL base y Timeout se configuran en MauiProgram.cs).
    // Cada fallo posible se traduce a un TipoErrorApi, para que el ViewModel muestre
    // un mensaje distinto según el problema.
    // ==============================================================================================
    public class UsuarioApiService : IUsuarioApiService
    {
        // Ruta relativa a la BaseAddress configurada en MauiProgram.cs.
        private const string RutaUsuarios = "users";

        private readonly HttpClient _httpClient;

        public UsuarioApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResult<List<UsuarioApi>>> ObtenerUsuariosAsync(CancellationToken cancellationToken = default)
        {
            // Chequeo rápido: si el dispositivo directamente no tiene internet,
            // no tiene sentido esperar a que la petición falle.
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return ApiResult<List<UsuarioApi>>.Falla(TipoErrorApi.SinConexion);

            try
            {
                using HttpResponseMessage respuesta = await _httpClient.GetAsync(RutaUsuarios, cancellationToken);

                int codigo = (int)respuesta.StatusCode;

                // Cualquier código fuera del rango 2xx (400, 401, 403, 404, 500, ...) se clasifica acá.
                if (!respuesta.IsSuccessStatusCode)
                    return ApiResult<List<UsuarioApi>>.Falla(ClasificarCodigoHttp(respuesta.StatusCode), codigo);

                // Deserialización: JSON -> List<UsuarioApi>.
                var usuarios = await respuesta.Content.ReadFromJsonAsync<List<UsuarioApi>>(cancellationToken: cancellationToken);

                if (usuarios is null || usuarios.Count == 0)
                    return ApiResult<List<UsuarioApi>>.Falla(TipoErrorApi.SinDatos, codigo);

                return ApiResult<List<UsuarioApi>>.Ok(usuarios, codigo);
            }
            catch (HttpRequestException ex)
            {
                // Se lanza cuando no se logró establecer la comunicación (sin red, DNS, SSL, servidor caído).
                Debug.WriteLine($"[UsuarioApiService] Error de conexión: {ex.Message}");
                return ApiResult<List<UsuarioApi>>.Falla(TipoErrorApi.SinConexion);
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                // HttpClient lanza TaskCanceledException cuando vence el Timeout.
                // Si en cambio el cancelamiento lo pidió el propio usuario, se deja subir la excepción.
                Debug.WriteLine($"[UsuarioApiService] Tiempo agotado: {ex.Message}");
                return ApiResult<List<UsuarioApi>>.Falla(TipoErrorApi.TiempoAgotado);
            }
            catch (JsonException ex)
            {
                // El servidor respondió 200 pero el cuerpo no es el JSON esperado.
                Debug.WriteLine($"[UsuarioApiService] JSON inválido: {ex.Message}");
                return ApiResult<List<UsuarioApi>>.Falla(TipoErrorApi.RespuestaInvalida);
            }
            catch (NotSupportedException ex)
            {
                // ReadFromJsonAsync lo lanza si el Content-Type no es JSON (por ejemplo, una página HTML).
                Debug.WriteLine($"[UsuarioApiService] Contenido no soportado: {ex.Message}");
                return ApiResult<List<UsuarioApi>>.Falla(TipoErrorApi.RespuestaInvalida);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UsuarioApiService] Error inesperado: {ex}");
                return ApiResult<List<UsuarioApi>>.Falla(TipoErrorApi.Desconocido);
            }
        }

        // Traduce un código HTTP de error a un tipo de error de la app.
        private static TipoErrorApi ClasificarCodigoHttp(HttpStatusCode codigo) => (int)codigo switch
        {
            400 => TipoErrorApi.SolicitudInvalida,
            401 or 403 => TipoErrorApi.NoAutorizado,
            404 => TipoErrorApi.NoEncontrado,
            >= 500 => TipoErrorApi.ErrorServidor,
            _ => TipoErrorApi.Desconocido
        };
    }
}