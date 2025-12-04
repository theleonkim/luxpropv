using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface ICitumRepository : IRepositoryBase<Citum>
    {
        Task<IEnumerable<Citum>> GetByExpedienteIdAsync(int expedienteId);
        Task<IEnumerable<Citum>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Citum>> GetByCanalAsync(string canal);
        Task<IEnumerable<Citum>> GetUpcomingCitasAsync(int daysAhead = 30);
        Task<IEnumerable<Citum>> GetPastCitasAsync();
    }
    
    public class CitumRepository : RepositoryBase<Citum>, ICitumRepository
    {
        public CitumRepository()
        {
            DbSet = DbContext.Set<Citum>();
        }

        public async Task<IEnumerable<Citum>> GetByExpedienteIdAsync(int expedienteId)
        {
            return await DbSet
                .Include(c => c.Expediente)
                .Where(c => c.ExpedienteId == expedienteId)
                .OrderBy(c => c.FechaInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Citum>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await DbSet
                .Include(c => c.Expediente)
                .Where(c => c.FechaInicio >= startDate && c.FechaInicio <= endDate)
                .OrderBy(c => c.FechaInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Citum>> GetByCanalAsync(string canal)
        {
            return await DbSet
                .Include(c => c.Expediente)
                .Where(c => c.Canal == canal)
                .OrderBy(c => c.FechaInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Citum>> GetUpcomingCitasAsync(int daysAhead = 30)
        {
            var futureDate = DateTime.Now.AddDays(daysAhead);
            return await DbSet
                .Include(c => c.Expediente)
                .Where(c => c.FechaInicio >= DateTime.Now && c.FechaInicio <= futureDate)
                .OrderBy(c => c.FechaInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Citum>> GetPastCitasAsync()
        {
            return await DbSet
                .Include(c => c.Expediente)
                .Where(c => c.FechaInicio < DateTime.Now)
                .OrderByDescending(c => c.FechaInicio)
                .ToListAsync();
        }
    }
}
