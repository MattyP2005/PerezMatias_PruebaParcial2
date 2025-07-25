using GestionTareas.API.Servicios;
using GestionTareas.API.Servicios.Interfaces;
using GestionTareas.Modelos;
using GestionTareas.Modelos.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace GestionTareas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerarJWTController : ControllerBase
    {
        private readonly IUsuarioRepositorio _repo;
        private readonly GenerarJWT _gener;

        public GenerarJWTController(IUsuarioRepositorio repo, GenerarJWT generar)
        {
            _repo = repo;
            _gener = generar;
        }

        // Post: api/GenerarJWT/registro
        [HttpPost("registro")]
        public async Task<IActionResult> Registrar(RegistrarDTOs dtos)
        {
            var existente = await _repo.ObtenerPorCorreo(dtos.Correo);
            if (existente != null)
                return BadRequest("Correo ya registrado.");

            var usuario = new Usuario
            {
                Nombre = dtos.Nombre,
                Correo = dtos.Correo,
                Rol = dtos.Rol,
                PasswordHash = Hashear(dtos.Password)
            };

            var id = await _repo.Crear(usuario);
            usuario.Id = id;

            var token = _gener.GenerarToken(usuario);
            return Ok(new { Token = token, Usuario = usuario });
        }

        // Post: api/GenerarJWT/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTOs dtos)
        {
            var usuario = await _repo.ObtenerPorCorreo(dtos.Correo);
            if (usuario == null || usuario.PasswordHash != Hashear(dtos.Password))
                return Unauthorized("Credenciales inválidas.");

            var token = _gener.GenerarToken(usuario);
            return Ok(new { Token = token, Usuario = usuario });
        }

        private string Hashear(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
