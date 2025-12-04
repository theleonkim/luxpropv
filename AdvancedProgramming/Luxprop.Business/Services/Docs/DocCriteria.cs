using System;

namespace Luxprop.Business.Services.Docs;

public sealed class DocCriteria
{
    public int? ExpedienteId { get; set; }
    public string? Estado { get; set; }
    public string? Search { get; set; }
    public DateOnly? From { get; set; }   // ← DateOnly?
    public DateOnly? To { get; set; }     // ← DateOnly?
}
