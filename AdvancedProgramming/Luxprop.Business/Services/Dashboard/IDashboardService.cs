using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luxprop.Data.Models;

namespace Luxprop.Business.Services.Dashboard
{
    public  interface IDashboardService
    {
        Task<List<Documento>> GetDocumentsToExpireAsync(int daysAhead = 7);
        Task<List<Recordatorio>> GetUpcomingRemindersAsync(int daysAhead = 7, int? userId = null);
    }
}
