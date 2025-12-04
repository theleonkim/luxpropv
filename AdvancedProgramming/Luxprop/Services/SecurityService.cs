using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Services;

public class SecurityService
{
    private readonly LuxpropContext _db;
    private readonly SessionService _session;

    public SecurityService(LuxpropContext db, SessionService session)
    {
        _db = db;
        _session = session;
    }

    public async Task<(int userId, string role, int? agentId)> GetContextAsync()
    {
        var uid = await _session.GetUserIdAsync();
        var role = await _session.GetUserRoleAsync() ?? "Guest";

        int? agentId = null;
        if (uid > 0 && role.Equals("Agente", StringComparison.OrdinalIgnoreCase))
        {
            agentId = await _db.Agentes
                .Where(a => a.UsuarioId == uid)
                .Select(a => (int?)a.AgenteId)
                .FirstOrDefaultAsync();
        }
        return (uid, role, agentId);
    }

    // -------- Filtros de lectura ----------
    public IQueryable<Cliente> FilterClientes(IQueryable<Cliente> q, string role, int userId, int? agentId)
    {
        if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase)) return q;
        if (role.Equals("Agente", StringComparison.OrdinalIgnoreCase) && agentId.HasValue)
        {
            // Clientes con expedientes/propiedades asignadas al agente
            var clienteIds = _db.Expedientes
                .Where(e => e.Propiedad!.AgenteId == agentId.Value)
                .Select(e => e.ClienteId)
                .Distinct();

            return q.Where(c => clienteIds.Contains(c.ClienteId));
        }
        if (role.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
            return q.Where(c => c.UsuarioId == userId);

        return q.Where(_ => false);
    }

    public IQueryable<Propiedad> FilterPropiedades(IQueryable<Propiedad> q, string role, int userId, int? agentId)
    {
        if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase)) return q;
        if (role.Equals("Agente", StringComparison.OrdinalIgnoreCase) && agentId.HasValue)
            return q.Where(p => p.AgenteId == agentId.Value);
        if (role.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
        {
            // propiedades ligadas a expedientes del usuario
            var propIds = _db.Expedientes.Where(e => e.Cliente!.UsuarioId == userId)
                                         .Select(e => e.PropiedadId);
            return q.Where(p => propIds.Contains(p.PropiedadId));
        }
        return q.Where(_ => false);
    }

    public IQueryable<Expediente> FilterExpedientes(IQueryable<Expediente> q, string role, int userId, int? agentId)
    {
        if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase)) return q;
        if (role.Equals("Agente", StringComparison.OrdinalIgnoreCase) && agentId.HasValue)
            return q.Where(e => e.Propiedad!.AgenteId == agentId.Value);
        if (role.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
            return q.Where(e => e.Cliente!.UsuarioId == userId);

        return q.Where(_ => false);
    }
}
