using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface ITareaTramiteRepository : IRepositoryBase<TareaTramite>
    {
        Task<IEnumerable<TareaTramite>> GetByExpedienteIdAsync(int expedienteId);
        Task<IEnumerable<TareaTramite>> GetByEstadoAsync(string estado);
        Task<IEnumerable<TareaTramite>> GetByPrioridadAsync(string prioridad);
        Task<IEnumerable<TareaTramite>> GetOverdueTasksAsync();
        Task<IEnumerable<TareaTramite>> GetUpcomingTasksAsync(int daysAhead = 7);
        Task<IEnumerable<TareaTramite>> GetCompletedTasksAsync();
    }
    
    public class TareaTramiteRepository : RepositoryBase<TareaTramite>, ITareaTramiteRepository
    {
        public TareaTramiteRepository()
        {
            DbSet = DbContext.Set<TareaTramite>();
        }

        public async Task<IEnumerable<TareaTramite>> GetByExpedienteIdAsync(int expedienteId)
        {
            return await DbSet
                .Include(t => t.Expediente)
                .Where(t => t.ExpedienteId == expedienteId)
                .OrderBy(t => t.FechaInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<TareaTramite>> GetByEstadoAsync(string estado)
        {
            return await DbSet
                .Include(t => t.Expediente)
                .Where(t => t.Estado == estado)
                .OrderBy(t => t.FechaInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<TareaTramite>> GetByPrioridadAsync(string prioridad)
        {
            return await DbSet
                .Include(t => t.Expediente)
                .Where(t => t.Prioridad == prioridad)
                .OrderBy(t => t.FechaInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<TareaTramite>> GetOverdueTasksAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await DbSet
                .Include(t => t.Expediente)
                .Where(t => t.FechaCompromiso < today && t.Estado != "Completed")
                .OrderBy(t => t.FechaCompromiso)
                .ToListAsync();
        }

        public async Task<IEnumerable<TareaTramite>> GetUpcomingTasksAsync(int daysAhead = 7)
        {
            var futureDate = DateOnly.FromDateTime(DateTime.Now.AddDays(daysAhead));
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await DbSet
                .Include(t => t.Expediente)
                .Where(t => t.FechaCompromiso >= today && t.FechaCompromiso <= futureDate && t.Estado != "Completed")
                .OrderBy(t => t.FechaCompromiso)
                .ToListAsync();
        }

        public async Task<IEnumerable<TareaTramite>> GetCompletedTasksAsync()
        {
            return await DbSet
                .Include(t => t.Expediente)
                .Where(t => t.Estado == "Completed")
                .OrderByDescending(t => t.FechaCierre)
                .ToListAsync();
        }
    }
}
