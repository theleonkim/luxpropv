using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IHistorialExpedienteRepository : IRepositoryBase<HistorialExpediente>
    {
        Task<IEnumerable<HistorialExpediente>> GetByExpedienteIdAsync(int expedienteId);
        Task<IEnumerable<HistorialExpediente>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<HistorialExpediente>> GetByFechaAsync(DateTime fecha);
        Task<IEnumerable<HistorialExpediente>> GetByAccionAsync(string tipoAccion);
        Task<IEnumerable<HistorialExpediente>> GetByEstadoNuevoAsync(string estadoNuevo);
        Task<IEnumerable<HistorialExpediente>> GetByIPAsync(string ip);
        Task<IEnumerable<HistorialExpediente>> GetByTipoAccionAsync(string tipoAccion);
        Task<IEnumerable<HistorialExpediente>> GetRecentAsync(int cantidad = 10);
        Task CrearHistorialAsync(int expedienteId, string estadoNuevo, string descripcion, int? usuarioId = null);
    }

    public class HistorialExpedienteRepository : RepositoryBase<HistorialExpediente>, IHistorialExpedienteRepository
    {
        private readonly LuxpropContext _context;
        private readonly DbSet<HistorialExpediente> _dbSet;

        public HistorialExpedienteRepository(LuxpropContext context)
        {
            _context = context;
            _dbSet = _context.Set<HistorialExpediente>();
        }

        private IQueryable<HistorialExpediente> IncludeAll()
        {
            return DbSet
                .Include(h => h.Expediente)
                .ThenInclude(e => e.Propiedad)
                .Include(h => h.Usuario);
        }

        public async Task<IEnumerable<HistorialExpediente>> GetByExpedienteIdAsync(int expedienteId)
        {
            return await IncludeAll()
                .Where(h => h.ExpedienteId == expedienteId)
                .OrderByDescending(h => h.FechaModificacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<HistorialExpediente>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await IncludeAll()
                .Where(h => h.UsuarioId == usuarioId)
                .OrderByDescending(h => h.FechaModificacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<HistorialExpediente>> GetByFechaAsync(DateTime fecha)
        {
            var fechaInicio = fecha.Date;
            var fechaFin = fechaInicio.AddDays(1);

            return await IncludeAll()
                .Where(h => h.FechaModificacion >= fechaInicio && h.FechaModificacion < fechaFin)
                .OrderByDescending(h => h.FechaModificacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<HistorialExpediente>> GetByAccionAsync(string tipoAccion)
        {
            return await IncludeAll()
                .Where(h => h.TipoAccion == tipoAccion)
                .OrderByDescending(h => h.FechaModificacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<HistorialExpediente>> GetByEstadoNuevoAsync(string estadoNuevo)
        {
            return await IncludeAll()
                .Where(h => h.EstadoNuevo == estadoNuevo)
                .OrderByDescending(h => h.FechaModificacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<HistorialExpediente>> GetByIPAsync(string ip)
        {
            return await IncludeAll()
                .Where(h => h.IPRegistro != null && h.IPRegistro == ip)
                .OrderByDescending(h => h.FechaModificacion)
                .ToListAsync();
        }
        public async Task<IEnumerable<HistorialExpediente>> GetByTipoAccionAsync(string tipoAccion)
        {
            return await GetByAccionAsync(tipoAccion);
        }

        public async Task<IEnumerable<HistorialExpediente>> GetRecentAsync(int cantidad = 10)
        {
            return await IncludeAll()
                .OrderByDescending(h => h.FechaModificacion)
                .Take(cantidad)
                .ToListAsync();
        }

        public async Task CrearHistorialAsync(int expedienteId, string estadoNuevo, string descripcion, int? usuarioId = null)
        {
            var historial = new HistorialExpediente
            {
                ExpedienteId = expedienteId,
                UsuarioId = usuarioId,
                EstadoNuevo = estadoNuevo,
                Descripcion = descripcion,
                FechaModificacion = DateTime.Now
            };

            await DbSet.AddAsync(historial);
            await _context.SaveChangesAsync();
        }
    }
}
