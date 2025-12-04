using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Luxprop.Business.Services;
using Luxprop.Business.Services.Dashboard;
using Luxprop.Business.Services.Docs;
using Luxprop.Data.Models;
using Luxprop.Data.Repositories;
using Luxprop.Data.Repositories;
using Luxprop.Hubs;
using Luxprop.Hubs;
using Luxprop.Services;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;   // <-- IMPORTANTE
// Alias explícito al hub correcto
using ChatHubType = Luxprop.Hubs.ChatHub;

var builder = WebApplication.CreateBuilder(args);

// 1) Servicios (TODO antes de Build)
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Controllers para ReportsController
builder.Services.AddControllers();

builder.Services.AddDbContextFactory<LuxpropContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Luxprop")));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<IDocumentoService, DocumentoService>();
builder.Services.AddScoped<PasswordHelper>();
builder.Services.AddScoped<AuditoriaService>();
builder.Services.AddScoped<SecurityService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddHostedService<ReminderNotifier>();
builder.Services.AddSingleton<IEmailService, SmtpEmailService>();
builder.Services.AddScoped<IHistorialExpedienteRepository, HistorialExpedienteRepository>();
builder.Services.AddHostedService<DocumentExpirationJob>();
builder.Services.AddScoped<IPropiedadRepository, PropiedadRepository>();
builder.Services.AddScoped<IPropiedadService, PropiedadService>();
builder.Services.AddScoped<DocumentoService>();
builder.Services.AddScoped<IUtilsService, UtilsService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();



builder.Services.AddScoped<IPropiedadService, PropiedadService>();

builder.Services.AddScoped<IAlertasDocumentoRepository, AlertasDocumentoRepository>();
builder.Services.AddScoped<IAlertasDocumentoService, AlertasDocumentoService>();


builder.Services.AddHttpContextAccessor();

// Autorización por roles reales (ya la usas en componentes Blazor)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DocsReaders", policy =>
        policy.RequireRole("admin", "agent"));
});
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddHostedService<ReminderNotifier>();
builder.Services.AddSingleton<IEmailService, SmtpEmailService>();
builder.Services.AddHttpContextAccessor();




// 🔹 Repositorios base
builder.Services.AddScoped<IExpedienteRepository, ExpedienteRepository>();
builder.Services.AddScoped<IHistorialExpedienteRepository, HistorialExpedienteRepository>();


builder.Services.AddScoped<IHistorialExpedienteService, HistorialExpedienteService>();
builder.Services.AddScoped<IExpedienteService, ExpedienteService>();

// Servicio de documentos
builder.Services.AddScoped<IDocService, DocService>();

builder.Services.AddScoped<EmailService>();

// SignalR
builder.Services.AddSignalR();

// (Opcional) Firebase credencial por variable de entorno
//var credentialPath = @"C:\ProyectoFinalGrupal\Luxprop\AdvancedProgramming\Luxprop\App_Data\firebase-config.json";
var credentialPath = @"C:\Users\pepon\Documents\GitHub\Luxprop\AdvancedProgramming\Luxprop\App_Data\firebase-config.json";
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

// ***** QuestPDF: licencia *****
QuestPDF.Settings.License = LicenseType.Community;

// 2) Construcción de la app
var app = builder.Build();

// 3) Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// carpeta /wwwroot/uploads
var uploadsPath = Path.Combine(app.Environment.WebRootPath, "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseRouting();

// Si empiezas a usar [Authorize] en controllers, aquí iría:
// app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapControllers();                 // <-- PARA ReportsController
app.MapBlazorHub();
app.MapHub<ChatHubType>("/hubs/chat");
app.MapFallbackToPage("/_Host");

app.Run();
