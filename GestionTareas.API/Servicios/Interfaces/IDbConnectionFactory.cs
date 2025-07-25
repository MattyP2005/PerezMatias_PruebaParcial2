using System.Data;

namespace GestionTareas.API.Servicios.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CrearConexion();
    }
}
