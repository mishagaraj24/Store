namespace Store.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsCompleted { get; set; } = false;

        // Внешний ключ для пользователя
        public string UserId { get; set; }
        public User User { get; set; }

        // Коллекция элементов заказа
        public ICollection<OrderItem> OrderItems { get; set; }
    }

}