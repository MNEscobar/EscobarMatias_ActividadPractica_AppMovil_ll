using EscobarMatias_ActividadPractica_AppMovil_ll.Models.Api;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Services
{
    // Contrato del servicio de API. Los ViewModels dependen de esta interfaz (no de la clase concreta),
    // lo que permite reemplazar el servicio por uno falso en pruebas.
    public interface IUsuarioApiService
    {
        Task<ApiResult<List<UsuarioApi>>> ObtenerUsuariosAsync(CancellationToken cancellationToken = default);
    }
}