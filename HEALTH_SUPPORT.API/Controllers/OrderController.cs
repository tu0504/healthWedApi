using HEALTH_SUPPORT.Services.Implementations;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace HEALTH_SUPPORT.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest.CreateOrderModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid order data" });
            }

            await _orderService.CreateOrder(model);

            return Ok(new { message = "Tạo đơn hàng thành công" });
        }

        [HttpGet("{orderId}", Name = "GetOrderDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetOrderDetails(Guid orderId)
        {
            var result = await _orderService.GetOrderDetails(orderId);

            if (result == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            return Ok(result);
        }

        [HttpGet(Name = "GetOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetOrders()
        {
            var result = await _orderService.GetOrders();
            return Ok(result);
        }

        [HttpPut("{orderId}", Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateOrder(Guid orderId, [FromBody] OrderRequest.UpdateOrderModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid update data" });
            }
            // Kiểm tra xem order có tồn tại không
            var existingOrder = await _orderService.GetOrderDetailsDeleted(orderId);
            if (existingOrder == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            await _orderService.UpdateOrder(orderId, model);
            return Ok(new { message = "Order updated successfully" });
        }

        [HttpDelete("{orderId}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOrder(Guid orderId)
        {
            var existingOrder = await _orderService.GetOrderDetails(orderId);
            if (existingOrder == null)
            {
                return NotFound(new { message = "Order not found" });
            }
            await _orderService.RemoveOrder(orderId);
            return Ok(new { message = "Order deleted successfully" });
        }
    }
}
