using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTareas.Modelos.DTOs
{
    public class RegistrarDTOs
    {
        public string Nombre { get; set; } // Nombre del usuario

        public string Correo { get; set; } // Correo electrónico del usuario

        public string Password { get; set; } // Contraseña del usuario

        public string Rol { get; set; } // Rol del usuario (por ejemplo, "Administrador", "Usuario", etc.)
    }
}
