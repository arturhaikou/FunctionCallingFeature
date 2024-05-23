using FunctionCallingFeature.Models.EShop;
using FunctionCallingFeature.Services.Interfaces;
using Microsoft.SemanticKernel;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace FunctionCallingFeature.Services.EShop
{
    public class CartService : ICartService
    {
        private static ConcurrentDictionary<int, List<Guid>> _carts = new ConcurrentDictionary<int, List<Guid>>();
        private static int _cartId = 0;

        public Task<bool> AddItemAsync(Guid itemId, int cartId)
        {
            if (_carts.TryGetValue(cartId, out List<Guid> items))
            {
                items.Add(itemId);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> ClearAsync(int id)
        {
            if (_carts.ContainsKey(id))
            {
                return Task.FromResult(_carts.TryRemove(id, out _));
            }

            return Task.FromResult(false);
        }

        [KernelFunction("add_product_to_cart")]
        [Description("Create a cart and add a catalog products to it")]
        public Task<int> CreateAsync(List<Guid> productIds)
        {
            var id = Interlocked.Increment(ref _cartId);
            _carts.TryAdd(id, productIds);
            return Task.FromResult(id);
        }

        public Task<Cart> GetCartAsync(int id)
        {
            Cart cart = null;
            if (_carts.TryGetValue(id, out List<Guid> ids))
            {
                cart = new Cart
                {
                    Id = id,
                    ProdcutIds = ids
                };
            }

            return Task.FromResult(cart);
        }

        public Task<bool> RemoveItemAsync(Guid itemId, int cartId)
        {

            if (_carts.TryGetValue(cartId, out List<Guid> items))
            {
                items.Remove(itemId);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
