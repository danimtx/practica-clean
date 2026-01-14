using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICargoRepositorio
    {
        Task<Cargo?> ObtenerPorNombreAsync(string nombre);
        Task<IEnumerable<Cargo>> ObtenerTodosAsync();
        Task<Cargo?> ObtenerPorIdAsync(Guid id);
        Task<Cargo> CrearCargoAsync(Cargo cargo);
        Task EliminarCargoAsync(Guid id);
    }
}
