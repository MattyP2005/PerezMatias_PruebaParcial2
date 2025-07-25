using GestionTareas.Modelos;

namespace GestionTareas.API.Servicios.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<IEnumerable<Usuario>> ObtenerTodos();

        Task<Usuario?> ObtenerPorId(int id);

        Task<Usuario?> ObtenerPorCorreo(string correo);

        Task<int> Crear(Usuario usuario);

        Task<bool> Actualizar(Usuario usuario);

        Task<bool> Eliminar(int id);
    }
}
