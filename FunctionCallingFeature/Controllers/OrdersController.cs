using FunctionCallingFeature.Models.EShop;
using FunctionCallingFeature.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FunctionCallingFeature.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService) => _orderService = orderService;
        
        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
        {
            return await _orderService.GetOrders();
        }
    }
}
