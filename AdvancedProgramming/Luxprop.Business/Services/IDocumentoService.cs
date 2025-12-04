using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luxprop.Business.Services
{
    public interface IDocumentoService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
        Task DeleteFileAsync(string fileUrl, int? expedienteId, int usuarioId);
        Task<bool> UpdateDocumentStatusAsync(int documentoId, string newStatus, int usuarioId);
        Task UpdateExpedienteStatusAsync(int? expedienteId, int usuarioId);
    }
}
