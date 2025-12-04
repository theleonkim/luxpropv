using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IClienteRepository : IRepositoryBase<Cliente>
    {
        Task<Cliente?> GetByCedulaAsync(string cedula);
        Task<Cliente?> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Cliente>> GetByTipoClienteAsync(string tipoCliente);
        Task<bool> CedulaExistsAsync(string cedula);
    }
    
    public class ClienteRepository : RepositoryBase<Cliente>, IClienteRepository
    {
        public ClienteRepository()
        {
            DbSet = DbContext.Set<Cliente>();
        }

        public async Task<Cliente?> GetByCedulaAsync(string cedula)
        {
            return await DbSet
                .Include(c => c.Usuario)
                .Include(c => c.Expedientes)
                .FirstOrDefaultAsync(c => c.Cedula == cedula);
        }

        public async Task<Cliente?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await DbSet
                .Include(c => c.Usuario)
                .Include(c => c.Expedientes)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        }

        public async Task<IEnumerable<Cliente>> GetByTipoClienteAsync(string tipoCliente)
        {
            return await DbSet
                .Include(c => c.Usuario)
                .Include(c => c.Expedientes)
                .Where(c => c.TipoCliente == tipoCliente)
                .ToListAsync();
        }

        public async Task<bool> CedulaExistsAsync(string cedula)
        {
            return await DbSet.AnyAsync(c => c.Cedula == cedula);
        }
    }
}
