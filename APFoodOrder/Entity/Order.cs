﻿
using APFoodOrder.Constant;
using APFoodOrder.Entity;

namespace APFood.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public required string CustomerId { get; set; }
        public required Customer Customer { get; set; }
        public Payment? Payment { get; set; }
        public required List<OrderItem> Items { get; set; }
        public required OrderStatus Status { get; set; } = OrderStatus.Pending;
        public required DineInOption DineInOption { get; set; } = DineInOption.Pickup;
        public int QueueNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
