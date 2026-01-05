using Aplication.DTOs;
using Aplication.UseCases;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly RegistrarUsuario _registrarUseCase;
        private readonly IUsuarioRepositorio _repositorio;

        public UsuariosController(RegistrarUsuario registrarUseCase, IUsuarioRepositorio repositorio)
        {
            _registrarUseCase = registrarUseCase;
            _repositorio = repositorio;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDTO dto)
        {
            var resultado = await _registrarUseCase.Ejecutar(dto);
            return Ok(resultado);
        }

        [HttpPut("gestionar")]
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
        public async Task<IActionResult> ListarTodos()
        {
            return Ok(await _repositorio.ObtenerTodosAsync());
        }
    }
}
