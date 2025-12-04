using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


public class ReminderService(LuxpropContext db, IEmailService emailSvc, IConfiguration cfg, IHttpContextAccessor httpContextAccessor) : IReminderService
{
    private readonly LuxpropContext _db = db;
    private readonly IEmailService _emailSvc = emailSvc;
    private readonly IConfiguration _cfg = cfg;
    private readonly IHttpContextAccessor _http = httpContextAccessor;

    public async Task<Recordatorio> CreateAsync(Recordatorio r, CancellationToken ct = default)
    {
        r.CreadoEn = DateTime.UtcNow;
        r.ActualizadoEn = DateTime.UtcNow;
        _db.Recordatorios.Add(r);
        await _db.SaveChangesAsync(ct);
        return r;
    }

    public Task<Recordatorio?> GetAsync(int id, CancellationToken ct = default) =>
        _db.Recordatorios
           .Include(x => x.Propiedad)
           .Include(x => x.Expediente)
           .FirstOrDefaultAsync(x => x.RecordatorioId == id, ct);

    public Task<List<Recordatorio>> ListAsync(int? usuarioId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        var q = _db.Recordatorios.AsQueryable();
        if (usuarioId is not null) q = q.Where(x => x.UsuarioId == usuarioId);
        if (from is not null) q = q.Where(x => x.Inicio >= from);
        if (to is not null) q = q.Where(x => x.Inicio <= to);
        return q.OrderBy(x => x.Inicio).ToListAsync(ct);
    }

    public async Task<bool> UpdateAsync(Recordatorio r, CancellationToken ct = default)
    {
        _db.Recordatorios.Update(r);
        r.ActualizadoEn = DateTime.UtcNow;
        return await _db.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var dbR = await _db.Recordatorios.FindAsync([id], ct);
        if (dbR is null) return false;
        _db.Recordatorios.Remove(dbR);
        return await _db.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> SetEstadoAsync(int id, string nuevoEstado, CancellationToken ct = default)
    {
        var r = await _db.Recordatorios.FindAsync([id], ct);
        if (r is null) return false;
        r.Estado = nuevoEstado;
        r.ActualizadoEn = DateTime.UtcNow;
        return await _db.SaveChangesAsync(ct) > 0;
    }


    // 🔥 NUEVO: CREAR un recordatorio
    public async Task AddAsync(Recordatorio recordatorio)
    {
        // Si no hay UsuarioId, tratar de obtenerlo desde el contexto HTTP
        if (recordatorio.UsuarioId == 0)
        {
            var email = _http.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value
                        ?? _http.HttpContext?.User?.FindFirst("email")?.Value
                        ?? _http.HttpContext?.User?.Identity?.Name;

            if (!string.IsNullOrWhiteSpace(email))
            {
                var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
                if (usuario != null)
                    recordatorio.UsuarioId = usuario.UsuarioId;
            }
        }

        if (string.IsNullOrWhiteSpace(recordatorio.Estado))
            recordatorio.Estado = "Pendiente";

        _db.Recordatorios.Add(recordatorio);
        await _db.SaveChangesAsync();

        await SendNotificationEmail(recordatorio);
    }

    // 🔥 NUEVO: OBTENER uno por ID (para la vista de edición)
    public async Task<Recordatorio?> GetByIdAsync(int id)
    {
        return await _db.Recordatorios
            .FirstOrDefaultAsync(r => r.RecordatorioId == id);
    }

    // 🔥 NUEVO: ACTUALIZAR un recordatorio existente
    public async Task UpdateAsync(Recordatorio recordatorio)
    {
        // buscamos el registro real en DB
        var existing = await _db.Recordatorios
            .FirstOrDefaultAsync(r => r.RecordatorioId == recordatorio.RecordatorioId);

        if (existing == null)
            return;

        // campos editables
        existing.Titulo = recordatorio.Titulo;
        existing.Descripcion = recordatorio.Descripcion;
        existing.Tipo = recordatorio.Tipo;
        existing.Inicio = recordatorio.Inicio; // asegúrate que ya viene con fecha+hora combinadas
        existing.Estado = recordatorio.Estado;
        existing.PropiedadId = recordatorio.PropiedadId;

        await _db.SaveChangesAsync();
    }

    private async Task SendNotificationEmail(Recordatorio recordatorio)
    {
        try
        {
            var usuario = await _db.Usuarios.FindAsync(recordatorio.UsuarioId);
            if (usuario == null || string.IsNullOrEmpty(usuario.Email))
                return;

            var subject = $"📅 Nuevo recordatorio creado: {recordatorio.Titulo}";
            var body = $@"
                <h2>Nuevo Recordatorio</h2>
                <p><strong>Título:</strong> {recordatorio.Titulo}</p>
                <p><strong>Descripción:</strong> {recordatorio.Descripcion}</p>
                <p><strong>Tipo:</strong> {recordatorio.Tipo}</p>
                <p><strong>Fecha:</strong> {recordatorio.Inicio:yyyy-MM-dd HH:mm}</p>
                <hr>
                <p>Este recordatorio fue registrado correctamente en el sistema <b>Luxprop</b>.</p>
            ";

            await _emailSvc.SendAsync(usuario.Email, subject, body);

            // (Opcional) enviar copia a administradores si aplica
            var admins = _cfg["Smtp:Admins"]?.Split(';', StringSplitOptions.RemoveEmptyEntries);
            if (admins != null)
            {
                foreach (var admin in admins)
                    await _emailSvc.SendAsync(admin, subject, body);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error enviando correo: {ex.Message}");
        }
    }
}
