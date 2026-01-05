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

        // Para subir el PDF y cambiar estados (Pendiente/Aceptada)
        Task ActualizarInspeccionAsync(Inspeccion inspeccion);

        // LA SOLUCIÓN AL DRIVE: Buscar por nombre de cliente
        Task<IEnumerable<Inspeccion>> BuscarPorClienteAsync(string nombreCliente);

        Task<Inspeccion?> ObtenerPorIdAsync(Guid id);

        // Para el historial o calendario
        Task<IEnumerable<Inspeccion>> ObtenerTodasAsync();
    }
}
