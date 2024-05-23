using FunctionCallingFeature.Models.EShop;

namespace FunctionCallingFeature.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> CreateAsync(int cartId);
        Task<List<Order>> GetOrders();
        Task<Order> GetOrderByIdAsync(int id);
    }
}
