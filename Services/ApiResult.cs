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
    // ==============================================================================================
    // Resultado de una llamada a la API: o trae los datos (Datos) o trae el tipo de error (Error).
    // Así el servicio nunca muestra mensajes ni lanza excepciones hacia arriba:
    // devuelve un valor que el ViewModel puede inspeccionar con un simple "if" o "switch".
    // ==============================================================================================
    public record ApiResult<T>(T? Datos, TipoErrorApi Error = TipoErrorApi.Ninguno, int? CodigoHttp = null)
    {
        public bool EsExitoso => Error == TipoErrorApi.Ninguno;

        public static ApiResult<T> Ok(T datos, int codigoHttp) => new(datos, TipoErrorApi.Ninguno, codigoHttp);

        public static ApiResult<T> Falla(TipoErrorApi error, int? codigoHttp = null) => new(default, error, codigoHttp);
    }
}