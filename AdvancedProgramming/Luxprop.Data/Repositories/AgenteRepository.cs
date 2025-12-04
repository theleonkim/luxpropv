using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IAgenteRepository : IRepositoryBase<Agente>
    {
        Task<Agente?> GetByUsuarioIdAsync(int usuarioId);
        Task<Agente?> GetByCodigoAgenteAsync(string codigoAgente);
        Task<IEnumerable<Agente>> GetBySucursalAsync(string sucursal);
    }
    
    public class AgenteRepository : RepositoryBase<Agente>, IAgenteRepository
    {
        public AgenteRepository()
        {
            DbSet = DbContext.Set<Agente>();
        }

        public async Task<Agente?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await DbSet
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.UsuarioId == usuarioId);
        }

        public async Task<Agente?> GetByCodigoAgenteAsync(string codigoAgente)
        {
            return await DbSet
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.CodigoAgente == codigoAgente);
        }

        public async Task<IEnumerable<Agente>> GetBySucursalAsync(string sucursal)
        {
            return await DbSet
                .Include(a => a.Usuario)
                .Where(a => a.Sucursal == sucursal)
                .ToListAsync();
        }
    }
}
