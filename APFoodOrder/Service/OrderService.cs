using APFood.Entity;
using APFoodOrder.Constant;
using APFoodOrder.Constants;
using APFoodOrder.Data;
using APFoodOrder.Entity;
using APFoodOrder.Model;
using Microsoft.EntityFrameworkCore;

namespace APFoodOrder.Service
{
    public class OrderService(
         ApplicationDbContext context
        ) : IOrderService
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<Order> CreateOrder(CreateOrderRequestModel createOrderRequestModel)
        {
            Cart cart = createOrderRequestModel.Cart;
            DineInOption dineInOption = createOrderRequestModel.DineInOption;
            Order order = new()
            {
                CustomerId = cart.CustomerId,
                Customer = cart.Customer,
                Items = cart.Items.Select(ci => new OrderItem
                {
                    FoodId = ci.FoodId,
                    Food = ci.Food,
                    Quantity = ci.Quantity
                }).ToList(),
                Status = OrderStatus.Pending,
                DineInOption = dineInOption,
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            Order order = await _context.Orders.FindAsync(orderId) ?? throw new Exception("Order not found");
            order.Status = newStatus;
            await _context.SaveChangesAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
               .Include(o => o.Items)
               .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<OrderListViewModel>> GetOrdersByStatusAsync(OrderStatus status, string? customerId = null)
        {
            var query = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Food)
                .Where(o => o.Status == status);

            if (customerId != null)
            {
                query = query.Where(o => o.CustomerId == customerId);
            }

            return await query.OrderByDescending(o => o.CreatedAt)
                .Select(o => new OrderListViewModel
                {
                    OrderId = o.Id,
                    OrderTime = o.CreatedAt,
                    QueueNumber = o.QueueNumber,
                    DineInOption = o.DineInOption,
                    OrderStatus = o.Status,
                    TotalPaid = _context.Payments
                        .Where(p => p.OrderId == o.Id)
                        .Select(p => p.Total)
                        .FirstOrDefault(),
                    IsReceivableOrder = IsReceivableOrder(o.Status, _context.DeliveryTasks
                        .Where(dt => dt.OrderId == o.Id)
                        .Select(dt => dt.Status)
                        .FirstOrDefault()),
                    IsCancellableOrder = o.Status == OrderStatus.Pending
                })
                .ToListAsync();
        }

        public async Task<Dictionary<OrderStatus, int>> GetOrderCountsAsync(string? customerId = null)
        {
            Dictionary<OrderStatus, int> orderCounts = Enum.GetValues(typeof(OrderStatus))
              .Cast<OrderStatus>()
              .ToDictionary(status => status, status => 0);

            var query = _context.Orders.AsQueryable();

            if (customerId != null)
            {
                query = query.Where(o => o.CustomerId == customerId);
            }

            var dbCounts = await query
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            foreach (var dbCount in dbCounts)
            {
                if (orderCounts.ContainsKey(dbCount.Status))
                {
                    orderCounts[dbCount.Status] = dbCount.Count;
                }
            }

            return orderCounts;
        }


        public OrderSummaryModel CalculateOrderSummary(OrderSummaryRequestModel orderSummaryRequestModel)
        {
            Cart cart = orderSummaryRequestModel.Cart;
            bool isUsingRunnerPoints = orderSummaryRequestModel.IsUsingRunnerPoints;

            DineInOption dineInOption = orderSummaryRequestModel.DineInOption;
            decimal subtotal = cart.Items.Sum(ci => ci.Food.Price * ci.Quantity);
            decimal deliveryFee = dineInOption == DineInOption.Delivery ? OrderConstant.DeliveryFee : 0;
            decimal runnerPointsRedeemed = isUsingRunnerPoints ? Math.Min(cart.Customer.Points, deliveryFee + subtotal) : 0;
            decimal total = Math.Max(subtotal + deliveryFee - runnerPointsRedeemed, 0);
            return new OrderSummaryModel
            {
                Subtotal = subtotal,
                DeliveryFee = deliveryFee,
                RunnerPointsRedeemed = runnerPointsRedeemed,
                Total = total
            };
        }
   
        private static bool IsReceivableOrder(OrderStatus orderStatus, DeliveryStatus? deliveryStatus)
        {
            return orderStatus == OrderStatus.Ready &&
                   (deliveryStatus != null && deliveryStatus == DeliveryStatus.Delivered);
        }
     
    }
}
