using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luxprop.Data.Models;

namespace Luxprop.Business.Services._360
{
    public interface IPropertyTour360Service
    {
        PropertyTour360? GetByPropertyId(int propertyId);
        Task<PropertyTour360?> GetByPropertyIdAsync(int propertyId);
        Task CreateOrUpdateAsync(int propertyId, string tourUrl, string? title, string userName);
        Task DeleteAsync(int propertyId);
    }
}
