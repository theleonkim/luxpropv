using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IRolRepository : IRepositoryBase<Rol>
    {
        Task<Rol?> GetByNameAsync(string nombre);
        Task<IEnumerable<Rol>> GetRolesWithUsersAsync();
    }
    
    public class RolRepository : RepositoryBase<Rol>, IRolRepository
    {
        public RolRepository()
        {
            DbSet = DbContext.Set<Rol>();
        }

        public async Task<Rol?> GetByNameAsync(string nombre)
        {
            return await DbSet
                .Include(r => r.UsuarioRols)
                .ThenInclude(ur => ur.Usuario)
                .FirstOrDefaultAsync(r => r.Nombre == nombre);
        }

        public async Task<IEnumerable<Rol>> GetRolesWithUsersAsync()
        {
            return await DbSet
                .Include(r => r.UsuarioRols)
                .ThenInclude(ur => ur.Usuario)
                .ToListAsync();
        }
    }
}
