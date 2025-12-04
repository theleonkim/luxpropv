using Luxprop.Data.Models;
using Luxprop.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Business.Services
{
    public interface IExpedienteService
    {
        Task<IEnumerable<Expediente>> GetAllAsync();
        Task<Expediente?> GetByIdAsync(int id);
        Task CreateAsync(Expediente expediente, int usuarioId, string ip);
        Task UpdateAsync(Expediente expediente, int usuarioId, string ip);
        Task CloseAsync(int id, int usuarioId, string ip);
        Task<bool> DeleteAsync(int id);
        Task<Cliente> GetClientByIdAsync(int id);
    }

    public class ExpedienteService : IExpedienteService
    {
        private readonly IExpedienteRepository _expedienteRepo;
        private readonly IHistorialExpedienteRepository _historialRepo;
        private readonly LuxpropContext _db;

        public ExpedienteService(
            IExpedienteRepository expedienteRepo,
            IHistorialExpedienteRepository historialRepo,
            LuxpropContext db)
        {
            _expedienteRepo = expedienteRepo;
            _historialRepo = historialRepo;
            _db = db;
        }

        // ✅ Carga todos los expedientes con sus relaciones necesarias para la tabla
        public async Task<IEnumerable<Expediente>> GetAllAsync()
        {
            return await _db.Expedientes
                .Include(e => e.Propiedad)
                .Include(e => e.Agente)
                .Include(e => e.Cliente)
                    .ThenInclude(c => c.Usuario)   // Cliente.Usuario
                .Include(e => e.Agente)           // Agente es Usuario directamente
                .Include(e => e.Documentos)
                .Include(e => e.HistorialExpedientes)
                .ToListAsync();
        }

        public async Task<Expediente?> GetByIdAsync(int id)
        {
            return await _db.Expedientes
                .Include(e => e.Cliente)
                    .ThenInclude(c => c.Usuario)
                .Include(e => e.Agente)
                .Include(e => e.Propiedad)
                .Include(e => e.Documentos)
                .Include(e => e.HistorialExpedientes)
                .FirstOrDefaultAsync(e => e.ExpedienteId == id);
        }

        // ✅ Crea un expediente y registra historial
        public async Task CreateAsync(Expediente expediente, int usuarioId, string ip)
        {
            expediente.FechaApertura = DateOnly.FromDateTime(DateTime.Now);
            expediente.Estado = "Abierto";
            expediente.UltimaActualizacion = DateTime.Now;

            await _expedienteRepo.CreateAsync(expediente);

            await RegistrarHistorial(
                expediente.ExpedienteId,
                usuarioId,
                "Creación",
                null,
                expediente.Estado,
                "Creación de expediente",
                "Nuevo expediente creado",
                ip
            );
        }

        // ✅ Actualiza un expediente y registra historial de cambios
        public async Task UpdateAsync(Expediente expediente, int usuarioId, string ip)
        {
            var anterior = await _db.Expedientes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ExpedienteId == expediente.ExpedienteId);

            if (anterior == null)
                throw new Exception("El expediente no existe.");

            expediente.UltimaActualizacion = DateTime.Now;
            await _expedienteRepo.UpdateAsync(expediente);

            await RegistrarHistorial(
                expediente.ExpedienteId,
                usuarioId,
                "Actualización",
                anterior.Estado,
                expediente.Estado,
                "Actualización de expediente",
                "Expediente modificado",
                ip
            );
        }

        // ✅ Cierra expediente y deja constancia en el historial
        public async Task CloseAsync(int id, int usuarioId, string ip)
        {
            var expediente = await _db.Expedientes.FindAsync(id);
            if (expediente == null)
                throw new Exception("El expediente no existe.");

            var estadoAnterior = expediente.Estado;
            expediente.Estado = "Cerrado";
            expediente.FechaCierre = DateOnly.FromDateTime(DateTime.Now);
            expediente.UltimaActualizacion = DateTime.Now;

            await _expedienteRepo.UpdateAsync(expediente);

            await RegistrarHistorial(
                id,
                usuarioId,
                "Cierre",
                estadoAnterior,
                expediente.Estado,
                "Cierre de expediente",
                "Expediente finalizado correctamente",
                ip
            );
        }

        // ✅ Eliminar expediente (borrando hijos para evitar errores de FK)
        public async Task<bool> DeleteAsync(int id)
        {
            var exp = await _db.Expedientes
                .Include(e => e.Documentos)
                .Include(e => e.Cita)
                .Include(e => e.TareaTramites)
                .Include(e => e.HistorialExpedientes)
                .FirstOrDefaultAsync(e => e.ExpedienteId == id);

            if (exp == null)
                return false;

            // Borrar hijos primero (por si las FKs no tienen cascade)
            if (exp.Documentos != null && exp.Documentos.Any())
                _db.Documentos.RemoveRange(exp.Documentos);

            if (exp.Cita != null && exp.Cita.Any())
                _db.Cita.RemoveRange(exp.Cita);

            if (exp.TareaTramites != null && exp.TareaTramites.Any())
                _db.TareaTramites.RemoveRange(exp.TareaTramites);

            if (exp.HistorialExpedientes != null && exp.HistorialExpedientes.Any())
                _db.HistorialExpedientes.RemoveRange(exp.HistorialExpedientes);

            _db.Expedientes.Remove(exp);

            await _db.SaveChangesAsync();
            return true;
        }

        // ✅ Método auxiliar centralizado
        private async Task RegistrarHistorial(
            int expedienteId,
            int usuarioId,
            string tipoAccion,
            string? estadoAnterior,
            string? estadoNuevo,
            string descripcion,
            string observacion,
            string ip)
        {
            var registro = new HistorialExpediente
            {
                ExpedienteId = expedienteId,
                UsuarioId = usuarioId,
                TipoAccion = tipoAccion,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = estadoNuevo,
                Descripcion = descripcion,
                Observacion = observacion,
                IPRegistro = ip,
                FechaModificacion = DateTime.Now
            };

            await _historialRepo.CreateAsync(registro);
        }

        public async Task<Cliente> GetClientByIdAsync(int id)
        {
            var cliente = await _db.Clientes
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(e => e.UsuarioId == id);

            return cliente ?? new();
        }
    }
}
