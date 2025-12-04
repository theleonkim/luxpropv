using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario?> GetByEmailAndPasswordAsync(string email, string password);
        Task<IEnumerable<Usuario>> GetActiveUsersAsync();
        Task<IEnumerable<Usuario>> GetByRoleAsync(int roleId);
        Task<bool> EmailExistsAsync(string email);
        Task<Usuario?> GetByResetTokenAsync(string token);
        Task<bool> SaveResetTokenAsync(int userId, string token, DateTime expiration);
        Task<bool> ClearResetTokenAsync(int userId);
        Task<List<Usuario>> GetBuyersAsync();
        

    }

    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository()
        {
            DbSet = DbContext.Set<Usuario>();
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await DbSet
                .Include(u => u.UsuarioRols)
                .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario?> GetByEmailAndPasswordAsync(string email, string password)
        {
            return await DbSet
                .Include(u => u.UsuarioRols)
                .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public async Task<IEnumerable<Usuario>> GetActiveUsersAsync()
        {
            return await DbSet
                .Include(u => u.UsuarioRols)
                .ThenInclude(ur => ur.Rol)
                .Where(u => u.Activo == true)
                .ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> GetByRoleAsync(int roleId)
        {
            return await DbSet
                .Include(u => u.UsuarioRols)
                .ThenInclude(ur => ur.Rol)
                .Where(u => u.UsuarioRols.Any(ur => ur.RolId == roleId))
                .ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await DbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<Usuario?> GetByResetTokenAsync(string token)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.ResetPasswordToken == token);
        }

        public async Task<bool> SaveResetTokenAsync(int userId, string token, DateTime expiration)
        {
            var user = await DbSet.FirstOrDefaultAsync(u => u.UsuarioId == userId);

            if (user == null) return false;

            user.ResetPasswordToken = token;
            user.ResetPasswordExpiration = expiration;

            DbContext.Update(user);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearResetTokenAsync(int userId)
        {
            var user = await DbSet.FirstOrDefaultAsync(u => u.UsuarioId == userId);

            if (user == null) return false;

            user.ResetPasswordToken = null;
            user.ResetPasswordExpiration = null;

            DbContext.Update(user);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Usuario>> GetBuyersAsync()
        {
            return await DbContext.Usuarios
                .Where(u => u.UsuarioRols.Any(r => r.RolId == 4))
                .ToListAsync();
        }

    }
}
