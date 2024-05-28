namespace Store.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Внешний ключ для блюда
        public int DishId { get; set; }
        public Dish Dish { get; set; }

        // Внешний ключ для заказа
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }

}
