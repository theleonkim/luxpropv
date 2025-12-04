using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IUsuarioRolRepository : IRepositoryBase<UsuarioRol>
    {
        Task<IEnumerable<UsuarioRol>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<UsuarioRol>> GetByRolIdAsync(int rolId);
        Task<UsuarioRol?> GetByUsuarioAndRolAsync(int usuarioId, int rolId);
        Task<bool> UserHasRoleAsync(int usuarioId, int rolId);
    }
    
    public class UsuarioRolRepository : RepositoryBase<UsuarioRol>, IUsuarioRolRepository
    {
        public UsuarioRolRepository()
        {
            DbSet = DbContext.Set<UsuarioRol>();
        }

        public async Task<IEnumerable<UsuarioRol>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await DbSet
                .Include(ur => ur.Usuario)
                .Include(ur => ur.Rol)
                .Where(ur => ur.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UsuarioRol>> GetByRolIdAsync(int rolId)
        {
            return await DbSet
                .Include(ur => ur.Usuario)
                .Include(ur => ur.Rol)
                .Where(ur => ur.RolId == rolId)
                .ToListAsync();
        }

        public async Task<UsuarioRol?> GetByUsuarioAndRolAsync(int usuarioId, int rolId)
        {
            return await DbSet
                .Include(ur => ur.Usuario)
                .Include(ur => ur.Rol)
                .FirstOrDefaultAsync(ur => ur.UsuarioId == usuarioId && ur.RolId == rolId);
        }

        public async Task<bool> UserHasRoleAsync(int usuarioId, int rolId)
        {
            return await DbSet.AnyAsync(ur => ur.UsuarioId == usuarioId && ur.RolId == rolId);
        }
    }
}
