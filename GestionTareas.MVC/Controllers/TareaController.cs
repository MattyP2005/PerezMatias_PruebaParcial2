using GestionTareas.API.Consumer;
using GestionTareas.Modelos;  // Clase Tarea
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.MVC.Controllers
{
    public class TareaController : Controller
    {
        private readonly Crud<Tarea> _crudApi;

        public TareaController(Crud<Tarea> crudApi)
        {
            _crudApi = crudApi;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            // No pasar "api/tareas" sino solo token (GetAllAsync no usa parámetro de URL)
            var tareas = await _crudApi.GetAllAsync(token);
            return View(tareas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tarea tarea)
        {
            var token = HttpContext.Session.GetString("JWT");
            if (!ModelState.IsValid || string.IsNullOrEmpty(token))
                return View(tarea);

            var resultado = await _crudApi.PostAsync(tarea, token);

            if (resultado == null || resultado == 0)  // Si tu PostAsync devuelve int? id
            {
                ViewBag.Error = "Error al crear la tarea";
                return View(tarea);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var tarea = await _crudApi.GetByIdAsync(id, token);
            if (tarea == null)
                return NotFound();

            return View(tarea);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Tarea tarea)
        {
            var token = HttpContext.Session.GetString("JWT");
            if (!ModelState.IsValid || string.IsNullOrEmpty(token))
                return View(tarea);

            var resultado = await _crudApi.PutAsync(tarea.Id, tarea, token);

            if (!resultado)
            {
                ViewBag.Error = "Error al actualizar la tarea";
                return View(tarea);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var tarea = await _crudApi.GetByIdAsync(id, token);
            if (tarea == null)
                return NotFound();

            return View(tarea);
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
                ViewBag.Error = "Error al eliminar la tarea";
                var tarea = await _crudApi.GetByIdAsync(id, token);
                return View("Delete", tarea);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}