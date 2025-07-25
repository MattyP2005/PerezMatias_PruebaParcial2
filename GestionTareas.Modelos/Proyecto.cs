using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTareas.Modelos
{
    public class Proyecto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } // Nombre del proyecto

        public string Descripcion { get; set; } // Descripción del proyecto

        public DateTime FechaCreacion { get; set; } // Fecha de creación del proyecto
    }
}
