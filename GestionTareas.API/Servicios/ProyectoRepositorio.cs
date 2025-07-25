using Dapper;
using GestionTareas.API.Servicios.Interfaces;
using GestionTareas.Modelos;

namespace GestionTareas.API.Servicios
{
    public class ProyectoRepositorio : IProyectoRepositorio
    {
        private readonly IDbConnectionFactory _conexionFactory;

        public ProyectoRepositorio(IDbConnectionFactory conexionFactory)
        {
            _conexionFactory = conexionFactory;
        }

        public async Task<IEnumerable<Proyecto>> ObtenerTodos()
        {
            using var conexion = _conexionFactory.CrearConexion();
            var sql = "SELECT * FROM Proyectos";
            return await conexion.QueryAsync<Proyecto>(sql);
        }

        public async Task<Proyecto?> ObtenerPorId(int id)
        {
            using var conexion = _conexionFactory.CrearConexion();
            var sql = "SELECT * FROM Proyectos WHERE Id = @Id";
            return await conexion.QueryFirstOrDefaultAsync<Proyecto>(sql, new { Id = id });
        }

        public async Task<int> Crear(Proyecto proyecto)
        {
            using var conexion = _conexionFactory.CrearConexion();
            var sql = @"INSERT INTO Proyectos (Nombre, Descripcion, FechaCreacion)
                        VALUES (@Nombre, @Descripcion, @FechaCreacion);
                        SELECT CAST(SCOPE_IDENTITY() AS INT)";
            return await conexion.ExecuteScalarAsync<int>(sql, proyecto);
        }

        public async Task<bool> Actualizar(Proyecto proyecto)
        {
            using var conexion = _conexionFactory.CrearConexion();
            var sql = @"UPDATE Proyectos
                        SET Nombre = @Nombre, Descripcion = @Descripcion
                        WHERE Id = @Id";
            var filas = await conexion.ExecuteAsync(sql, proyecto);
            return filas > 0;
        }

        public async Task<bool> Eliminar(int id)
        {
            using var conexion = _conexionFactory.CrearConexion();
            var sql = "DELETE FROM Proyectos WHERE Id = @Id";
            var filas = await conexion.ExecuteAsync(sql, new { Id = id });
            return filas > 0;
        }
    }
}
