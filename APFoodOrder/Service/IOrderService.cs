using APFoodOrder.Constant;
using APFoodOrder.Entity;
using APFoodOrder.Model;

namespace APFoodOrder.Service
{
    public interface IOrderService
    {
        Task<CreateOrderResponseModel> CreateOrder(CreateOrderRequestModel createOrderRequestModel);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<List<OrderListViewModel>> GetOrdersByStatusAsync(OrderStatus status, string? customerId);
        Task<Dictionary<OrderStatus, int>> GetOrderCountsAsync(string? customerId);
        OrderSummaryModel CalculateOrderSummary(OrderSummaryRequestModel orderSummaryRequestModel);

    }
}
