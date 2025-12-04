using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;


public class ReminderNotifier : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<ReminderNotifier> _log;

    public ReminderNotifier(IServiceProvider sp, ILogger<ReminderNotifier> log) { _sp = sp; _log = log; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<LuxpropContext>();
                var mail = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var cfg = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                var now = DateTime.UtcNow;

                // 1) Avisos previos al evento
                var proximos = await db.Recordatorios
                    .Where(r => r.NotificarCorreo && r.Estado == "Pendiente")
                    .Where(r => r.Inicio > now && r.Inicio <= now.AddMinutes(r.MinutosAntes))
                    .Where(r => r.UltimoAviso == null)   // simple de-dup
                    .ToListAsync(stoppingToken);

                foreach (var r in proximos)
                {
                    // 🔹 Obtener el correo del usuario asociado al recordatorio
                    var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == r.UsuarioId);

                    if (usuario != null && !string.IsNullOrEmpty(usuario.Email))
                    {
                        await mail.SendAsync(
                            usuario.Email,
                            $"[Luxprop] Recordatorio: {r.Titulo}",
                            $"<p>Evento: {r.Titulo}</p><p>Inicio: {r.Inicio:yyyy-MM-dd HH:mm}</p>"
                        );

                        r.UltimoAviso = now;
                    }
                }


                // 2) Incumplidos
                var vencidos = await db.Recordatorios
                    .Where(r => r.Estado == "Pendiente")
                    .Where(r =>
                        (r.Fin != null && r.Fin < now) ||
                        (r.Fin == null && r.Inicio.AddMinutes(30) < now))
                    .ToListAsync(stoppingToken);

                if (vencidos.Any())
                {
                    var admins = (cfg["Smtp:Admins"] ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var r in vencidos)
                    {
                        r.Estado = "Incumplido";

                        // Buscar el usuario dueño del recordatorio
                        var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == r.UsuarioId);

                        // Enviar notificación al usuario afectado
                        if (usuario != null && !string.IsNullOrEmpty(usuario.Email))
                        {
                            await mail.SendAsync(
                                usuario.Email,
                                $"[Luxprop] Evento incumplido: {r.Titulo}",
                                $"<p>Tu evento '{r.Titulo}' no se completó en el tiempo esperado.</p>" +
                                $"<p>Inicio: {r.Inicio:yyyy-MM-dd HH:mm}</p>"
                            );
                        }

                        // Notificar también a los administradores
                        foreach (var admin in admins)
                        {
                            if (!string.IsNullOrWhiteSpace(admin))
                            {
                                await mail.SendAsync(
                                        usuario.Email,
                                        $"[Luxprop] Recordatorio: {r.Titulo}",
                                        $@"
                                        <div style='font-family:Arial,sans-serif; background:#f9fafb; padding:20px; border-radius:8px;'>
                                            <h2 style='color:#007bff;'>🔔 Recordatorio Luxprop</h2>
                                            <p><strong>Evento:</strong> {r.Titulo}</p>
                                            <p><strong>Inicio:</strong> {r.Inicio:yyyy-MM-dd HH:mm}</p>
                                            <p><strong>Descripción:</strong> {r.Descripcion}</p>
                                            <p><strong>Tipo:</strong> {r.Tipo}</p>
                                            <hr />
                                            <p style='font-size:13px; color:#555;'>Este recordatorio fue generado automáticamente por el sistema <b>Luxprop</b>.</p>
                                        </div>"
                                    );

                            }
                        }
                    }

                    await db.SaveChangesAsync(stoppingToken);

                }

                await db.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error en ReminderNotifier");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    private static string ObtenerCorreoAgente(int agenteId, LuxpropContext db)
        => db.Usuarios.Where(u => u.UsuarioId == agenteId).Select(u => u.Email).First(); // ajusta entidad Usuario
}
