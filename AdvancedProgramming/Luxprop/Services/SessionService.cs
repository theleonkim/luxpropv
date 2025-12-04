using Microsoft.JSInterop;

namespace Luxprop.Services
{
    public class SessionService
    {
        private readonly IJSRuntime _js;

        // ✅ Propiedades en memoria
        public int CurrentUserId { get; private set; }
        public string? CurrentUserName { get; private set; }
        public string? CurrentUserRole { get; private set; }
        public string? CurrentUserEmail { get; private set; }

        public SessionService(IJSRuntime js)
        {
            _js = js;
        }

        // ✅ Guarda en sessionStorage y también en memoria
        //    (se mantiene igual que antes, pero podés pasar email opcional)
        public async Task SetUserAsync(int userId, string userName, string roleName, string? userEmail = null)
        {
            CurrentUserId = userId;
            CurrentUserName = userName;
            CurrentUserRole = roleName;
            CurrentUserEmail = userEmail;

            await _js.InvokeVoidAsync("sessionStorage.setItem", "UserId", userId.ToString());
            await _js.InvokeVoidAsync("sessionStorage.setItem", "UserName", userName);
            await _js.InvokeVoidAsync("sessionStorage.setItem", "UserRole", roleName);

            if (!string.IsNullOrEmpty(userEmail))
                await _js.InvokeVoidAsync("sessionStorage.setItem", "UserEmail", userEmail);
        }

        // ✅ Carga desde sessionStorage a las propiedades locales
        public async Task LoadUserAsync()
        {
            var idString = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserId");
            if (int.TryParse(idString, out var id))
                CurrentUserId = id;

            CurrentUserName = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserName");
            CurrentUserRole = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserRole");
            CurrentUserEmail = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserEmail");
        }

        // ✅ Obtener nombre actual
        public async Task<string?> GetUserNameAsync()
        {
            if (string.IsNullOrEmpty(CurrentUserName))
                CurrentUserName = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserName");
            return CurrentUserName;
        }

        // ✅ Obtener rol actual
        public async Task<string?> GetUserRoleAsync()
        {
            if (string.IsNullOrEmpty(CurrentUserRole))
                CurrentUserRole = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserRole");
            return CurrentUserRole;
        }

        // ✅ Obtener correo actual
        public async Task<string?> GetUserEmailAsync()
        {
            if (string.IsNullOrEmpty(CurrentUserEmail))
                CurrentUserEmail = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserEmail");
            return CurrentUserEmail;
        }

        // ✅ Obtener ID actual
        public async Task<int> GetUserIdAsync()
        {
            if (CurrentUserId == 0)
            {
                var idString = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserId");
                if (int.TryParse(idString, out var id))
                    CurrentUserId = id;
            }
            return CurrentUserId;
        }

        public async Task<string?> GetUserRoleRawAsync()
        {
            return await _js.InvokeAsync<string>("sessionStorage.getItem", "UserRoleRaw");
        }

        // ✅ Cierra sesión y limpia almacenamiento
        public async Task LogoutAsync()
        {
            CurrentUserId = 0;
            CurrentUserName = null;
            CurrentUserRole = null;
            CurrentUserEmail = null;

            // 🔥 BORRAR TODO LO DEL SESSION STORAGE
            await _js.InvokeVoidAsync("sessionStorage.removeItem", "UserId");
            await _js.InvokeVoidAsync("sessionStorage.removeItem", "UserName");
            await _js.InvokeVoidAsync("sessionStorage.removeItem", "UserRole");
            await _js.InvokeVoidAsync("sessionStorage.removeItem", "UserEmail");
            await _js.InvokeVoidAsync("sessionStorage.removeItem", "UserRoleRaw");

            // (Opcional) Limpiar todo completamente:
            // await _js.InvokeVoidAsync("sessionStorage.clear");
        }


        // ✅ Verifica si hay sesión activa
        public bool IsAuthenticated => CurrentUserId > 0;
    }



}
