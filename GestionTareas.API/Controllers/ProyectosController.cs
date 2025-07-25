using GestionTareas.API.Servicios.Interfaces;
using GestionTareas.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProyectosController : ControllerBase
    {
        private readonly IProyectoRepositorio _repositorio;

        public ProyectosController(IProyectoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var proyectos = await _repositorio.ObtenerTodos();
            return Ok(proyectos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var proyecto = await _repositorio.ObtenerPorId(id);
            return proyecto is null ? NotFound() : Ok(proyecto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Proyecto proyecto)
        {
            proyecto.FechaCreacion = DateTime.Now;
            var id = await _repositorio.Crear(proyecto);
            proyecto.Id = id;
            return CreatedAtAction(nameof(Get), new { id = proyecto.Id }, proyecto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Proyecto proyecto)
        {
            if (id != proyecto.Id) return BadRequest();

            var actualizado = await _repositorio.Actualizar(proyecto);
            return actualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _repositorio.Eliminar(id);
            return eliminado ? NoContent() : NotFound();
        }
    }
}
