using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTareas.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; } // Nombre completo del usuario

        public string Correo { get; set; } // Correo electrónico del usuario

        public string PasswordHash { get; set; } // Hash de la contraseña del usuario

        public string Rol { get; set; }  // Puede ser "Administrador"
    }
}
