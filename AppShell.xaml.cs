using EscobarMatias_ActividadPractica_AppMovil_ll.Views;

namespace EscobarMatias_ActividadPractica_AppMovil_ll
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ContactoModalPage), typeof(ContactoModalPage));
        }
    }
}
