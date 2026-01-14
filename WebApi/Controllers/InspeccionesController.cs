using Aplication.DTOs;
using Aplication.UseCases;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InspeccionesController : ControllerBase
    {
        private readonly CrearInspeccion _crearUseCase;
        private readonly GestionarArchivoInspeccion _archivoUseCase;
        private readonly IInspeccionRepositorio _repositorio;
        private readonly IMapper _mapper;

        public InspeccionesController(
            CrearInspeccion crearUseCase,
            GestionarArchivoInspeccion archivoUseCase,
            IInspeccionRepositorio repositorio,
            IMapper mapper)
        {
            _crearUseCase = crearUseCase;
            _archivoUseCase = archivoUseCase;
            _repositorio = repositorio;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Policy = "inspeccion:crear")]
        public async Task<IActionResult> Crear([FromBody] CrearInspeccionDTO dto)
        {
            var adminName = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(adminName))
            {
                return Unauthorized("No se pudo identificar el nombre del administrador desde el token.");
            }

            var resultado = await _crearUseCase.Ejecutar(dto, adminName);
            return Ok(resultado);
        }

        [HttpGet("buscar")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> BuscarPorCliente([FromQuery] string nombre)
        {
            var inspecciones = await _repositorio.BuscarPorClienteAsync(nombre);
            var resultado = _mapper.Map<IEnumerable<InspeccionDetalleDTO>>(inspecciones);
            return Ok(resultado);
        }

        [HttpGet("mis-inspecciones")]
        [Authorize(Roles = "Tecnico")]
        public async Task<IActionResult> MisInspecciones()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var inspecciones = await _repositorio.ObtenerPorTecnicoAsync(Guid.Parse(userId));
            var resultado = _mapper.Map<IEnumerable<InspeccionDetalleDTO>>(inspecciones);
            return Ok(resultado);
        }

        [HttpPost("{id}/archivo")]
        [Authorize(Policy = "inspeccion:archivo:subir")]
        public async Task<IActionResult> SubirArchivo(Guid id, [FromForm] IFormFile archivo)
        {
            // TODO: Validar que el técnico solo pueda subir archivos a SUS inspecciones.
            var ruta = await _archivoUseCase.SubirArchivo(id, archivo);
            return Ok(new { rutaArchivo = ruta });
        }

        [HttpDelete("{id}/archivo")]
        [Authorize(Policy = "inspeccion:archivo:borrar")]
        public async Task<IActionResult> EliminarArchivo(Guid id)
        {
            await _archivoUseCase.EliminarArchivo(id);
            return Ok("Archivo eliminado");
        }

        [HttpGet("{id}/descargar")]
        public async Task<IActionResult> DescargarArchivo(Guid id)
        {
            var inspeccion = await _repositorio.ObtenerPorIdAsync(id);
            if (inspeccion == null || string.IsNullOrEmpty(inspeccion.RutaArchivoPdf))
            {
                return NotFound("El archivo no existe o la inspección es inválida.");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", inspeccion.RutaArchivoPdf.TrimStart('/'));
            if (!System.IO.File.Exists(path))
            {
                return NotFound("Archivo no encontrado en el servidor.");
            }

            return PhysicalFile(path, "application/pdf", Path.GetFileName(path));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            var inspeccion = await _repositorio.ObtenerPorIdAsync(id);
            if (inspeccion == null) return NotFound();

            var resultado = _mapper.Map<InspeccionDetalleDTO>(inspeccion);
            return Ok(resultado);
        }

        [HttpPatch("{id}/estado")]
        [Authorize(Policy = "inspeccion:estado")]
        public async Task<IActionResult> ActualizarEstado(Guid id, [FromBody] InspeccionEstadoDTO dto)
        {
            await _repositorio.ActualizarEstadoAsync(id, dto.NuevoEstado);
            return Ok("Estado actualizado");
        }

        [HttpGet] // Nueva ruta para obtener todas las inspecciones
        [Authorize(Roles = "Admin,SuperAdmin")] // Asumiendo que solo los admins pueden ver todas las inspecciones
        public async Task<IActionResult> ObtenerTodas()
        {
            var inspecciones = await _repositorio.ObtenerTodasAsync();
            var resultado = _mapper.Map<IEnumerable<InspeccionDetalleDTO>>(inspecciones);
            return Ok(resultado);
        }

    }
}
