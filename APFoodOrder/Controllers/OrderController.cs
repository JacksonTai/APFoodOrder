using APFoodOrder.Constant;
using APFoodOrder.Model;
using APFoodOrder.Service;
using Microsoft.AspNetCore.Mvc;

namespace APFoodOrder.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrderController(
        IOrderService orderService
        ) : ControllerBase
    {

        private readonly IOrderService _orderService = orderService;

        [HttpPost(Name = "CreateOrder")]
        public Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestModel createOrderRequestModel)
        {
            return Task.FromResult<IActionResult>(Ok(_orderService.CreateOrder(createOrderRequestModel)));
        }

        [HttpPut("{id}", Name = "UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus(int id, OrderStatus status)
        {
            await _orderService.UpdateOrderStatusAsync(id, status);
            return Ok();
        }

        [HttpGet(Name = "GetOrders")]
        public async Task<IActionResult> GetOrders(string? customerId, OrderStatus status = OrderStatus.Pending)
        {
            List<OrderListViewModel> orders = await _orderService.GetOrdersByStatusAsync(status, customerId);
            return Ok(orders);
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<IActionResult> GetOrder(int id)
        {
            return Ok(await _orderService.GetOrderByIdAsync(id));
        }

        [HttpGet("counts", Name = "GetOrderCounts")]
        public async Task<IActionResult> GetOrderCounts(string? customerId)
        {
            return Ok(await _orderService.GetOrderCountsAsync(customerId));
        }


        [HttpPost("summary" ,Name = "GetOrderSummary")]
        public IActionResult GetOrderSummary([FromBody] OrderSummaryRequestModel orderSummaryRequestModel)
        {
            return Ok(_orderService.CalculateOrderSummary(orderSummaryRequestModel));
        }
    }
}
