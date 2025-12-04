using Luxprop.Data;
using Luxprop.Data.Models;

namespace Luxprop.Web.Services
{
    public class AuditoriaService
    {
        private readonly LuxpropContext _db;

        public AuditoriaService(LuxpropContext db)
        {
            _db = db;
        }

        public void RegistrarAuditoria(int usuarioId, string accion, string entidad, string descripcion)
        {
            try
            {
                var auditoria = new Auditorium
                {
                    UsuarioId = usuarioId,
                    Accion = accion,
                    Entidad = entidad,
                    //Desc = descripcion,
                    Fecha = DateTime.Now,
                    //IpAddress = GetClientIPAddress()
                };

                _db.Auditoria.Add(auditoria);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log error but don't throw to avoid breaking the main operation
                Console.WriteLine($"Error registering audit: {ex.Message}");
            }
        }

        private string GetClientIPAddress()
        {
            // This is a simplified implementation
            // In a real application, you would get this from the HTTP context
            return "127.0.0.1";
        }
    }
}
