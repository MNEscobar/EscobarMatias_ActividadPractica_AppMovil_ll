using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EscobarMatias_ActividadPractica_AppMovil_ll.Models;
using EscobarMatias_ActividadPractica_AppMovil_ll.Data;
using EscobarMatias_ActividadPractica_AppMovil_ll.Views;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace EscobarMatias_ActividadPractica_AppMovil_ll.ViewModels
{
    // =============================================================================
    // ViewModel de la pantalla principal (lista de contactos).
    // No conoce SQLite ni ninguna clase de "SQLite.*": toda la persistencia
    // pasa por ContactoRepository, que llega inyectado por constructor
    // (el registro de la dependencia está en MauiProgram.cs).
    // =============================================================================

    public partial class ContactosViewModel : ObservableObject
    {
        private readonly ContactoRepository _contactoRepository;
        public ObservableCollection<Contacto> Contactos { get; } = new();

        [ObservableProperty]
        private string textoBusqueda = string.Empty;

        [ObservableProperty]
        private bool estaCargando;

        public ContactosViewModel(ContactoRepository contactoRepository)
        {
            _contactoRepository = contactoRepository;
        }

        [RelayCommand]
        private async Task CargarContactosAsync()
        {
            try
            {
                EstaCargando = true;

                var listaDesdeDb = await _contactoRepository.ObtenerTodosAsync();

                Contactos.Clear();
                foreach (var contacto in listaDesdeDb)
                    Contactos.Add(contacto);
            }
            catch (Exception ex)
            {
                await MostrarErrorAsync("No se pudo cargar la lista de contactos.", ex);
            }
            finally
            {
                EstaCargando = false;
            }
        }

        [RelayCommand]
        private async Task BuscarAsync()
        {
            try
            {
                EstaCargando = true;

                var resultado = await _contactoRepository.BuscarPorNombreAsync(TextoBusqueda);

                Contactos.Clear();
                foreach (var contacto in resultado)
                    Contactos.Add(contacto);
            }
            catch (Exception ex)
            {
                await MostrarErrorAsync("No se pudo realizar la búsqueda.", ex);
            }
            finally
            {
                EstaCargando = false;
            }
        }

        [RelayCommand]
        private async Task AgregarContactoAsync()
        {
            await NavegarAlModalAsync(new Contacto());
        }

        [RelayCommand]
        private async Task EditarContactoAsync(Contacto contacto)
        {
            if (contacto is null)
                return;

            await NavegarAlModalAsync(contacto);
        }

        private async Task NavegarAlModalAsync(Contacto contacto)
        {
            try
            {
                var parametrosNavegacion = new Dictionary<string, object>
                {
                    { "Contacto", contacto }
                };

                await Shell.Current.GoToAsync(nameof(ContactoModalPage), parametrosNavegacion);
            }
            catch (Exception ex)
            {
                await MostrarErrorAsync("No se pudo abrir la pantalla de contacto.", ex);
            }
        }

        [RelayCommand]
        private async Task EliminarContactoAsync(Contacto contacto)
        {
            if (contacto is null)
                return;

            try
            {
                bool confirmar = await Shell.Current.DisplayAlert(
                    "Eliminar contacto",
                    $"¿Seguro que querés eliminar a {contacto.Nombre}?",
                    "Eliminar",
                    "Cancelar");

                if (!confirmar)
                    return;

                bool eliminado = await _contactoRepository.EliminarAsync(contacto.Id);

                if (eliminado)
                    await CargarContactosAsync();
                else
                    await Shell.Current.DisplayAlert("Aviso", "No se pudo eliminar el contacto.", "OK");
            }
            catch (Exception ex)
            {
                await MostrarErrorAsync("Ocurrió un error al eliminar el contacto.", ex);
            }
        }

        private static async Task MostrarErrorAsync(string mensajeUsuario, Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ContactosViewModel] {ex}");

            if (Shell.Current is not null)
                await Shell.Current.DisplayAlert("Error", mensajeUsuario, "OK");
        }

    }
}