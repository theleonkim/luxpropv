using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IUbicacionRepository : IRepositoryBase<Ubicacion>
    {
        Task<IEnumerable<Ubicacion>> GetByProvinciaAsync(string provincia);
        Task<IEnumerable<Ubicacion>> GetByCantonAsync(string canton);
        Task<IEnumerable<Ubicacion>> GetByDistritoAsync(string distrito);
        Task<IEnumerable<Ubicacion>> GetUbicacionesWithPropertiesAsync();
    }
    
    public class UbicacionRepository : RepositoryBase<Ubicacion>, IUbicacionRepository
    {
        public UbicacionRepository()
        {
            DbSet = DbContext.Set<Ubicacion>();
        }

        public async Task<IEnumerable<Ubicacion>> GetByProvinciaAsync(string provincia)
        {
            return await DbSet
                .Include(u => u.Propiedads)
                .Where(u => u.Provincia == provincia)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ubicacion>> GetByCantonAsync(string canton)
        {
            return await DbSet
                .Include(u => u.Propiedads)
                .Where(u => u.Canton == canton)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ubicacion>> GetByDistritoAsync(string distrito)
        {
            return await DbSet
                .Include(u => u.Propiedads)
                .Where(u => u.Distrito == distrito)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ubicacion>> GetUbicacionesWithPropertiesAsync()
        {
            return await DbSet
                .Include(u => u.Propiedads)
                .ThenInclude(p => p.Agente)
                .ToListAsync();
        }
    }
}
