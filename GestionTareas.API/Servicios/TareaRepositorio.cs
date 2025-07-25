using Dapper;
using GestionTareas.API.Servicios.Interfaces;
using GestionTareas.Modelos;

namespace GestionTareas.API.Servicios
{
    public class TareaRepositorio : ITareaRepositorio
    {
        private readonly IDbConnectionFactory _conexionFactory;

        public TareaRepositorio(IDbConnectionFactory conexionFactory)
        {
            _conexionFactory = conexionFactory;
        }

        public async Task<IEnumerable<Tarea>> ObtenerTodas()
        {
            using var db = _conexionFactory.CrearConexion();
            var sql = "SELECT * FROM Tareas";
            return await db.QueryAsync<Tarea>(sql);
        }

        public async Task<Tarea?> ObtenerPorId(int id)
        {
            using var db = _conexionFactory.CrearConexion();
            var sql = "SELECT * FROM Tareas WHERE Id = @Id";
            return await db.QueryFirstOrDefaultAsync<Tarea>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Tarea>> ObtenerPorProyecto(int proyectoId)
        {
            using var db = _conexionFactory.CrearConexion();
            var sql = "SELECT * FROM Tareas WHERE ProyectoId = @ProyectoId";
            return await db.QueryAsync<Tarea>(sql, new { ProyectoId = proyectoId });
        }

        public async Task<IEnumerable<Tarea>> ObtenerPorUsuario(int usuarioId)
        {
            using var db = _conexionFactory.CrearConexion();
            var sql = "SELECT * FROM Tareas WHERE UsuarioAsignadoId = @UsuarioId";
            return await db.QueryAsync<Tarea>(sql, new { UsuarioId = usuarioId });
        }

        public async Task<IEnumerable<Tarea>> Filtrar(string? estado, string? prioridad, DateTime? vencimiento)
        {
            using var db = _conexionFactory.CrearConexion();

            var sql = "SELECT * FROM Tareas WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(estado))
                sql += " AND Estado = @Estado";

            if (!string.IsNullOrWhiteSpace(prioridad))
                sql += " AND Prioridad = @Prioridad";

            if (vencimiento.HasValue)
                sql += " AND FechaVencimiento = @FechaVencimiento";

            return await db.QueryAsync<Tarea>(sql, new { Estado = estado, Prioridad = prioridad, FechaVencimiento = vencimiento });
        }

        public async Task<int> Crear(Tarea tarea)
        {
            using var db = _conexionFactory.CrearConexion();
            var sql = @"INSERT INTO Tareas (Titulo, Descripcion, Estado, Prioridad, FechaVencimiento, ProyectoId, UsuarioAsignadoId)
                        VALUES (@Titulo, @Descripcion, @Estado, @Prioridad, @FechaVencimiento, @ProyectoId, @UsuarioAsignadoId);
                        SELECT CAST(SCOPE_IDENTITY() AS INT)";
            return await db.ExecuteScalarAsync<int>(sql, tarea);
        }

        public async Task<bool> Actualizar(Tarea tarea)
        {
            using var db = _conexionFactory.CrearConexion();
            var sql = @"UPDATE Tareas
                        SET Titulo = @Titulo,
                            Descripcion = @Descripcion,
                            Estado = @Estado,
                            Prioridad = @Prioridad,
                            FechaVencimiento = @FechaVencimiento,
                            ProyectoId = @ProyectoId,
                            UsuarioAsignadoId = @UsuarioAsignadoId
                        WHERE Id = @Id";
            var filas = await db.ExecuteAsync(sql, tarea);
            return filas > 0;
        }

        public async Task<bool> Eliminar(int id)
        {
            using var db = _conexionFactory.CrearConexion();
            var sql = "DELETE FROM Tareas WHERE Id = @Id";
            var filas = await db.ExecuteAsync(sql, new { Id = id });
            return filas > 0;
        }
    }
}
