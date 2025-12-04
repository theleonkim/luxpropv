using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Luxprop.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Luxprop.Controllers
{
    [Route("[controller]")]
    public class ReportsController : Controller
    {
        private readonly LuxpropContext _db;

        public ReportsController(LuxpropContext db)
        {
            _db = db;
        }

        // GET /Reports/CaseRecordsPdf?... (mismos filtros que Records.razor)
        [HttpGet("CaseRecordsPdf")]
        public async Task<IActionResult> CaseRecordsPdf(
            [FromQuery] int? expedienteId,
            [FromQuery] string? estado,
            [FromQuery] string? search,
            [FromQuery] string? from,
            [FromQuery] string? to)
        {
            // Parseo de fechas desde querystring
            DateOnly? fromDate = ParseDateOnly(from);
            DateOnly? toDate = ParseDateOnly(to);

            // MISMA QUERY que usas en Records.razor
            var q = _db.Expedientes
                .Include(e => e.Propiedad)
                .Include(e => e.Cliente).ThenInclude(c => c.Usuario)
                .Include(e => e.Documentos)
                .Include(e => e.TareaTramites)
                .AsQueryable();

            if (expedienteId.HasValue)
                q = q.Where(e => e.ExpedienteId == expedienteId.Value);

            if (!string.IsNullOrWhiteSpace(estado))
                q = q.Where(e => e.Estado == estado);

            if (fromDate.HasValue)
                q = q.Where(e => e.FechaApertura.HasValue && e.FechaApertura.Value >= fromDate.Value);

            if (toDate.HasValue)
                q = q.Where(e => e.FechaApertura.HasValue && e.FechaApertura.Value <= toDate.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.ToLower();
                q = q.Where(e =>
                    (e.Propiedad != null && e.Propiedad.Titulo != null &&
                     e.Propiedad.Titulo.ToLower().Contains(term)) ||
                    (e.Cliente != null && e.Cliente.Usuario != null &&
                     (e.Cliente.Usuario.Nombre + " " + e.Cliente.Usuario.Apellido)
                        .ToLower().Contains(term)));
            }

            var list = await q
                .OrderByDescending(e => e.FechaApertura)
                .ThenBy(e => e.ExpedienteId)
                .ToListAsync();

            var rows = list.Select(e =>
            {
                var totalTasks = e.TareaTramites?.Count ?? 0;
                var completedTasks = e.TareaTramites?.Count(t =>
                    t.Estado == "Completed" ||
                    t.Estado == "Completado" ||
                    t.Estado == "Finalizado" ||
                    t.Estado == "Cerrado") ?? 0;

                double progress = 0;
                if (totalTasks > 0)
                    progress = (double)completedTasks / totalTasks * 100.0;

                return new CaseRecordRow
                {
                    Id = e.ExpedienteId,
                    Property = e.Propiedad?.Titulo ?? "-",
                    Client = e.Cliente?.Usuario != null
                        ? $"{e.Cliente.Usuario.Nombre} {e.Cliente.Usuario.Apellido}"
                        : "No client",
                    Status = e.Estado ?? "-",
                    Opened = e.FechaApertura,
                    Closed = e.FechaCierre,
                    DocumentCount = e.Documentos?.Count ?? 0,
                    TotalTasks = totalTasks,
                    CompletedTasks = completedTasks,
                    ProgressPercent = progress
                };
            }).ToList();

            // Generar PDF con QuestPDF
            var document = new CaseRecordsPdfDocument(rows);
            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", "case-records.pdf");
        }

        private static DateOnly? ParseDateOnly(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var date))
            {
                return date;
            }

            return null;
        }
    }

    // DTO simple para el PDF
    public class CaseRecordRow
    {
        public int Id { get; set; }
        public string Property { get; set; } = string.Empty;
        public string Client { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateOnly? Opened { get; set; }
        public DateOnly? Closed { get; set; }
        public int DocumentCount { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public double ProgressPercent { get; set; }
    }

    // Documento PDF con QuestPDF
    public class CaseRecordsPdfDocument : IDocument
    {
        private readonly IList<CaseRecordRow> _rows;

        public CaseRecordsPdfDocument(IList<CaseRecordRow> rows)
        {
            _rows = rows;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A4.Landscape());
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Text("Case Records Report")
                    .SemiBold().FontSize(16).FontColor(Colors.Blue.Darken2);

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.ConstantColumn(40);  // ID
                        cols.RelativeColumn(2);   // Property
                        cols.RelativeColumn(2);   // Client
                        cols.RelativeColumn(1);   // Status
                        cols.RelativeColumn(1);   // Opened
                        cols.RelativeColumn(1);   // Closed
                        cols.ConstantColumn(40);  // Docs
                        cols.ConstantColumn(60);  // Tasks
                        cols.ConstantColumn(60);  // Progress
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderCell).Text("ID");
                        header.Cell().Element(HeaderCell).Text("Property");
                        header.Cell().Element(HeaderCell).Text("Client");
                        header.Cell().Element(HeaderCell).Text("Status");
                        header.Cell().Element(HeaderCell).Text("Opened");
                        header.Cell().Element(HeaderCell).Text("Closed");
                        header.Cell().Element(HeaderCell).Text("Docs");
                        header.Cell().Element(HeaderCell).Text("Tasks");
                        header.Cell().Element(HeaderCell).Text("Progress");
                    });

                    // Rows
                    foreach (var r in _rows)
                    {
                        table.Cell().Element(BodyCell).Text(r.Id.ToString());
                        table.Cell().Element(BodyCell).Text(r.Property);
                        table.Cell().Element(BodyCell).Text(r.Client);
                        table.Cell().Element(BodyCell).Text(r.Status);
                        table.Cell().Element(BodyCell).Text(r.Opened?.ToString("yyyy-MM-dd") ?? "");
                        table.Cell().Element(BodyCell).Text(r.Closed?.ToString("yyyy-MM-dd") ?? "");
                        table.Cell().Element(BodyCell).Text(r.DocumentCount.ToString());
                        table.Cell().Element(BodyCell).Text($"{r.CompletedTasks}/{r.TotalTasks}");
                        table.Cell().Element(BodyCell).Text($"{r.ProgressPercent:0}%");
                    }
                });
            });
        }

        private static IContainer HeaderCell(IContainer container) =>
            container.DefaultTextStyle(x => x.SemiBold())
                     .PaddingVertical(5)
                     .PaddingHorizontal(3)
                     .BorderBottom(1)
                     .BorderColor(Colors.Grey.Lighten2);

        private static IContainer BodyCell(IContainer container) =>
            container.PaddingVertical(2).PaddingHorizontal(3);
    }
}