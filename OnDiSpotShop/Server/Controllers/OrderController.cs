using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnDiSpotShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewResponse>>>> GetOrders()
        {
            var result = await orderService.GetOrders();
            return Ok(result);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<ServiceResponse<OrderDetailsResponse>>> GetOrdersDetails(int orderId)
        {
            var result = await orderService.GetOrderDetails(orderId);
            return Ok(result);
        }
    }
}
