using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Services
{
    public async Task<ApiResult<List<UsuarioApi>>> ObtenerUsuariosAsync(CancellationToken ct = default)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            return new(null, TipoErrorApi.SinConexion);

        try
        {
            using var resp = await _http.GetAsync("users", ct);

            if (!resp.IsSuccessStatusCode)
                return new(null, MapearEstado(resp.StatusCode), (int)resp.StatusCode);

            var datos = await resp.Content.ReadFromJsonAsync<List<UsuarioApi>>(cancellationToken: ct);
            return datos is { Count: > 0 }
                ? new(datos, CodigoHttp: (int)resp.StatusCode)          // 200
                : new(null, TipoErrorApi.SinDatos, (int)resp.StatusCode);
        }
        catch (HttpRequestException) { return new(null, TipoErrorApi.SinConexion); }
        catch (TaskCanceledException) when (!ct.IsCancellationRequested)
        { return new(null, TipoErrorApi.Timeout); }
        catch (JsonException) { return new(null, TipoErrorApi.RespuestaInvalida); }
        catch (Exception ex) { Debug.WriteLine(ex); return new(null, TipoErrorApi.Desconocido); }
    }

    private static TipoErrorApi MapearEstado(HttpStatusCode c) => (int)c switch
    {
        400 => TipoErrorApi.SolicitudInvalida,
        401 or 403 => TipoErrorApi.NoAutorizado,
        404 => TipoErrorApi.NoEncontrado,
        >= 500 => TipoErrorApi.ErrorServidor,
        _ => TipoErrorApi.Desconocido
    };
}