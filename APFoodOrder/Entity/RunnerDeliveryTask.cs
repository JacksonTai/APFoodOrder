﻿
using APFoodOrder.Constants;
using APFoodOrder.Entity;

namespace APFood.Entity
{
    public class RunnerDeliveryTask
    {
        public required string RunnerId { get; set; }
        public required int DeliveryTaskId { get; set; }
        public required DeliveryStatus Status { get; set; } = DeliveryStatus.Accepted;

        public Customer? Runner { get; set; }
        public DeliveryTask? DeliveryTask { get; set; }
    }
}
