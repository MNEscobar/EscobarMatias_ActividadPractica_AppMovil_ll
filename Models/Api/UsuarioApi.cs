using System.Text.Json.Serialization;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Models.Api
{
    // ==============================================================================================
    // DTO (Data Transfer Object) que representa un usuario tal como lo devuelve la API pública
    // https://jsonplaceholder.typicode.com/users
    //
    // Los nombres de las propiedades C# están en español, pero el JSON de la API está en inglés:
    // [JsonPropertyName] indica a System.Text.Json con qué campo del JSON se corresponde cada una.
    // Los objetos anidados del JSON ("company", "address") se modelan con sus propias clases.
    // ==============================================================================================
    public class UsuarioApi
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        public string NombreUsuario { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Telefono { get; set; } = string.Empty;

        [JsonPropertyName("website")]
        public string SitioWeb { get; set; } = string.Empty;

        [JsonPropertyName("company")]
        public EmpresaApi Empresa { get; set; } = new();

        [JsonPropertyName("address")]
        public DireccionApi Direccion { get; set; } = new();
    }

    // Objeto anidado "company" del JSON.
    public class EmpresaApi
    {
        [JsonPropertyName("name")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("catchPhrase")]
        public string Eslogan { get; set; } = string.Empty;
    }

    // Objeto anidado "address" del JSON.
    public class DireccionApi
    {
        [JsonPropertyName("street")]
        public string Calle { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string Ciudad { get; set; } = string.Empty;
    }
}