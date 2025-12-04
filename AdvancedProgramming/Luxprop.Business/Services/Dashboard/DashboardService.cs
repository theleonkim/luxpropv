using Microsoft.EntityFrameworkCore;
using Luxprop.Data.Models;

namespace Luxprop.Business.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly LuxpropContext _db;

        public DashboardService(LuxpropContext db)
        {
            _db = db;
        }

        // 🔹 Documentos que vencerán pronto
        public async Task<List<Documento>> GetDocumentsToExpireAsync(int daysAhead = 7)
        {
            var today = DateTime.Today;
            var limit = today.AddDays(daysAhead);

            return await _db.Documentos
                .Where(d =>
                    d.FechaVencimiento != null &&
                    d.FechaVencimiento >= today &&
                    d.FechaVencimiento <= limit)
                .OrderBy(d => d.FechaVencimiento)
                .ToListAsync();
        }

        // 🔹 Recordatorios próximos
        public async Task<List<Recordatorio>> GetUpcomingRemindersAsync(int daysAhead = 7, int? userId = null)
        {
            var now = DateTime.Now; // 👈 importante
            var limit = now.AddDays(daysAhead);

            return await _db.Recordatorios
                .Where(r =>
                    r.Inicio >= now &&
                    r.Inicio <= limit &&
                    (userId == null || r.UsuarioId == userId))
                .OrderBy(r => r.Inicio)
                .ToListAsync();
        }
    }
}
