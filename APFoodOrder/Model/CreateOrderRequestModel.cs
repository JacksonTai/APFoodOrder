using APFoodOrder.Constant;
using APFoodOrder.Entity;

namespace APFoodOrder.Model
{
    public class CreateOrderRequestModel
    {
        public Cart Cart { get; set; }
        public DineInOption DineInOption { get; set; }
    }
}
