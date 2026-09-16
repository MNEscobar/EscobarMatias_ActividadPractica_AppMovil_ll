using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Helpers
{
    // Métodos de validación puros: reciben datos simples (strings) y
    // devuelven un resultado, sin depender de otro servicio.
    // Esto los hace fáciles de reutilizar desde cualquier ViewModel.

    public class ValidacionHelper
    {
        // Verifica que el email tenga un formato válido (no necesariamente que exista).
        // Verifica que los campos nombre y teléfono no estén vacíos o sean solo espacios en blanco.

        private static readonly Regex regex = new(
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.Compiled);
        private static readonly Regex PatronEmail = regex;
        public static bool EsNombreValido(string? nombre)
        {
            return !string.IsNullOrWhiteSpace(nombre);
        }
        public static bool EsTelefonoValido(string? telefono)
        {
            return !string.IsNullOrWhiteSpace(telefono);
        }
        public static bool EsEmailValido(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return PatronEmail.IsMatch(email.Trim());
        }
    }
}
