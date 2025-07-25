using GestionTareas.API.Servicios.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace GestionTareas.API.Servicios
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        // ✅ Inyectar IConfiguration
        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public IDbConnection CrearConexion()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
