namespace Store.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public float Weight { get; set; }

        public string ImagePath { get; set; }

        // Внешний ключ для категории
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Коллекция элементов заказа
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
