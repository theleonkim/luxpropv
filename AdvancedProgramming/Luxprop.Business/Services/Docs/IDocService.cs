using System.Security.Claims;

namespace Luxprop.Business.Services.Docs;

public interface IDocService
{
    Task<IReadOnlyList<DocRowDto>> ListAsync(DocCriteria criteria, ClaimsPrincipal user);
    Task<(byte[] file, string contentType, string downloadName)?> GetFileAsync(int documentoId, ClaimsPrincipal user);
}
