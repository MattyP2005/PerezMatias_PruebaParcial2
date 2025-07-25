using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTareas.Modelos
{
    public class Tarea
    {
        public int Id { get; set; }

        public string Titulo { get; set; } // Título breve de la tarea

        public string Descripcion { get; set; } // Descripción detallada de la tarea

        public string Estado { get; set; } // Puede ser "Pendiente", "En Progreso", "Completada"

        public int Prioridad { get; set; } // 1 (Alta), 2 (Media), 3 (Baja)

        public DateTime FechaVencimiento { get; set; } // Fecha límite para completar la tarea

        public int UsuarioId { get; set; } // ID del usuario asignado a la tarea

        public int ProyectoId { get; set; } // ID del proyecto al que pertenece la tarea
    }
}
