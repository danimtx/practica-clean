using Aplication.DTOs;
using Aplication.UseCases;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly RegistrarUsuario _registrarUseCase;
        private readonly LoginUsuario _loginUseCase;
        private readonly RefrescarToken _refrescarTokenUseCase;
        private readonly IUsuarioRepositorio _repositorio;

        public UsuariosController(
            RegistrarUsuario registrarUseCase,
            LoginUsuario loginUseCase,
            RefrescarToken refrescarTokenUseCase,
            IUsuarioRepositorio repositorio)
        {
            _registrarUseCase = registrarUseCase;
            _loginUseCase = loginUseCase;
            _refrescarTokenUseCase = refrescarTokenUseCase;
            _repositorio = repositorio;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var resultado = await _loginUseCase.Ejecutar(request);
            if (resultado == null) return Unauthorized("Credenciales inválidas o usuario inactivo.");

            return Ok(resultado);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDTO request)
        {
            var result = await _refrescarTokenUseCase.Ejecutar(request);
            if (result == null)
            {
                return BadRequest("Invalid or expired refresh token.");
            }
            return Ok(result);
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDTO dto)
        {
            var resultado = await _registrarUseCase.Ejecutar(dto);
            return Ok(resultado);
        }

        [HttpPut("gestionar")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Gestionar([FromBody] UsuarioGestionDTO dto)
        {
            var usuario = await _repositorio.ObtenerPorIdAsync(dto.Id);
            if (usuario == null) return NotFound("Usuario no encontrado");

            usuario.Cargo = dto.Cargo;
            usuario.EstaActivo = dto.EstaActivo;
            usuario.Permisos = dto.Permisos;

            await _repositorio.ActualizarUsuarioAsync(usuario);
            return Ok("Usuario actualizado correctamente");
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos([FromQuery] string? cargo = null)
        {
            return Ok(await _repositorio.ObtenerTodosAsync(cargo));
        }
    }
}
