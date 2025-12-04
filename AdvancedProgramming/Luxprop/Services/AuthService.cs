using System.Threading.Tasks;

namespace Luxprop.Services
{
    public class AuthService
    {
        private readonly SessionService _session;

        public bool IsAuthenticated { get; private set; }
        public string? UserEmail { get; private set; }
        public string? UserRole { get; private set; }
        public string? UserName { get; private set; }

        public AuthService(SessionService session)
        {
            _session = session;
        }

        /// <summary>
        /// Autenticación simple. En una app real, valida en base de datos.
        /// </summary>
        public async Task<bool> LoginAsync(string email, string password)
        {
            // 🔹 Ejemplo de usuarios ficticios por rol
            var users = new Dictionary<string, (string Password, string Name, string Role)>
            {
                ["admin@example.com"] = ("password", "Admin User", "Admin"),
                ["agent@example.com"] = ("password", "Agent User", "Agente"),
                ["client@example.com"] = ("password", "Client User", "Cliente")
            };

            if (users.TryGetValue(email, out var info) && password == info.Password)
            {
                IsAuthenticated = true;
                UserEmail = email;
                UserName = info.Name;
                UserRole = info.Role;

                // Guarda la sesión en el navegador
                await _session.SetUserAsync(
                    userId: GetFakeUserId(info.Role),
                    userName: info.Name,
                    roleName: info.Role
                );

                return true;
            }

            return false;
        }

        /// <summary>
        /// Cierra la sesión y limpia el almacenamiento del navegador.
        /// </summary>
        public async Task LogoutAsync()
        {
            IsAuthenticated = false;
            UserEmail = null;
            UserRole = null;
            UserName = null;
            await _session.LogoutAsync();
        }

        /// <summary>
        /// Genera IDs ficticios por rol (solo para pruebas).
        /// </summary>
        private int GetFakeUserId(string role) => role switch
        {
            "Admin" => 1,
            "Agente" => 2,
            "Cliente" => 3,
            _ => 0
        };
    }
}
