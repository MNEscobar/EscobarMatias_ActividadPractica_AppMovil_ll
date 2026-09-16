using Microsoft.Extensions.Logging;
using EscobarMatias_ActividadPractica_AppMovil_ll.ViewModels;
using EscobarMatias_ActividadPractica_AppMovil_ll.Views;
using EscobarMatias_ActividadPractica_AppMovil_ll.Data;

namespace EscobarMatias_ActividadPractica_AppMovil_ll
{
    public static class MauiProgram
    {
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

            // ContactoRepository como Singleton: mantiene UNA sola conexión
            // SQLiteAsyncConnection durante toda la vida de la app, en vez de
            // abrir y cerrar el archivo .db3 constantemente.
            //
            builder.Services.AddSingleton<ContactoRepository>();

            // ViewModels y Páginas como Transient: cada vez que Shell navega
            // a una de estas rutas, el contenedor crea una instancia nueva.
            // Es el comportamiento esperado para pantallas de lista/formulario
            // (así el modal siempre arranca limpio).
            builder.Services.AddTransient<ContactosViewModel>();
            builder.Services.AddTransient<ContactosPage>();

            builder.Services.AddTransient<ContactoDetalleViewModel>();
            builder.Services.AddTransient<ContactoModalPage>();

            return builder.Build();
        }
    }
}
