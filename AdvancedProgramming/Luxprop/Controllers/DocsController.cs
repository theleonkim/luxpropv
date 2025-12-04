using Luxprop.Business.Services.Docs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luxprop.Controllers;

[Authorize(Policy = "DocsReaders")]
[Route("[controller]")]
public class DocsController : Controller
{
    private readonly IDocService _docService;

    public DocsController(IDocService docService)
    {
        _docService = docService;
    }

    [HttpGet("Download/{id:int}")]
    public async Task<IActionResult> Download(int id)
    {
        var result = await _docService.GetFileAsync(id, User);
        if (result is null)
            return Forbid(); // no permission or not found

        var (file, contentType, fileName) = result.Value;
        return File(file, contentType, fileName);
    }
}
