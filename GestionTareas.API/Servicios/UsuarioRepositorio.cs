using Dapper;
using GestionTareas.API.Servicios.Interfaces;
using GestionTareas.Modelos;
using System.Data;

namespace GestionTareas.API.Servicios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UsuarioRepositorio(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodos()
        {
            using var conexion = _dbConnectionFactory.CrearConexion();
            var query = "SELECT * FROM Usuarios";
            return await conexion.QueryAsync<Usuario>(query);
        }

        public async Task<Usuario?> ObtenerPorId(int id)
        {
            using var conexion = _dbConnectionFactory.CrearConexion();
            var sql = "SELECT * FROM Usuarios WHERE Id = @Id";
            return await conexion.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = id });
        }

        public async Task<Usuario?> ObtenerPorCorreo(string correo)
        {
            using var conexion = _dbConnectionFactory.CrearConexion();
            var sql = "SELECT * FROM Usuarios WHERE Correo = @Correo";
            return await conexion.QueryFirstOrDefaultAsync<Usuario>(sql, new { Correo = correo });
        }

        public async Task<int> Crear(Usuario usuario)
        {
            using var conexion = _dbConnectionFactory.CrearConexion();
            var sql = @"INSERT INTO Usuarios (Nombre, Correo, PasswordHash, Rol)
                        VALUES (@Nombre, @Correo, @PasswordHash, @Rol);
                        SELECT CAST(SCOPE_IDENTITY() as int)";
            return await conexion.ExecuteScalarAsync<int>(sql, usuario);
        }

        public async Task<bool> Actualizar(Usuario usuario)
        {
            using var conexion = _dbConnectionFactory.CrearConexion();
            var sql = @"UPDATE Usuarios SET Nombre = @Nombre, Correo = @Correo,
                        PasswordHash = @PasswordHash, Rol = @Rol
                        WHERE Id = @Id";
            var filas = await conexion.ExecuteAsync(sql, usuario);
            return filas > 0;
        }

        public async Task<bool> Eliminar(int id)
        {
            using var conexion = _dbConnectionFactory.CrearConexion();
            var sql = "DELETE FROM Usuarios WHERE Id = @Id";
            var filas = await conexion.ExecuteAsync(sql, new { Id = id });
            return filas > 0;
        }
    }
}
