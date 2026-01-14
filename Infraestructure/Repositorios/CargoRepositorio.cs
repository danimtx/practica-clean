using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infraestructure.Repositorios
{
    public class CargoRepositorio : ICargoRepositorio
    {
        private readonly AppDbContext _context;

        public CargoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cargo?> ObtenerPorNombreAsync(string nombre)
        {
            return await _context.Cargos.FirstOrDefaultAsync(c => c.Nombre == nombre);
        }

        public async Task<IEnumerable<Cargo>> ObtenerTodosAsync()
        {
            return await _context.Cargos.ToListAsync();
        }

        public async Task<Cargo?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Cargos.FindAsync(id);
        }

        public async Task<Cargo> CrearCargoAsync(Cargo cargo)
        {
            _context.Cargos.Add(cargo);
            await _context.SaveChangesAsync();
            return cargo;
        }

        public async Task EliminarCargoAsync(Guid id)
        {
            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo != null)
            {
                _context.Cargos.Remove(cargo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
