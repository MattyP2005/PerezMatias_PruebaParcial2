using GestionTareas.API.Consumer;
using GestionTareas.Modelos.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.MVC.Controllers
{
    public class AutorizacionController : Controller
    {
        private readonly Crud<UsuarioDTOs> _crudApi;

        public AutorizacionController(Crud<UsuarioDTOs> crudApi)
        {
            _crudApi = crudApi;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegistrarDTOs dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var respuesta = await _crudApi.PostAsync<RegistrarDTOs, string>("api/autorizaciones/registro", dto);

            if (string.IsNullOrEmpty(respuesta))
            {
                ViewBag.Error = "No se pudo registrar.";
                return View(dto);
            }

            TempData["Mensaje"] = "Registro exitoso. Ahora puedes iniciar sesión.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTOs dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var token = await _crudApi.PostAsync<LoginDTOs, string>("api/auth/login", dto);

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View(dto);
            }

            HttpContext.Session.SetString("JWT", token);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWT");
            return RedirectToAction("Login");
        }
    }
}