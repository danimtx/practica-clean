using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositorios
{
    public class InspeccionRepositorio : IInspeccionRepositorio
    {
        private readonly AppDbContext _context;

        public InspeccionRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Inspeccion> CrearInspeccionAsync(Inspeccion inspeccion)
        {
            _context.Inspecciones.Add(inspeccion);
            await _context.SaveChangesAsync();
            return inspeccion;
        }

        public async Task ActualizarInspeccionAsync(Inspeccion inspeccion)
        {
            _context.Inspecciones.Update(inspeccion);
            await _context.SaveChangesAsync();
        }

        public async Task<Inspeccion?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Inspecciones
                .Include(i => i.Tecnico)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Inspeccion>> ObtenerTodasAsync()
        {
            return await _context.Inspecciones.ToListAsync();
        }

        public async Task<IEnumerable<Inspeccion>> BuscarPorClienteAsync(string nombreCliente)
        {
            return await _context.Inspecciones
                .Include(i => i.Tecnico)
                .Where(i => i.NombreCliente.Contains(nombreCliente))
                .ToListAsync();
        }

        public async Task ActualizarArchivoAsync(Guid id, string? ruta)
        {
            var inspeccion = await _context.Inspecciones.FindAsync(id);
            if (inspeccion != null)
            {
                inspeccion.RutaArchivoPdf = ruta;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ActualizarEstadoAsync(Guid id, string nuevoEstado)
        {
            var inspeccion = await _context.Inspecciones.FindAsync(id);
            if (inspeccion != null)
            {
                inspeccion.Estado = nuevoEstado;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Inspeccion>> ObtenerPorTecnicoAsync(Guid tecnicoId)
        {
            return await _context.Inspecciones
                .Where(i => i.UsuarioId == tecnicoId)
                .ToListAsync();
        }
    }
}
