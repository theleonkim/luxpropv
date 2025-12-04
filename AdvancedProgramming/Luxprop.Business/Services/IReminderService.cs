public interface IReminderService
{
    Task<Recordatorio> CreateAsync(Recordatorio r, CancellationToken ct = default);
    Task<Recordatorio?> GetAsync(int id, CancellationToken ct = default);
    Task<List<Recordatorio>> ListAsync(int? usuarioId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
    Task<bool> UpdateAsync(Recordatorio r, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> SetEstadoAsync(int id, string nuevoEstado, CancellationToken ct = default); // Pendiente/Completado/Incumplido

    Task AddAsync(Recordatorio recordatorio);
    Task<Recordatorio?> GetByIdAsync(int id);
    Task UpdateAsync(Recordatorio recordatorio);

}
