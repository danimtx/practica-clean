using Aplication.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CargosController : ControllerBase
    {
        private readonly ICargoRepositorio _cargoRepositorio;

        public CargosController(ICargoRepositorio cargoRepositorio)
        {
            _cargoRepositorio = cargoRepositorio;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cargos = await _cargoRepositorio.ObtenerTodosAsync();
            return Ok(cargos);
        }

        [HttpPost]
        [Authorize(Policy = "SuperAdminPolicy")]
        [Authorize(Policy = "cargo:gestionar")]
        public async Task<IActionResult> Create([FromBody] CargoDTO dto)
        {
            var cargo = new Cargo { Nombre = dto.Nombre };
            var nuevoCargo = await _cargoRepositorio.CrearCargoAsync(cargo);
            return CreatedAtAction(nameof(GetAll), new { id = nuevoCargo.Id }, nuevoCargo);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "SuperAdminPolicy")]
        [Authorize(Policy = "cargo:gestionar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cargo = await _cargoRepositorio.ObtenerPorIdAsync(id);
            if (cargo == null)
            {
                return NotFound();
            }

            if (cargo.Nombre == "SuperAdmin")
            {
                return BadRequest("No se puede eliminar el cargo SuperAdmin.");
            }

            await _cargoRepositorio.EliminarCargoAsync(id);
            return NoContent();
        }
    }
}
