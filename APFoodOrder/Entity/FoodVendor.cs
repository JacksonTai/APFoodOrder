using System.ComponentModel.DataAnnotations;

namespace APFoodOrder.Entity;

public class FoodVendor : APFoodUser
{

    [Required]
    public required string StoreName { get; set; }

}

