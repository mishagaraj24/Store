using Microsoft.EntityFrameworkCore;
using Store.Contracts;
using Store.Models;

namespace Store.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Context _context;
        private readonly object _lock = new object();

        public OrderRepository(Context context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            lock (_lock)
            {
                return _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Dish)
                    .FirstOrDefault(o => o.Id == id);
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            IQueryable<Order> query;
            lock (_lock)
            {
                query = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Dish);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            IQueryable<Order> query;
            lock (_lock)
            {
                query = _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Dish)
                    .Where(o => o.UserId == userId);
            }
            return await query.ToListAsync();
        }


        public async Task AddOrderAsync(Order order)
        {
            lock (_lock)
            {
                _context.Orders.Add(order);

                // Добавляем все элементы заказа в контекст данных, если они не добавлены
                foreach (var orderItem in order.OrderItems)
                {
                    if (_context.OrderItems.Find(orderItem.Id) == null)
                    {
                        _context.OrderItems.Add(orderItem);
                    }
                }

                SaveChanges();
            }
        }


        public async Task UpdateOrderAsync(Order order)
        {
            lock (_lock)
            {
                _context.Orders.Update(order);
                
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            lock (_lock)
            {
                var order =  _context.Orders.Where(x => x.Id == id).FirstOrDefault();    
                if (order != null)
                {
                    _context.Orders.Remove(order);
                    SaveChanges();
                }
            }
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(int id)
        {
            lock (_lock)
            {
                return _context.OrderItems
                    .Include(oi => oi.Dish)
                    .FirstOrDefault(oi => oi.Id == id);
            }
        }

        public async Task UpdateOrderItemAsync(OrderItem orderItem)
        {
            lock (_lock)
            {
                _context.OrderItems.Update(orderItem);
                SaveChanges();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
