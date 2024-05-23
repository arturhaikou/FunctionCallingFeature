using FunctionCallingFeature.Models.EShop;

namespace FunctionCallingFeature.Services.Interfaces
{
    public interface ICartService
    {
        Task<int> CreateAsync(List<Guid> ids);
        Task<bool> ClearAsync(int id);
        Task<bool> AddItemAsync(Guid id, int cartId);
        Task<bool> RemoveItemAsync(Guid id, int cartId);
        Task<Cart> GetCartAsync(int id);
    }
}
