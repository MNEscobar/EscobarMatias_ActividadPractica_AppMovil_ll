using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EscobarMatias_ActividadPractica_AppMovil_ll.Data;
using EscobarMatias_ActividadPractica_AppMovil_ll.Helpers;
using EscobarMatias_ActividadPractica_AppMovil_ll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.ViewModels
{
    // ========================================================================================
    // ViewModel de la pantalla modal de Alta/Edición de contacto.
    // Implementa IQueryAttributable para recibir el Contacto que envía ContactosViewModel.
    // Esto reemplaza la necesidad de acceder a la base de datos desde acá constantemente.
    // ========================================================================================

    public partial class ContactoDetalleViewModel : ObservableObject, IQueryAttributable
    {
        private readonly ContactoRepository _contactoRepository;
        private int _idContacto;

        [ObservableProperty]
        private string nombre = string.Empty;

        [ObservableProperty]
        private string telefono = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string errorNombre = string.Empty;

        [ObservableProperty]
        private string errorTelefono = string.Empty;

        [ObservableProperty]
        private string errorEmail = string.Empty;

        [ObservableProperty]
        private string tituloPagina = "Nuevo contacto";

        public ContactoDetalleViewModel(ContactoRepository contactoRepository)
        {
            _contactoRepository = contactoRepository;
        }

        // Se llama a este método automáticamente apenas se navega a esta página
        // pasándole el diccionario de parámetros de la navegación.
        // Acá se "desempaqueta" el Contacto recibido y se cargan los campos editables del formulario.

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Contacto", out var valor) && valor is Contacto contactoRecibido)
            {
                _idContacto = contactoRecibido.Id;
                Nombre = contactoRecibido.Nombre;
                Telefono = contactoRecibido.Telefono;
                Email = contactoRecibido.Email;

                TituloPagina = _idContacto == 0 ? "Nuevo contacto" : "Editar contacto";
            }
        }

        [RelayCommand]
        private async Task GuardarAsync()
        {
            if (!ValidarCampos())
                return;

            try
            {
                var contacto = new Contacto
                {
                    Id = _idContacto,
                    Nombre = Nombre.Trim(),
                    Telefono = Telefono.Trim(),
                    Email = Email.Trim()
                };

                // _idContacto == 0  -> todavía no existe en la base -> Insertar.
                // _idContacto != 0  -> ya existe -> Actualizar (nombre/teléfono/email).
                bool operacionExitosa = _idContacto == 0
                    ? await _contactoRepository.InsertarAsync(contacto)
                    : await _contactoRepository.ActualizarAsync(contacto);

                if (operacionExitosa)
                    await CerrarModalAsync();
                else
                    await Shell.Current.DisplayAlert("Error", "No se pudo guardar el contacto.", "OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ContactoDetalleViewModel] {ex}");
                await Shell.Current.DisplayAlert("Error", "Ocurrió un error al guardar el contacto.", "OK");
            }
        }

        // Cierra el modal sin guardar ningún cambio.
        [RelayCommand]
        private async Task CancelarAsync()
        {
            await CerrarModalAsync();
        }

        private static Task CerrarModalAsync() => Shell.Current.GoToAsync("..");

        // Ejecuta las validaciones de ValidacionHelper
        private bool ValidarCampos()
        {
            ErrorNombre = ValidacionHelper.EsNombreValido(Nombre)
                ? string.Empty
                : "El nombre es obligatorio.";

            ErrorTelefono = ValidacionHelper.EsTelefonoValido(Telefono)
                ? string.Empty
                : "El teléfono es obligatorio.";

            ErrorEmail = ValidacionHelper.EsEmailValido(Email)
                ? string.Empty
                : "El email no tiene un formato válido.";

            return string.IsNullOrEmpty(ErrorNombre)
                && string.IsNullOrEmpty(ErrorTelefono)
                && string.IsNullOrEmpty(ErrorEmail);
        }
    }
}