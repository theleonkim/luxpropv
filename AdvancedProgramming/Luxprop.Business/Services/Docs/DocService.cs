using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Business.Services.Docs;

public sealed class DocService : IDocService
{
    private readonly LuxpropContext _db;

    public DocService(LuxpropContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<DocRowDto>> ListAsync(DocCriteria c, ClaimsPrincipal user)
    {
        var isAdmin = user.IsInRole("admin");

        // Current user id from claims
        var uidClaim = user.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier);
        int usuarioId = 0;
        if (uidClaim != null && !string.IsNullOrEmpty(uidClaim.Value))
            int.TryParse(uidClaim.Value, out usuarioId);

        // If it's an agent, get its AgenteId
        int? agenteIdActual = null;
        if (!isAdmin && usuarioId > 0)
        {
            agenteIdActual = await _db.Agentes
                .Where(a => a.UsuarioId == usuarioId)
                .Select(a => (int?)a.AgenteId)
                .FirstOrDefaultAsync();
        }

        // Base query sobre Documento
        var docs = _db.Documentos
            .AsNoTracking()
            .Include(d => d.Expediente)!.ThenInclude(e => e.Propiedad)
            .AsQueryable();

        // 🔒 Seguridad: agentes solo ven sus propiedades
        if (!isAdmin && agenteIdActual.HasValue)
        {
            docs = docs.Where(d =>
                d.Expediente != null &&
                d.Expediente.Propiedad != null &&
                d.Expediente.Propiedad.AgenteId == agenteIdActual.Value);
        }

        // Filtros
        if (c.ExpedienteId.HasValue)
            docs = docs.Where(d => d.ExpedienteId == c.ExpedienteId);

        if (!string.IsNullOrWhiteSpace(c.Estado))
            docs = docs.Where(d => d.Estado == c.Estado);

        if (c.From.HasValue)
            docs = docs.Where(d => d.FechaCarga.HasValue && d.FechaCarga.Value >= c.From.Value);

        if (c.To.HasValue)
            docs = docs.Where(d => d.FechaCarga.HasValue && d.FechaCarga.Value <= c.To.Value);

        if (!string.IsNullOrWhiteSpace(c.Search))
            docs = docs.Where(d => d.Nombre != null && d.Nombre.Contains(c.Search));

        // Proyección al DTO
        var q = docs.Select(d => new DocRowDto
        {
            Id = d.DocumentoId,
            ExpedienteId = d.ExpedienteId,
            ExpedienteRef = d.Expediente != null
                ? (d.Expediente.PropiedadId.HasValue
                    ? $"Property #{d.Expediente.PropiedadId}"
                    : $"Case #{d.Expediente.ExpedienteId}")
                : (d.ExpedienteId.HasValue ? $"Case #{d.ExpedienteId}" : "-"),
            Nombre = d.Nombre,
            Estado = d.Estado,
            Fecha = d.FechaCarga,      // ← ya es DateOnly?
            UrlArchivo = d.UrlArchivo
        });

        return await q
            .OrderByDescending(x => x.Fecha)
            .ThenBy(x => x.Nombre)
            .ToListAsync();
    }

    public async Task<(byte[] file, string contentType, string downloadName)?> GetFileAsync(
        int documentoId,
        ClaimsPrincipal user)
    {
        var isAdmin = user.IsInRole("admin");

        var uidClaim = user.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier);
        int usuarioId = 0;
        if (uidClaim != null && !string.IsNullOrEmpty(uidClaim.Value))
            int.TryParse(uidClaim.Value, out usuarioId);

        int? agenteIdActual = null;
        if (!isAdmin && usuarioId > 0)
        {
            agenteIdActual = await _db.Agentes
                .Where(a => a.UsuarioId == usuarioId)
                .Select(a => (int?)a.AgenteId)
                .FirstOrDefaultAsync();
        }

        var doc = await _db.Documentos
            .Include(d => d.Expediente)!.ThenInclude(e => e.Propiedad)
            .FirstOrDefaultAsync(d => d.DocumentoId == documentoId);

        if (doc is null)
            return null;

        // 🔒 Per-document security
        if (!isAdmin && agenteIdActual.HasValue)
        {
            var prop = doc.Expediente?.Propiedad;
            if (prop == null || prop.AgenteId != agenteIdActual.Value)
                return null;
        }

        if (!string.IsNullOrWhiteSpace(doc.UrlArchivo))
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                doc.UrlArchivo.TrimStart('/', '\\'));

            if (!File.Exists(path))
                return null;

            var bytes = await File.ReadAllBytesAsync(path);
            var fileName = Path.GetFileName(path);
            var contentType = "application/octet-stream";
            return (bytes, contentType, fileName);
        }

        return null;
    }
}
