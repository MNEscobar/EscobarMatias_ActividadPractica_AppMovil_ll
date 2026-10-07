using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Services
{
    public enum TipoErrorApi
    {
        Ninguno, SinConexion, Timeout, SolicitudInvalida, NoAutorizado,
        NoEncontrado, ErrorServidor, RespuestaInvalida, SinDatos, Desconocido
    }

    public record ApiResult<T>(T? Datos, TipoErrorApi Error = TipoErrorApi.Ninguno, int? CodigoHttp = null)
    {
        public bool EsExitoso => Error == TipoErrorApi.Ninguno;
    }
}
