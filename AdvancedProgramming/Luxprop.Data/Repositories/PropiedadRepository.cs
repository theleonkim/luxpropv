using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IPropiedadRepository : IRepositoryBase<Propiedad>
    {
        Task<IEnumerable<Propiedad>> GetByAgenteIdAsync(int agenteId);
        Task<IEnumerable<Propiedad>> GetByUbicacionIdAsync(int ubicacionId);
        Task<IEnumerable<Propiedad>> GetByEstadoPublicacionAsync(string estado);
        Task<IEnumerable<Propiedad>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Propiedad>> GetActivePropertiesAsync();
    }
    
    public class PropiedadRepository : RepositoryBase<Propiedad>, IPropiedadRepository
    {
        public PropiedadRepository()
        {
            DbSet = DbContext.Set<Propiedad>();
        }

        public async Task<IEnumerable<Propiedad>> GetByAgenteIdAsync(int agenteId)
        {
            return await DbSet
                .Include(p => p.Agente)
                .Include(p => p.Ubicacion)
                .Where(p => p.AgenteId == agenteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Propiedad>> GetByUbicacionIdAsync(int ubicacionId)
        {
            return await DbSet
                .Include(p => p.Agente)
                .Include(p => p.Ubicacion)
                .Where(p => p.UbicacionId == ubicacionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Propiedad>> GetByEstadoPublicacionAsync(string estado)
        {
            return await DbSet
                .Include(p => p.Agente)
                .Include(p => p.Ubicacion)
                .Where(p => p.EstadoPublicacion == estado)
                .ToListAsync();
        }

        public async Task<IEnumerable<Propiedad>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await DbSet
                .Include(p => p.Agente)
                .Include(p => p.Ubicacion)
                .Where(p => p.Precio >= minPrice && p.Precio <= maxPrice)
                .ToListAsync();
        }

        public async Task<IEnumerable<Propiedad>> GetActivePropertiesAsync()
        {
            return await DbSet
                .Include(p => p.Agente)
                .Include(p => p.Ubicacion)
                .Where(p => p.EstadoPublicacion == "Active")
                .ToListAsync();
        }
    }
}
