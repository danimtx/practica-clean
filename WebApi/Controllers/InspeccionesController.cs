using Aplication.DTOs;
using Aplication.UseCases;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InspeccionesController : ControllerBase
    {
        private readonly CrearInspeccion _crearUseCase;
        private readonly IInspeccionRepositorio _repositorio;
        private readonly IMapper _mapper;

        public InspeccionesController(
            CrearInspeccion crearUseCase,
            IInspeccionRepositorio repositorio,
            IMapper mapper)
        {
            _crearUseCase = crearUseCase;
            _repositorio = repositorio;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearInspeccionDTO dto)
        {
            var resultado = await _crearUseCase.Ejecutar(dto);
            return Ok(resultado);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarPorCliente([FromQuery] string nombre)
        {
            var inspecciones = await _repositorio.BuscarPorClienteAsync(nombre);
            var resultado = _mapper.Map<IEnumerable<InspeccionDetalleDTO>>(inspecciones);

            return Ok(resultado);
        }
    }
}
