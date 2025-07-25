using GestionTareas.Modelos;

namespace GestionTareas.API.Servicios.Interfaces
{
    public interface ITareaRepositorio
    {
        Task<IEnumerable<Tarea>> ObtenerTodas(); // Obtiene todas las tareas

        Task<Tarea?> ObtenerPorId(int id); // Obtiene una tarea por su ID

        Task<IEnumerable<Tarea>> ObtenerPorProyecto(int proyectoId); // Obtiene tareas por ID de proyecto

        Task<IEnumerable<Tarea>> ObtenerPorUsuario(int usuarioId); // Obtiene tareas asignadas a un usuario específico

        Task<IEnumerable<Tarea>> Filtrar(string? estado, string? prioridad, DateTime? vencimiento); // Filtra tareas por estado, prioridad o fecha de vencimiento

        Task<int> Crear(Tarea tarea); // Crea una nueva tarea y devuelve su ID

        Task<bool> Actualizar(Tarea tarea); // Actualiza una tarea existente

        Task<bool> Eliminar(int id); // Elimina una tarea por su ID
    }
}
