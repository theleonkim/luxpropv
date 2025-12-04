using Luxprop.Business.Services._360;
using Luxprop.Models._360;
using Luxprop.Pages.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Agent,Admin")]
public class PropertyTour360Controller : Controller
{
    private readonly IPropertyTour360Service _tourService;

    public PropertyTour360Controller(IPropertyTour360Service tourService)
    {
        _tourService = tourService;
    }

    // GET: /PropertyTour360/Edit/5  (5 = propertyId)
    public async Task<IActionResult> Edit(int propertyId)
    {
        var tour = await _tourService.GetByPropertyIdAsync(propertyId);

        var model = new PropertyTour360ViewModel
        {
            PropertyId = propertyId,
            TourUrl = tour?.TourUrl,
            Title = tour?.Title
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PropertyTour360ViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userName = User.Identity?.Name ?? "system";

        await _tourService.CreateOrUpdateAsync(model.PropertyId, model.TourUrl!, model.Title, userName);

        TempData["SuccessMessage"] = "Recorrido 360° actualizado correctamente.";
        return RedirectToAction("Details", "Property", new { id = model.PropertyId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int propertyId)
    {
        await _tourService.DeleteAsync(propertyId);
        TempData["SuccessMessage"] = "Recorrido 360° eliminado.";
        return RedirectToAction("Details", "Property", new { id = propertyId });
    }
}
