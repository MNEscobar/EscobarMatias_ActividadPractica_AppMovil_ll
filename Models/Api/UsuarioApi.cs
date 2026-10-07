using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Models.Api
{
    public class UsuarioApi
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("name")] public string Nombre { get; set; } = "";
        [JsonPropertyName("username")] public string NombreUsuario { get; set; } = "";
        [JsonPropertyName("email")] public string Email { get; set; } = "";
        [JsonPropertyName("phone")] public string Telefono { get; set; } = "";
        [JsonPropertyName("website")] public string SitioWeb { get; set; } = "";
        [JsonPropertyName("company")] public EmpresaApi Empresa { get; set; } = new();
        [JsonPropertyName("address")] public DireccionApi Direccion { get; set; } = new();
    }
    public class EmpresaApi { [JsonPropertyName("name")] public string Nombre { get; set; } = ""; }
    public class DireccionApi
    {
        [JsonPropertyName("city")] public string Ciudad { get; set; } = "";
        [JsonPropertyName("street")] public string Calle { get; set; } = "";
    }
}