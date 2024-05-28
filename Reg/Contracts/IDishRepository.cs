using Store.Models;

namespace Store.Contracts
{
    public interface IDishRepository
    {
        Task<List<Dish>> GetAllDishesAsync();
        Task<Dish> GetDishByIdAsync(int id);
        Task AddDishAsync(Dish dish);
        Task UpdateDishAsync(Dish dish);
        Task DeleteDishAsync(Dish dish);
        bool DishExists(int id);
        Task<IEnumerable<Dish>> GetDishesByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Dish>> SearchByNameAsync(string searchTerm);
    }

}