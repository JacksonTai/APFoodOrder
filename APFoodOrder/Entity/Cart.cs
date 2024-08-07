﻿
namespace APFoodOrder.Entity
{
    public class Cart
    {
        public int Id { get; set; }
        public required string CustomerId { get; set; }
        public required Customer Customer { get; set; }
        public required ICollection<CartItem> Items { get; set; }

    }
}
