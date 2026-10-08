using Microsoft.Extensions.Logging;
using EscobarMatias_ActividadPractica_AppMovil_ll.Data;
using EscobarMatias_ActividadPractica_AppMovil_ll.Services;
using EscobarMatias_ActividadPractica_AppMovil_ll.ViewModels;
using EscobarMatias_ActividadPractica_AppMovil_ll.Views;

namespace EscobarMatias_ActividadPractica_AppMovil_ll
{
    public static class MauiProgram
    {
        // Dirección base de la API pública de pruebas (JSONPlaceholder).
        // Termina en "/" para que las rutas relativas del servicio ("users") se combinen bien.
        private const string UrlBaseApi = "https://jsonplaceholder.typicode.com/";

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // ===================================================================
            // Inyección de dependencias: todo el "cableado" de la app vive acá,
            // Nada se instancia con "new" en Views ni en ViewModels; todo llega por constructor.
            // ===================================================================

            // --- Persistencia local (SQLite) ---
            // ContactoRepository como Singleton: mantiene UNA sola conexión
            // SQLiteAsyncConnection durante toda la vida de la app, en vez de
            // abrir y cerrar el archivo .db3 constantemente.
            builder.Services.AddSingleton<ContactoRepository>();

            // --- Servicios ---
            // Diálogos: Singleton porque no guarda estado.
            builder.Services.AddSingleton<IDialogService, DialogService>();

            // API: AddHttpClient administra el ciclo de vida de HttpClient (evita agotar sockets)
            // y se lo inyecta ya configurado a UsuarioApiService.
            builder.Services.AddHttpClient<IUsuarioApiService, UsuarioApiService>(cliente =>
            {
                cliente.BaseAddress = new Uri(UrlBaseApi);
                cliente.Timeout = TimeSpan.FromSeconds(10);
            });

            // --- ViewModels y Páginas ---
            // Transient: cada vez que Shell navega a una de estas rutas, el contenedor crea
            // una instancia nueva. Es el comportamiento esperado para pantallas de
            // lista/formulario (así el modal siempre arranca limpio).
            builder.Services.AddTransient<ContactosViewModel>();
            builder.Services.AddTransient<ContactosPage>();

            builder.Services.AddTransient<ContactoDetalleViewModel>();
            builder.Services.AddTransient<ContactoModalPage>();

            builder.Services.AddTransient<UsuariosViewModel>();
            builder.Services.AddTransient<UsuariosPage>();

            builder.Services.AddTransient<UsuarioApiDetalleViewModel>();
            builder.Services.AddTransient<UsuarioApiDetallePage>();

            return builder.Build();
        }
    }
}