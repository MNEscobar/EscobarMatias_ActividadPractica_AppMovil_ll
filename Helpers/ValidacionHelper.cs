using System.Text.RegularExpressions;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Helpers
{
    // Métodos de validación puros: reciben datos simples (strings) y
    // devuelven un resultado, sin depender de otro servicio.
    // Por eso la clase es estática y se reutiliza desde cualquier ViewModel.
    public static class ValidacionHelper
    {
        // Patrón simple de email: algo@dominio.ext (sin espacios ni dobles arrobas).
        private static readonly Regex PatronEmail = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled);

        // Verifica que el nombre no esté vacío ni sea solo espacios en blanco.
        public static bool EsNombreValido(string? nombre)
        {
            return !string.IsNullOrWhiteSpace(nombre);
        }

        // Verifica que el teléfono no esté vacío ni sea solo espacios en blanco.
        public static bool EsTelefonoValido(string? telefono)
        {
            return !string.IsNullOrWhiteSpace(telefono);
        }

        // Verifica que el email tenga un formato válido (no necesariamente que exista).
        public static bool EsEmailValido(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return PatronEmail.IsMatch(email.Trim());
        }
    }
}