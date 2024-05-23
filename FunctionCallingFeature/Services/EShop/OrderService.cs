using FunctionCallingFeature.Data;
using FunctionCallingFeature.Models.EShop;
using FunctionCallingFeature.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace FunctionCallingFeature.Services.EShop
{
    public class OrderService : IOrderService
    {
        private readonly EShopDbContext _context;
        private readonly ICartService _cartService;
        public OrderService(EShopDbContext context, ICartService cartService) => (_context, _cartService) = (context, cartService);

        [KernelFunction("create_order")]
        [Description("Create an order from the cart")]
        public async Task<int> CreateAsync(int cartId)
        {
            var cart = await _cartService.GetCartAsync(cartId);

            if (cart == null)
            {
                throw new ArgumentNullException("It's not possible to find a cart");
            }

            var order = new Order
            {
                Status = OrderStatus.Created,
                CreateDate = DateTime.UtcNow,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderProducts = cart.ProdcutIds.Select(productId => new OrderProduct { OrderId = order.Id, ProductId = productId });
            _context.OrderProducts.AddRange(orderProducts);
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(order => order.Id == id);
        }

        public async Task<List<Order>> GetOrders()
        {
            return await _context.Orders.Include(order => order.Products).ThenInclude(product => product.Product).ToListAsync();
        }
    }
}
