using APFoodOrder.Constant;
namespace APFoodOrder.Model
{
    public class CreateOrderRequestModel
    {
        public required int CartId { get; set; }
        public DineInOption DineInOption { get; set; }
    }
}
