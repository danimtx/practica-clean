using Aplication.DTOs;
using Aplication.UseCases;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        private readonly ICargoRepositorio _cargoRepositorio;
        private readonly GestionarFotoPerfil _gestionarFotoPerfilUseCase;
        private readonly ActualizarPerfilUsuario _actualizarPerfilUseCase;
        private readonly GestionarUsuario _gestionarUsuarioUseCase;
        private readonly IMapper _mapper;

        public UsuariosController(
            RegistrarUsuario registrarUseCase,
            LoginUsuario loginUseCase,
            RefrescarToken refrescarTokenUseCase,
            IUsuarioRepositorio repositorio,
            ICargoRepositorio cargoRepositorio,
            GestionarFotoPerfil gestionarFotoPerfilUseCase,
            ActualizarPerfilUsuario actualizarPerfilUseCase,
            GestionarUsuario gestionarUsuarioUseCase,
            IMapper mapper)
        {
            _registrarUseCase = registrarUseCase;
            _loginUseCase = loginUseCase;
            _refrescarTokenUseCase = refrescarTokenUseCase;
            _repositorio = repositorio;
            _cargoRepositorio = cargoRepositorio;
            _gestionarFotoPerfilUseCase = gestionarFotoPerfilUseCase;
            _actualizarPerfilUseCase = actualizarPerfilUseCase;
            _gestionarUsuarioUseCase = gestionarUsuarioUseCase;
            _mapper = mapper;
        }

        [HttpGet("perfil")]
        [Authorize]
        public async Task<IActionResult> GetPerfil()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var usuario = await _repositorio.ObtenerPorIdAsync(Guid.Parse(userId));
            if (usuario == null)
            {
                return NotFound();
            }

            var resultado = _mapper.Map<UsuarioDTO>(usuario);
            return Ok(resultado);
        }

        [HttpPut("perfil")]
        [Authorize]
        public async Task<IActionResult> UpdatePerfil([FromBody] PerfilEdicionDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var usuario = await _actualizarPerfilUseCase.Ejecutar(Guid.Parse(userId), dto);
                var resultado = _mapper.Map<UsuarioDTO>(usuario);
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
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
            var usuario = await _registrarUseCase.Ejecutar(dto);
            var resultado = _mapper.Map<UsuarioDTO>(usuario);
            return Ok(resultado);
        }

        [HttpPost("foto-perfil")]
        [Authorize]
        public async Task<IActionResult> SubirFotoPerfil([FromForm(Name = "file")] IFormFile archivo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var usuario = await _gestionarFotoPerfilUseCase.Ejecutar(Guid.Parse(userId), archivo);
                var resultado = _mapper.Map<UsuarioDTO>(usuario);
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("gestionar")]
        [Authorize(Policy = "usuario:gestionar")]
        public async Task<IActionResult> Gestionar([FromBody] UsuarioGestionDTO dto)
        {
            try
            {
                await _gestionarUsuarioUseCase.Ejecutar(dto, User);
                return Ok("Usuario actualizado correctamente.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListarTodos([FromQuery] string? cargo = null)
        {
            var usuarios = await _repositorio.ObtenerTodosAsync(cargo);
            var resultado = _mapper.Map<IEnumerable<UsuarioDTO>>(usuarios);
            return Ok(resultado);
        }
    }
}
