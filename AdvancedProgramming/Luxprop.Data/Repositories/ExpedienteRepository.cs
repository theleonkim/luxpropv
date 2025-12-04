using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IExpedienteRepository : IRepositoryBase<Expediente>
    {
        Task<IEnumerable<Expediente>> GetByPropiedadIdAsync(int propiedadId);
        Task<IEnumerable<Expediente>> GetByClienteIdAsync(int clienteId);
        Task<IEnumerable<Expediente>> GetByEstadoAsync(string estado);
        Task<IEnumerable<Expediente>> GetByTipoOcupacionAsync(string tipoOcupacion);
        Task<IEnumerable<Expediente>> GetOpenExpedientesAsync();
        Task<IEnumerable<Expediente>> GetExpedientesWithDocumentsAsync();
        Task<IEnumerable<Expediente>> GetByAgenteIdAsync(int agenteId);
        Task<IEnumerable<Expediente>> GetByPrioridadAsync(string prioridad);
        Task<IEnumerable<Expediente>> GetByCategoriaAsync(string categoria);
        Task<Expediente?> GetByCodigoAsync(string codigo);
    }

    public class ExpedienteRepository : RepositoryBase<Expediente>, IExpedienteRepository
    {
        public ExpedienteRepository()
        {
            DbSet = DbContext.Set<Expediente>();
        }

        private IQueryable<Expediente> IncludeAll()
        {
            return DbSet
                .Include(e => e.Propiedad)

                // *** Cliente -> Usuario (importante) ***
                .Include(e => e.Cliente)
                    .ThenInclude(c => c.Usuario)

                // Agente (que también es Usuario)
                .Include(e => e.Agente)

                // Auditoría
                .Include(e => e.CreadoPor)
                .Include(e => e.ModificadoPor)

                // Otros
                .Include(e => e.Documentos)
                .Include(e => e.Cita)
                .Include(e => e.TareaTramites)
                .Include(e => e.HistorialExpedientes);
        }

        public async Task<IEnumerable<Expediente>> GetByPropiedadIdAsync(int propiedadId)
        {
            return await IncludeAll()
                .Where(e => e.PropiedadId == propiedadId)
                .OrderByDescending(e => e.FechaApertura)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expediente>> GetByClienteIdAsync(int clienteId)
        {
            return await IncludeAll()
                .Where(e => e.ClienteId == clienteId)
                .OrderByDescending(e => e.FechaApertura)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expediente>> GetByEstadoAsync(string estado)
        {
            return await IncludeAll()
                .Where(e => e.Estado == estado)
                .OrderByDescending(e => e.FechaApertura)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expediente>> GetByTipoOcupacionAsync(string tipoOcupacion)
        {
            return await IncludeAll()
                .Where(e => e.TipoOcupacion == tipoOcupacion)
                .OrderByDescending(e => e.FechaApertura)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expediente>> GetOpenExpedientesAsync()
        {
            return await IncludeAll()
                .Where(e => e.Estado != "Cerrado" && e.FechaCierre == null)
                .OrderByDescending(e => e.FechaApertura)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expediente>> GetExpedientesWithDocumentsAsync()
        {
            return await IncludeAll()
                .Where(e => e.Documentos.Any())
                .OrderByDescending(e => e.FechaApertura)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expediente>> GetByAgenteIdAsync(int agenteId)
        {
            return await IncludeAll()
                .Where(e => e.AgenteId == agenteId)
                .OrderByDescending(e => e.FechaApertura)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expediente>> GetByPrioridadAsync(string prioridad)
        {
            return await IncludeAll()
                .Where(e => e.Prioridad == prioridad)
                .OrderByDescending(e => e.FechaApertura)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expediente>> GetByCategoriaAsync(string categoria)
        {
            return await IncludeAll()
                .Where(e => e.Categoria == categoria)
                .OrderByDescending(e => e.FechaApertura)
                .ToListAsync();
        }

        public async Task<Expediente?> GetByCodigoAsync(string codigo)
        {
            return await IncludeAll()
                .FirstOrDefaultAsync(e => e.Codigo == codigo);
        }
    }
}
