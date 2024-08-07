using APFoodOrder.Constant;
using APFoodOrder.Entity;

namespace APFoodOrder.Model
{
    public class OrderSummaryRequestModel
    {
       public Cart Cart { get; set; }
       public DineInOption DineInOption { get; set; }
       public bool IsUsingRunnerPoints { get; set; }
    }
}
