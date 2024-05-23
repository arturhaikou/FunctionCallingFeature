using FunctionCallingFeature.Data;
using FunctionCallingFeature.Models.EShop;
using FunctionCallingFeature.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace FunctionCallingFeature.Services.EShop
{
    public class PurchaseService : IPurchaseService
    {
        private readonly EShopDbContext _context;
        public PurchaseService(EShopDbContext context) => _context = context;

        [KernelFunction("purchase_order")]
        [Description("Purchase the order")]
        public async Task<bool> BuyAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(order => order.Id == orderId);

            if (order is null)
            {
                throw new ArgumentException("It's not possible to find an order");
            }

            order.Status = OrderStatus.Paid;
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
