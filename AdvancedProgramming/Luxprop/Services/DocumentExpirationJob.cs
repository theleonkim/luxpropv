using Luxprop.Business.Services;
using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

public class DocumentExpirationJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public DocumentExpirationJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // ⏰ Primera ejecución HOY a las 20:50 (8:50 pm)
        await EsperarHastaHoraAsync(20, 50, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<LuxpropContext>();
                var email = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var alertaService = scope.ServiceProvider.GetRequiredService<IAlertasDocumentoService>();

                var hoy = DateTime.Today;

                // -----------------------------------------
                // 🟦 OBTENER CORREOS DE TODOS LOS AGENTES
                // -----------------------------------------
                var correosAgentes = await db.Usuarios
                    .Where(u => u.UsuarioRols.Any(ur => ur.Rol.Nombre == "Admin"))
                    .Select(u => u.Email)
                    .ToListAsync();

                if (!correosAgentes.Any())
                    return; // no hay agentes, no enviamos nada

                // -----------------------------------------
                // 🔍 DOCUMENTOS VENCIDOS
                // -----------------------------------------
                var vencidos = await db.Documentos
                    .Where(d => d.FechaVencimiento < hoy)
                    .ToListAsync();

                // 🔍 DOCUMENTOS POR VENCER (5 días)
                var porVencer = await db.Documentos
                    .Where(d => d.FechaVencimiento >= hoy &&
                                d.FechaVencimiento <= hoy.AddDays(5))
                    .ToListAsync();

                string html = "<h2>Reporte Diario de Documentos</h2>";
                bool hayAlerta = false;

                // -------------------------------
                // 🟥 VENCIDOS
                // -------------------------------
                if (vencidos.Any())
                {
                    html += "<h3 style='color:red;'>Vencidos:</h3><ul>";

                    foreach (var d in vencidos)
                    {
                        html += $"<li>{d.Nombre} — {d.FechaVencimiento:yyyy-MM-dd}</li>";

                        await alertaService.CrearAlertaAsync(d.DocumentoId, "Vencido", "Pendiente");

                        hayAlerta = true;
                    }

                    html += "</ul>";
                }

                // -------------------------------
                // 🟧 POR VENCER
                // -------------------------------
                if (porVencer.Any())
                {
                    html += "<h3 style='color:orange;'>Por vencer (5 días):</h3><ul>";

                    foreach (var d in porVencer)
                    {
                        html += $"<li>{d.Nombre} — {d.FechaVencimiento:yyyy-MM-dd}</li>";

                        await alertaService.CrearAlertaAsync(d.DocumentoId, "PorVencer", "Pendiente");

                        hayAlerta = true;
                    }

                    html += "</ul>";
                }

                // -----------------------------------------
                // 📧 ENVIAR SOLO SI HAY ALERTAS NUEVAS HOY
                // -----------------------------------------
                if (hayAlerta)
                {
                    foreach (var correo in correosAgentes)
                    {
                        await email.SendAsync(
                            correo,
                            "Reporte Diario — Documentos de Luxprop",
                            html
                        );
                    }
                }
            }

            // Espera 24h
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    // -----------------------------------------
    // ⏰ Esperar a la hora exacta
    // -----------------------------------------
    private async Task EsperarHastaHoraAsync(int hour, int minute, CancellationToken stoppingToken)
    {
        var now = DateTime.Now;
        var target = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);

        if (now > target)
            target = target.AddDays(1);

        var delay = target - now;
        await Task.Delay(delay, stoppingToken);
    }
}
