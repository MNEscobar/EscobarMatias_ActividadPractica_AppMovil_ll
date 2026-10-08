using EscobarMatias_ActividadPractica_AppMovil_ll.Views;

namespace EscobarMatias_ActividadPractica_AppMovil_ll
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Rutas de las pantallas que no están en el TabBar y se abren con GoToAsync.
            Routing.RegisterRoute(nameof(ContactoModalPage), typeof(ContactoModalPage));
            Routing.RegisterRoute(nameof(UsuarioApiDetallePage), typeof(UsuarioApiDetallePage));
        }
    }
}
