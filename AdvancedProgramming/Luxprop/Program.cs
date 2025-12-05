using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Luxprop.Business.Services;
using Luxprop.Business.Services.Dashboard;
using Luxprop.Business.Services.Docs;
using Luxprop.Data.Models;
using Luxprop.Data.Repositories;
using Luxprop.Hubs;
using Luxprop.Services;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Razor & Blazor
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers();

// DB CONTEXT (Render toma variable environment ConnectionStrings__Luxprop)
builder.Services.AddDbContextFactory<LuxpropContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Luxprop"))
);

// Services & Repositories
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
builder.Services.AddScoped<IUtilsService, UtilsService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDocService, DocService>();
builder.Services.AddScoped<EmailService>();

builder.Services.AddHttpContextAccessor();

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DocsReaders", policy =>
        policy.RequireRole("admin", "agent"));
});

// SignalR
builder.Services.AddSignalR();

// Firebase (manejo para producción)
if (builder.Environment.IsDevelopment())
{
    string credentialPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "firebase-config.json");
    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
}

QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Create /uploads
var uploadsPath = Path.Combine(app.Environment.WebRootPath, "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapHub<ChatHub>("/hubs/chat");
app.MapFallbackToPage("/_Host");

app.Run();
