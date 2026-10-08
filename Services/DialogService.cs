namespace EscobarMatias_ActividadPractica_AppMovil_ll.Services
{
    // Implementación de IDialogService apoyada en la página actual de Shell.
    public class DialogService : IDialogService
    {
        public async Task MostrarAlertaAsync(string titulo, string mensaje, string aceptar = "OK")
        {
            if (Shell.Current is null)
                return;

            await Shell.Current.DisplayAlert(titulo, mensaje, aceptar);
        }

        public async Task<bool> ConfirmarAsync(string titulo, string mensaje, string aceptar, string cancelar)
        {
            if (Shell.Current is null)
                return false;

            return await Shell.Current.DisplayAlert(titulo, mensaje, aceptar, cancelar);
        }
    }
}