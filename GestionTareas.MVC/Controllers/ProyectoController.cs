using GestionTareas.API.Consumer;
using GestionTareas.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.MVC.Controllers
{
    public class ProyectoController : Controller
    {
        private readonly Crud<Proyecto> _crudApi;

        public ProyectoController(Crud<Proyecto> crudApi)
        {
            _crudApi = crudApi;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var proyectos = await _crudApi.GetAllAsync(token);
            return View(proyectos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Proyecto proyecto)
        {
            var token = HttpContext.Session.GetString("JWT");
            if (!ModelState.IsValid || string.IsNullOrEmpty(token))
                return View(proyecto);

            var resultado = await _crudApi.PostAsync(proyecto, token);

            if (resultado == null)
            {
                ViewBag.Error = "Error al crear el proyecto";
                return View(proyecto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var proyecto = await _crudApi.GetByIdAsync(id, token);
            if (proyecto == null)
                return NotFound();

            return View(proyecto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Proyecto proyecto)
        {
            var token = HttpContext.Session.GetString("JWT");
            if (!ModelState.IsValid || string.IsNullOrEmpty(token))
                return View(proyecto);

            var resultado = await _crudApi.PutAsync(proyecto.Id, proyecto, token);

            if (!resultado)
            {
                ViewBag.Error = "Error al actualizar el proyecto";
                return View(proyecto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var proyecto = await _crudApi.GetByIdAsync(id, token);
            if (proyecto == null)
                return NotFound();

            return View(proyecto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var resultado = await _crudApi.DeleteAsync(id, token);

            if (!resultado)
            {
                ViewBag.Error = "Error al eliminar el proyecto";
                var proyecto = await _crudApi.GetByIdAsync(id, token);
                return View("Delete", proyecto);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}