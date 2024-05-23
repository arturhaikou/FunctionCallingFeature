using FunctionCallingFeature.Models.EShop;

namespace FunctionCallingFeature.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<Product>> GetItemsAsync();
    }
}
