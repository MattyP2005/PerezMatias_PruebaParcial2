using GestionTareas.API.Consumer;
using GestionTareas.Modelos.DTOs;  // UsuarioDTOs
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.MVC.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly Crud<UsuarioDTOs> _crudApi;

        public UsuarioController(Crud<UsuarioDTOs> crudApi)
        {
            _crudApi = crudApi;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var usuarios = await _crudApi.GetAllAsync(token);  // Sin URL aquí
            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UsuarioDTOs usuario)
        {
            var token = HttpContext.Session.GetString("JWT");
            if (!ModelState.IsValid || string.IsNullOrEmpty(token))
                return View(usuario);

            var resultado = await _crudApi.PostAsync(usuario, token);

            if (resultado == null || resultado == 0)
            {
                ViewBag.Error = "Error al crear usuario";
                return View(usuario);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var usuario = await _crudApi.GetByIdAsync(id, token);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UsuarioDTOs usuario)
        {
            var token = HttpContext.Session.GetString("JWT");
            if (!ModelState.IsValid || string.IsNullOrEmpty(token))
                return View(usuario);

            var resultado = await _crudApi.PutAsync(usuario.Id, usuario, token);

            if (!resultado)
            {
                ViewBag.Error = "Error al actualizar usuario";
                return View(usuario);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var usuario = await _crudApi.GetByIdAsync(id, token);
            if (usuario == null)
                return NotFound();

            return View(usuario);
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
                ViewBag.Error = "Error al eliminar usuario";
                var usuario = await _crudApi.GetByIdAsync(id, token);
                return View("Delete", usuario);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}