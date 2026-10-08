using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Services
{
    // Abstrae los cuadros de diálogo para que los ViewModels no dependan de Shell ni de ninguna
    // clase de la interfaz gráfica. Se registra en MauiProgram.cs y llega por constructor.
    public interface IDialogService
    {
        // Muestra un mensaje informativo con un único botón.
        Task MostrarAlertaAsync(string titulo, string mensaje, string aceptar = "OK");

        // Muestra una pregunta con dos botones. Devuelve true si el usuario aceptó.
        Task<bool> ConfirmarAsync(string titulo, string mensaje, string aceptar, string cancelar);
    }
}
