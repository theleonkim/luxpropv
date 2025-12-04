using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IDocumentoRepository : IRepositoryBase<Documento>
    {
        Task<IEnumerable<Documento>> GetByExpedienteIdAsync(int expedienteId);
        Task<IEnumerable<Documento>> GetByTipoDocumentoAsync(string tipoDocumento);
        Task<IEnumerable<Documento>> GetByEstadoAsync(string estado);
        Task<IEnumerable<Documento>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<Documento>> GetDocumentsWithAlertsAsync();

        Task<IEnumerable<Documento>> GetDocumentsNearExpirationAsync(int daysBefore = 5);
        Task<IEnumerable<Documento>> GetExpiredDocumentsAsync();
    }

    public class DocumentoRepository : RepositoryBase<Documento>, IDocumentoRepository
    {
        public DocumentoRepository()
        {
            DbSet = DbContext.Set<Documento>();
        }

        public async Task<IEnumerable<Documento>> GetByExpedienteIdAsync(int expedienteId)
        {
            return await DbSet
                .Include(d => d.Expediente)
                .Include(d => d.AlertasDocumentos)
                .Where(d => d.ExpedienteId == expedienteId)
                .OrderBy(d => d.FechaCarga)
                .ToListAsync();
        }

        public async Task<IEnumerable<Documento>> GetByTipoDocumentoAsync(string tipoDocumento)
        {
            return await DbSet
                .Include(d => d.Expediente)
                .Include(d => d.AlertasDocumentos)
                .Where(d => d.TipoDocumento == tipoDocumento)
                .OrderBy(d => d.FechaCarga)
                .ToListAsync();
        }

        public async Task<IEnumerable<Documento>> GetByEstadoAsync(string estado)
        {
            return await DbSet
                .Include(d => d.Expediente)
                .Include(d => d.AlertasDocumentos)
                .Where(d => d.Estado == estado)
                .OrderBy(d => d.FechaCarga)
                .ToListAsync();
        }

        public async Task<IEnumerable<Documento>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            return await DbSet
                .Include(d => d.Expediente)
                .Include(d => d.AlertasDocumentos)
                .Where(d =>
                    d.FechaCarga >= startDate &&
                    d.FechaCarga <= endDate
                )
                .OrderBy(d => d.FechaCarga)
                .ToListAsync();
        }

        public async Task<IEnumerable<Documento>> GetDocumentsWithAlertsAsync()
        {
            return await DbSet
                .Include(d => d.Expediente)
                .Include(d => d.AlertasDocumentos)
                .Where(d => d.AlertasDocumentos.Any())
                .OrderBy(d => d.FechaCarga)
                .ToListAsync();
        }

        // Documentos por vencer
        public async Task<IEnumerable<Documento>> GetDocumentsNearExpirationAsync(int daysBefore = 5)
        {
            var today = DateTime.Today;
            var limitDate = today.AddDays(daysBefore);

            return await DbSet
                .Include(d => d.Expediente)
                .Include(d => d.AlertasDocumentos)
                .Where(d =>
                    d.FechaVencimiento != null &&
                    d.FechaVencimiento >= today &&
                    d.FechaVencimiento <= limitDate
                )
                .OrderBy(d => d.FechaVencimiento)
                .ToListAsync();
        }

        // Documentos vencidos
        public async Task<IEnumerable<Documento>> GetExpiredDocumentsAsync()
        {
            var today = DateTime.Today;

            return await DbSet
                .Include(d => d.Expediente)
                .Include(d => d.AlertasDocumentos)
                .Where(d =>
                    d.FechaVencimiento != null &&
                    d.FechaVencimiento < today
                )
                .OrderBy(d => d.FechaVencimiento)
                .ToListAsync();
        }
    }
}
