using SQLite;

// ==============================================================================================
// Representa un contacto de la agenda.
// SQLite mapea directamente a una fila de la tabla "Contacto" en la base de datos local.
// Esta clase NO tiene lógica de negocio ni de validación: es solo la "forma" de los datos. 
// La validación vive en Helpers/ValidacionHelper.cs
// y el acceso a la base de datos vive en Data/ContactoRepository.cs.
// ==============================================================================================

namespace EscobarMatias_ActividadPractica_AppMovil_ll.Models
{
    public class Contacto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
