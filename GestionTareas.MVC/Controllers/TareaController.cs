using GestionTareas.API.Consumer;
using GestionTareas.Modelos;  // Aquí debería estar la clase Tarea
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

            var tareas = await _crudApi.GetAllAsync("api/tareas", token);
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

            var resultado = await _crudApi.PostAsync("api/tareas", tarea, token);

            if (!resultado)
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

            var tarea = await _crudApi.GetByIdAsync($"api/tareas/{id}", token);
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

            var resultado = await _crudApi.PutAsync($"api/tareas/{tarea.Id}", tarea, token);

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

            var tarea = await _crudApi.GetByIdAsync($"api/tareas/{id}", token);
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

            var resultado = await _crudApi.DeleteAsync($"api/tareas/{id}", token);

            if (!resultado)
            {
                ViewBag.Error = "Error al eliminar la tarea";
                var tarea = await _crudApi.GetByIdAsync($"api/tareas/{id}", token);
                return View("Delete", tarea);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}