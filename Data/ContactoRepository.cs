using EscobarMatias_ActividadPractica_AppMovil_ll.Models;
using SQLite;
using System.Diagnostics;


namespace EscobarMatias_ActividadPractica_AppMovil_ll.Data
{
    // ==============================================================================================
    // Única clase de todo el proyecto que conoce la existencia de SQLite.
    // Ni las Views ni los ViewModels deben referenciar "SQLite.*" directamente:
    // todo pasa por los métodos públicos de este Repository.
    // esta clase NO mantiene ninguna lista de contactos en memoria.
    // Cada método público consulta o modifica la base de datos en el momento en que se lo llama.
    // Lo único que se conserva entre llamadas es la conexión física
    // ==============================================================================================

    public class ContactoRepository
    {
        private SQLiteAsyncConnection? _conexion;
        private bool _baseDeDatosInicializada;
        private const string NombreArchivoBaseDeDatos = "agenda_contactos.db3";

        // Crea la conexión (si todavía no existe), crea la tabla "Contacto"
        // si no existe y siembra datos de ejemplo si la tabla está vacía.
        // se ejecuta una solo una vez, la primera vez que se necesita la base de datos.

        private async Task InicializarBaseDeDatosAsync()
        {
            if (_baseDeDatosInicializada)
                return;

            try
            {
                string rutaBaseDeDatos = Path.Combine(FileSystem.AppDataDirectory, NombreArchivoBaseDeDatos);
                _conexion = new SQLiteAsyncConnection(rutaBaseDeDatos);

                // CreateTableAsync no hace nada si la tabla ya existe.
                await _conexion.CreateTableAsync<Contacto>();

                await SembrarDatosIniciales();

                _baseDeDatosInicializada = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ContactoRepository] Error al inicializar la base de datos: {ex.Message}");
                // Se relanza la excepción para que quien llamó (un ViewModel)
                // decida cómo informarle el problema al usuario.
                throw;
            }
        }

        // Inserta 3 contactos de ejemplo SOLO la primera vez que se usa la app.
        // Esto evitará duplicar datos en cada apertura de la aplicación.
        private async Task SembrarDatosIniciales()
        {
            try
            {
                int cantidadDeContactos = await _conexion!.Table<Contacto>().CountAsync();
                if (cantidadDeContactos > 0)
                    return;

                var contactosDeEjemplo = new List<Contacto>
                {
                    new() { Nombre = "Ana García",       Telefono = "342-5123456", Email = "ana.garcia@mail.com" },
                    new() { Nombre = "Juan Pérez",        Telefono = "342-5234567", Email = "juan.perez@mail.com" },
                    new() { Nombre = "Lucía Fernández",   Telefono = "342-5345678", Email = "lucia.fernandez@mail.com" }
                };

                await _conexion.InsertAllAsync(contactosDeEjemplo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ContactoRepository] Error al sembrar datos iniciales: {ex.Message}");
                throw;
            }
        }

        // Devuelve todos los contactos, ordenados alfabéticamente por nombre
        public async Task<List<Contacto>> ObtenerTodosAsync()
        {
            try
            {
                await InicializarBaseDeDatosAsync();

                return await _conexion!.Table<Contacto>()
                                        .OrderBy(c => c.Nombre)
                                        .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ContactoRepository] Error al obtener contactos: {ex.Message}");
                // Se devuelve una lista vacía en vez de relanzar la excepción, para que la pantalla principal no se rompa.
                return [];
            }
        }

        // Busca contactos por nombre. Si el texto está vacío, se comporta igual que ObtenerTodosAsync.
        public async Task<List<Contacto>> BuscarPorNombreAsync(string textoBusqueda)
        {
            try
            {
                await InicializarBaseDeDatosAsync();

                if (string.IsNullOrWhiteSpace(textoBusqueda))
                    return await ObtenerTodosAsync();

                string texto = textoBusqueda.Trim();

                return await _conexion!.Table<Contacto>()
                                        .Where(c => c.Nombre.Contains(texto))
                                        .OrderBy(c => c.Nombre)
                                        .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ContactoRepository] Error al buscar contactos: {ex.Message}");
                return [];
            }
        }

        // Inserta un nuevo contacto. Devuelve true si se insertó correctamente.
        public async Task<bool> InsertarAsync(Contacto contacto)
        {
            try
            {
                await InicializarBaseDeDatosAsync();

                int filasAfectadas = await _conexion!.InsertAsync(contacto);
                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ContactoRepository] Error al insertar contacto: {ex.Message}");
                return false;
            }
        }

        // Actualiza nombre, teléfono y email de un contacto existente, identificado por su Id.
        public async Task<bool> ActualizarAsync(Contacto contacto)
        {
            try
            {
                await InicializarBaseDeDatosAsync();

                int filasAfectadas = await _conexion!.UpdateAsync(contacto);
                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ContactoRepository] Error al actualizar contacto: {ex.Message}");
                return false;
            }
        }

        // Elimina un contacto según su Id. Devuelve true si se eliminó correctamente.
        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                await InicializarBaseDeDatosAsync();

                int filasAfectadas = await _conexion!.DeleteAsync<Contacto>(id);
                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ContactoRepository] Error al eliminar contacto: {ex.Message}");
                return false;
            }
        }
    }
}
