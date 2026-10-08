namespace EscobarMatias_ActividadPractica_AppMovil_ll.Services
{
    // Clasifica los distintos problemas que pueden ocurrir al consumir la API.
    // El servicio solo "clasifica" el error; es el ViewModel quien decide qué mensaje mostrar.
    public enum TipoErrorApi
    {
        Ninguno,
        SinConexion,        // Sin internet, DNS caído o servidor inalcanzable
        TiempoAgotado,      // La petición superó el Timeout configurado
        SolicitudInvalida,  // HTTP 400
        NoAutorizado,       // HTTP 401 / 403
        NoEncontrado,       // HTTP 404
        ErrorServidor,      // HTTP 5xx
        RespuestaInvalida,  // El cuerpo no es un JSON con el formato esperado
        SinDatos,           // HTTP 200 pero la lista vino vacía
        Desconocido         // Cualquier otro caso (otros códigos HTTP o excepciones inesperadas)
    }
}