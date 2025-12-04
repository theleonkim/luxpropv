using Luxprop.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Luxprop.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Register all repository interfaces and implementations
            services.AddScoped<IAgenteRepository, AgenteRepository>();
            services.AddScoped<IAuditoriumRepository, AuditoriumRepository>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IChatThreadRepository, ChatThreadRepository>();
            services.AddScoped<ICitumRepository, CitumRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IDocumentoRepository, DocumentoRepository>();
            services.AddScoped<IExpedienteRepository, ExpedienteRepository>();
            services.AddScoped<IPropiedadRepository, PropiedadRepository>();
            services.AddScoped<IRolRepository, RolRepository>();
            services.AddScoped<ITareaTramiteRepository, TareaTramiteRepository>();
            services.AddScoped<IUbicacionRepository, UbicacionRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioRolRepository, UsuarioRolRepository>();
            services.AddScoped<IAlertasDocumentoRepository, AlertasDocumentoRepository>();

            return services;
        }
    }
}
