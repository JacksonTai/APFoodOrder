﻿using APFoodOrder.Constant;
using System.ComponentModel.DataAnnotations.Schema;

namespace APFoodOrder.Entity
{
    public class Food
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? Category { get; set; }
        public FoodStatus? Status { get; set; }

        [ForeignKey("FoodVendorId")]
        public string? FoodVendorId { get; set; }

        public FoodVendor? FoodVendor { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }


    }
}
