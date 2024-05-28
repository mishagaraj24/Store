using Store.Models;

namespace Store.ViewModels
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public Dish Dish { get; set; }
        public int Quantity { get; set; }
    }
}