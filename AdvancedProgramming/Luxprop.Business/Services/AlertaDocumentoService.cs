using Luxprop.Data.Models;
using Luxprop.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Business.Services
{
    public interface IAlertasDocumentoService
    {
        Task<bool> CrearAlertaAsync(int documentoId, string tipo, string estado);
        Task<IEnumerable<AlertasDocumento>> GetAlertasPorDocumentoAsync(int documentoId);
        Task<IEnumerable<AlertasDocumento>> GetAlertasPorTipoAsync(string tipo);
        Task<IEnumerable<AlertasDocumento>> GetAlertasPorEstadoAsync(string estado);
        Task<bool> YaExisteAlertaHoyAsync(int documentoId, string tipo);
    }

    public class AlertasDocumentoService : IAlertasDocumentoService
    {
        private readonly LuxpropContext _db;
        private readonly IAlertasDocumentoRepository _repo;

        public AlertasDocumentoService(LuxpropContext db, IAlertasDocumentoRepository repo)
        {
            _db = db;
            _repo = repo;
        }

        // -------------------------------------------------------------
        // 🔹 CREA UNA ALERTA (SOLO SI NO SE HA CREADO HOY)
        // -------------------------------------------------------------
        public async Task<bool> CrearAlertaAsync(int documentoId, string tipo, string estado)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            // Verifica si ya existe
            if (await _repo.ExisteAlertaParaHoyAsync(documentoId, tipo))
                return false;

            var alerta = new AlertasDocumento
            {
                DocumentoId = documentoId,
                Tipo = tipo,                     // "Vencido" o "Por vencer"
                Estado = estado,                 // "Pendiente", "Enviado", etc.
                FechaRegistro = hoy
            };

            await _db.AlertasDocumentos.AddAsync(alerta);
            await _db.SaveChangesAsync();
            return true;
        }

        // -------------------------------------------------------------
        // 🔹 CONSULTAS
        // -------------------------------------------------------------
        public async Task<IEnumerable<AlertasDocumento>> GetAlertasPorDocumentoAsync(int documentoId)
        {
            return await _repo.GetByDocumentoIdAsync(documentoId);
        }

        public async Task<IEnumerable<AlertasDocumento>> GetAlertasPorTipoAsync(string tipo)
        {
            return await _repo.GetByTipoAsync(tipo);
        }

        public async Task<IEnumerable<AlertasDocumento>> GetAlertasPorEstadoAsync(string estado)
        {
            return await _repo.GetByEstadoAsync(estado);
        }

        public async Task<bool> YaExisteAlertaHoyAsync(int documentoId, string tipo)
        {
            return await _repo.ExisteAlertaParaHoyAsync(documentoId, tipo);
        }
    }
}
