using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IInspeccionRepositorio
    {
        Task<Inspeccion> CrearInspeccionAsync(Inspeccion inspeccion);
        Task ActualizarInspeccionAsync(Inspeccion inspeccion);
        Task<IEnumerable<Inspeccion>> BuscarPorClienteAsync(string nombreCliente);
        Task<Inspeccion?> ObtenerPorIdAsync(Guid id);
        Task<IEnumerable<Inspeccion>> ObtenerTodasAsync();
    }
}
