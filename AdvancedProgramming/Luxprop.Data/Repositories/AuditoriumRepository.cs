using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IAuditoriumRepository : IRepositoryBase<Auditorium>
    {
        Task<IEnumerable<Auditorium>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Auditorium>> GetByAccionAsync(string accion);
        Task<IEnumerable<Auditorium>> GetByEntidadAsync(string entidad);
        Task<IEnumerable<Auditorium>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Auditorium>> GetRecentAuditsAsync(int count = 50);
    }
    
    public class AuditoriumRepository : RepositoryBase<Auditorium>, IAuditoriumRepository
    {
        public AuditoriumRepository()
        {
            DbSet = DbContext.Set<Auditorium>();
        }

        public async Task<IEnumerable<Auditorium>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await DbSet
                .Include(a => a.Usuario)
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auditorium>> GetByAccionAsync(string accion)
        {
            return await DbSet
                .Include(a => a.Usuario)
                .Where(a => a.Accion == accion)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auditorium>> GetByEntidadAsync(string entidad)
        {
            return await DbSet
                .Include(a => a.Usuario)
                .Where(a => a.Entidad == entidad)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auditorium>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await DbSet
                .Include(a => a.Usuario)
                .Where(a => a.Fecha >= startDate && a.Fecha <= endDate)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auditorium>> GetRecentAuditsAsync(int count = 50)
        {
            return await DbSet
                .Include(a => a.Usuario)
                .OrderByDescending(a => a.Fecha)
                .Take(count)
                .ToListAsync();
        }
    }
}
