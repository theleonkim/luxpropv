using Luxprop.Data.Models;
using Luxprop.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Business.Services
{
    public interface IHistorialExpedienteService
    {
        Task<IEnumerable<HistorialExpediente>> GetAllAsync();
        Task<HistorialExpediente?> GetByIdAsync(int id);
        Task<IEnumerable<HistorialExpediente>> GetByExpedienteIdAsync(int expedienteId);
        Task<IEnumerable<HistorialExpediente>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<HistorialExpediente>> GetByTipoAccionAsync(string tipoAccion);
        Task<IEnumerable<HistorialExpediente>> GetRecentAsync(int cantidad = 10);
        Task CreateAsync(HistorialExpediente historial);
    }

    public class HistorialExpedienteService : IHistorialExpedienteService
    {
        private readonly IHistorialExpedienteRepository _historialRepo;
        private readonly LuxpropContext _db;

        public HistorialExpedienteService(IHistorialExpedienteRepository historialRepo, LuxpropContext db)
        {
            _historialRepo = historialRepo;
            _db = db;
        }

        // ✅ Obtiene todo el historial con sus relaciones
        public async Task<IEnumerable<HistorialExpediente>> GetAllAsync()
        {
            return await _db.HistorialExpedientes
                .Include(h => h.Expediente)
                .Include(h => h.Usuario)
                .OrderByDescending(h => h.FechaModificacion)
                .ToListAsync();
        }

        // ✅ Obtiene un registro por ID
        public async Task<HistorialExpediente?> GetByIdAsync(int id)
        {
            return await _db.HistorialExpedientes
                .Include(h => h.Expediente)
                .Include(h => h.Usuario)
                .FirstOrDefaultAsync(h => h.HistorialId == id);
        }

        // ✅ Obtiene todos los movimientos de un expediente
        public async Task<IEnumerable<HistorialExpediente>> GetByExpedienteIdAsync(int expedienteId)
        {
            return await _historialRepo.GetByExpedienteIdAsync(expedienteId);
        }

        // ✅ Obtiene los registros hechos por un usuario específico
        public async Task<IEnumerable<HistorialExpediente>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _historialRepo.GetByUsuarioIdAsync(usuarioId);
        }

        // ✅ Filtra por tipo de acción (Creación, Actualización, Cierre...)
        public async Task<IEnumerable<HistorialExpediente>> GetByTipoAccionAsync(string tipoAccion)
        {
            return await _historialRepo.GetByTipoAccionAsync(tipoAccion);
        }

        // ✅ Obtiene los registros más recientes (por defecto los últimos 10)
        public async Task<IEnumerable<HistorialExpediente>> GetRecentAsync(int cantidad = 10)
        {
            return await _historialRepo.GetRecentAsync(cantidad);
        }

        // ✅ Crea un registro de historial de expediente
        public async Task CreateAsync(HistorialExpediente historial)
        {
            historial.FechaModificacion = DateTime.Now;
            await _historialRepo.CreateAsync(historial);
        }
    }
}
