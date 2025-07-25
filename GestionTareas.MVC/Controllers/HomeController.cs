using GestionTareas.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GestionTareas.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            ViewBag.Token = token;
            return View();
        }
    }
}
