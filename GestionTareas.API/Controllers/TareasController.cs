using GestionTareas.API.Servicios.Interfaces;
using GestionTareas.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TareasController : ControllerBase
    {
        private readonly ITareaRepositorio _repo;

        public TareasController(ITareaRepositorio repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tareas = await _repo.ObtenerTodas();
            return Ok(tareas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var tarea = await _repo.ObtenerPorId(id);
            return tarea is null ? NotFound() : Ok(tarea);
        }

        [HttpGet("proyecto/{proyectoId}")]
        public async Task<IActionResult> GetPorProyecto(int proyectoId)
        {
            var tareas = await _repo.ObtenerPorProyecto(proyectoId);
            return Ok(tareas);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetPorUsuario(int usuarioId)
        {
            var tareas = await _repo.ObtenerPorUsuario(usuarioId);
            return Ok(tareas);
        }

        [HttpGet("filtrar")]
        public async Task<IActionResult> Filtrar([FromQuery] string? estado, [FromQuery] string? prioridad, [FromQuery] DateTime? fecha)
        {
            var tareas = await _repo.Filtrar(estado, prioridad, fecha);
            return Ok(tareas);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Tarea tarea)
        {
            var id = await _repo.Crear(tarea);
            tarea.Id = id;
            return CreatedAtAction(nameof(Get), new { id = tarea.Id }, tarea);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Tarea tarea)
        {
            if (id != tarea.Id) return BadRequest();

            var actualizado = await _repo.Actualizar(tarea);
            return actualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _repo.Eliminar(id);
            return eliminado ? NoContent() : NotFound();
        }
    }
}
