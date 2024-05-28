using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Contracts;
using Store.Models;

namespace Store.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<User> _userManager;
        private readonly IDishRepository _dishRepository;

        public OrderController(IOrderRepository orderRepository, IDishRepository dishRepository, UserManager<User> userManager)
        {
            _orderRepository = orderRepository;
            _dishRepository = dishRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddToOrder(int dishId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var dish = await _dishRepository.GetDishByIdAsync(dishId);
            if (dish == null)
            {
                return NotFound();
            }

            // Получаем все заказы пользователя, включая элементы заказа
            var orders = await _orderRepository.GetOrdersByUserIdAsync(user.Id);
            var order = orders.FirstOrDefault(o => !o.IsCompleted);

            if (order == null)
            {
                order = new Order
                {
                    UserId = user.Id,
                    OrderDate = DateTime.Now,
                    IsCompleted = false,
                    OrderItems = new List<OrderItem>()
                };
                await _orderRepository.AddOrderAsync(order);
            }

            var orderItem = order.OrderItems.FirstOrDefault(oi => oi.DishId == dishId);
            if (orderItem == null)
            {
                orderItem = new OrderItem
                {
                    DishId = dishId,
                    Quantity = 1,
                    Price = dish.Price
                };
                order.OrderItems.Add(orderItem);
            }
            else
            {
                orderItem.Quantity++;
            }

            _orderRepository.SaveChanges(); // Сохранить изменения

            return RedirectToAction("Details", "Order", new { id = order.Id });
        }



        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || (order.UserId != user.Id && !User.IsInRole("admin")))
            {
                return Forbid();
            }

            return View(order);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AllOrders()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return View(orders);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> ToggleCompletion(int orderId, bool isCompleted)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                order.IsCompleted = isCompleted;
                await _orderRepository.UpdateOrderAsync(order);
                return RedirectToAction("AllOrders");
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int orderId)
        {
            await _orderRepository.DeleteOrderAsync(orderId);
            return RedirectToAction("AllOrders");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int orderItemId, int quantity)
        {
            var orderItem = await _orderRepository.GetOrderItemByIdAsync(orderItemId);
            if (orderItem != null)
            {
                orderItem.Quantity = quantity;
                await _orderRepository.UpdateOrderItemAsync(orderItem);
            }
            return RedirectToAction("Details", new { id = orderItem.OrderId });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CompleteOrder(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order != null)
            {
                order.IsCompleted = true;
                await _orderRepository.UpdateOrderAsync(order);
                return RedirectToAction("AllOrders");
            }
            return NotFound();
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        public async Task<IActionResult> SendOrderForProcessing(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order != null)
            {
            
                await _orderRepository.UpdateOrderAsync(order);
                return RedirectToAction("OrderDetails", new { id = id });
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int id, bool isCompleted)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order != null)
            {
                order.IsCompleted = isCompleted;
                await _orderRepository.UpdateOrderAsync(order);
            }
            return RedirectToAction("Details", new { id });
        }
    }
}
