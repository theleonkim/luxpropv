using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Business.Services._360
{
    public class PropertyTour360Service : IPropertyTour360Service
    {
        private readonly LuxpropContext _context;

        public PropertyTour360Service(LuxpropContext context)
        {
            _context = context;
        }

        public PropertyTour360? GetByPropertyId(int propertyId)
            => _context.PropertyTours360.SingleOrDefault(t => t.PropertyId == propertyId && t.IsActive);

        public async Task<PropertyTour360?> GetByPropertyIdAsync(int propertyId)
            => await _context.PropertyTours360
                             .SingleOrDefaultAsync(t => t.PropertyId == propertyId && t.IsActive);

        public async Task CreateOrUpdateAsync(int propertyId, string tourUrl, string? title, string userName)
        {
            var existing = await _context.PropertyTours360
                .SingleOrDefaultAsync(t => t.PropertyId == propertyId);

            if (existing == null)
            {
                var entity = new PropertyTour360
                {
                    PropertyId = propertyId,
                    TourUrl = tourUrl,
                    Title = title,
                    CreatedBy = userName
                };
                _context.PropertyTours360.Add(entity);
            }
            else
            {
                existing.TourUrl = tourUrl;
                existing.Title = title;
                existing.IsActive = true;
                existing.UpdatedAt = DateTime.UtcNow;
                existing.UpdatedBy = userName;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int propertyId)
        {
            var existing = await _context.PropertyTours360
                .SingleOrDefaultAsync(t => t.PropertyId == propertyId);

            if (existing != null)
            {
                existing.IsActive = false; // baja lógica
                existing.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
