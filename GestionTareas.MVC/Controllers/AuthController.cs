using GestionTareas.API.Consumer;
using GestionTareas.Modelos.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegistrarDTOs dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var response = await _httpClient.PostAsJsonAsync("auth/registro", dto);
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "No se pudo registrar.";
                return View(dto);
            }

            TempData["Mensaje"] = "Registro exitoso. Ahora puedes iniciar sesión.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTOs dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var response = await _httpClient.PostAsJsonAsync("auth/login", dto);
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View(dto);
            }

            var token = await response.Content.ReadAsStringAsync();
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