using System;

namespace Luxprop.Business.Services.Docs;

public sealed class DocRowDto
{
    public int Id { get; set; }
    public int? ExpedienteId { get; set; }
    public string ExpedienteRef { get; set; } = string.Empty;
    public string? Nombre { get; set; }
    public string? Estado { get; set; }
    public DateOnly? Fecha { get; set; }   // ← DateOnly?
    public string? UrlArchivo { get; set; }
}
