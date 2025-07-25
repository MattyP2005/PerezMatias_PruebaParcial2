using GestionTareas.Modelos;

namespace GestionTareas.API.Servicios.Interfaces
{
    public interface IProyectoRepositorio
    {
        Task<IEnumerable<Proyecto>> ObtenerTodos(); // Obtiene todos los proyectos

        Task<Proyecto?> ObtenerPorId(int id); // Obtiene un proyecto por su ID

        Task<int> Crear(Proyecto proyecto); // Crea un nuevo proyecto y devuelve su ID

        Task<bool> Actualizar(Proyecto proyecto); // Actualiza un proyecto existente

        Task<bool> Eliminar(int id); // Elimina un proyecto por su ID
    }
}
