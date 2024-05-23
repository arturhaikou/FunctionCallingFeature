using FunctionCallingFeature.Models.EShop;
using FunctionCallingFeature.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FunctionCallingFeature.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        public CatalogController(ICatalogService service) => _catalogService = service;

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            return await _catalogService.GetItemsAsync();
        }
    }
}
