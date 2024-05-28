using Store.Models;

namespace Store.Contracts
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<OrderItem> GetOrderItemByIdAsync(int id);
        Task UpdateOrderItemAsync(OrderItem orderItem);
        void SaveChanges();
    }

}