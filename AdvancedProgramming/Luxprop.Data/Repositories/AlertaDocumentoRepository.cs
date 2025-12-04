using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IAlertasDocumentoRepository : IRepositoryBase<AlertasDocumento>
    {
        Task<IEnumerable<AlertasDocumento>> GetByDocumentoIdAsync(int documentoId);
        Task<IEnumerable<AlertasDocumento>> GetByTipoAsync(string tipo);
        Task<IEnumerable<AlertasDocumento>> GetByEstadoAsync(string estado);
        Task<IEnumerable<AlertasDocumento>> GetByDateRangeAsync(DateOnly start, DateOnly end);
        Task<bool> ExisteAlertaParaHoyAsync(int documentoId, string tipo);
    }

    public class AlertasDocumentoRepository
        : RepositoryBase<AlertasDocumento>, IAlertasDocumentoRepository
    {
        public AlertasDocumentoRepository()
        {
            DbSet = DbContext.Set<AlertasDocumento>();
        }

        // -------------------------------------------------------------
        // 🔹 OBTENER ALERTAS POR DOCUMENTO
        // -------------------------------------------------------------
        public async Task<IEnumerable<AlertasDocumento>> GetByDocumentoIdAsync(int documentoId)
        {
            return await DbSet
                .Include(a => a.Documento)
                .Where(a => a.DocumentoId == documentoId)
                .OrderByDescending(a => a.FechaRegistro)
                .ToListAsync();
        }

        // -------------------------------------------------------------
        // 🔹 OBTENER ALERTAS POR TIPO (Vencido / Por vencer)
        // -------------------------------------------------------------
        public async Task<IEnumerable<AlertasDocumento>> GetByTipoAsync(string tipo)
        {
            return await DbSet
                .Include(a => a.Documento)
                .Where(a => a.Tipo == tipo)
                .OrderByDescending(a => a.FechaRegistro)
                .ToListAsync();
        }

        // -------------------------------------------------------------
        // 🔹 OBTENER ALERTAS POR ESTADO (Pendiente / Enviado / etc.)
        // -------------------------------------------------------------
        public async Task<IEnumerable<AlertasDocumento>> GetByEstadoAsync(string estado)
        {
            return await DbSet
                .Include(a => a.Documento)
                .Where(a => a.Estado == estado)
                .OrderByDescending(a => a.FechaRegistro)
                .ToListAsync();
        }

        // -------------------------------------------------------------
        // 🔹 RANGO DE FECHAS
        // -------------------------------------------------------------
        public async Task<IEnumerable<AlertasDocumento>> GetByDateRangeAsync(DateOnly start, DateOnly end)
        {
            return await DbSet
                .Include(a => a.Documento)
                .Where(a => a.FechaRegistro >= start && a.FechaRegistro <= end)
                .OrderBy(a => a.FechaRegistro)
                .ToListAsync();
        }

        // -------------------------------------------------------------
        // 🔹 VERIFICAR SI YA SE CREÓ UNA ALERTA HOY
        //    (para no repetir correos el mismo día)
        // -------------------------------------------------------------
        public async Task<bool> ExisteAlertaParaHoyAsync(int documentoId, string tipo)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            return await DbSet
                .AnyAsync(a =>
                    a.DocumentoId == documentoId &&
                    a.Tipo == tipo &&
                    a.FechaRegistro == today);
        }
    }
}
