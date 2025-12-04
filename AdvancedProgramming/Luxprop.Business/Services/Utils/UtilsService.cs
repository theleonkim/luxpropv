using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luxprop.Business.Services
{
    public interface IUtilsService
    {
        Task<string> NormalizeRole(string? roleName);
    }

    public class UtilsService : IUtilsService
    {
        public Task<string> NormalizeRole(string? roleName)
        {
            var role = roleName?.Trim().ToLowerInvariant();

            var normalized = role switch
            {
                "administrador" or "admin" => "admin",
                "agente" or "agent" => "agent",
                "Seller" or "seller" => "seller",
                "comprador" or "buyer" or "cliente" => "buyer",
                _ => string.Empty
            };

            return Task.FromResult(normalized);
        }
    }
}
