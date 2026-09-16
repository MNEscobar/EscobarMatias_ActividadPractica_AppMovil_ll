using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Helpers
{
    // Convierte un string en bool: true si el string tiene contenido,
    // false si está vacío o null. Se usa para bindear IsVisible de las
    // Labels de error en ContactoModalPage.xaml, de forma que el mensaje
    // rojo solo se vea cuando ValidacionHelper detectó un problema.
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty(value as string);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // No hace falta convertir en sentido inverso para este caso de uso.
            throw new NotImplementedException();
        }
    }
}
