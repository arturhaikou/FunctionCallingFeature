using FunctionCallingFeature.Data;
using FunctionCallingFeature.Models.EShop;
using FunctionCallingFeature.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace FunctionCallingFeature.Services.EShop
{
    public class CatalogService : ICatalogService
    {
        private readonly EShopDbContext _context;
        public CatalogService(EShopDbContext context) => _context = context;

        [KernelFunction("get_products")]
        [Description("Get catalog products")]
        public async Task<List<Product>> GetItemsAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
