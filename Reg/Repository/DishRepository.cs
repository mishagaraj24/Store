using Microsoft.EntityFrameworkCore;
using Store.Contracts;
using Store.Models;

namespace Store.Repository
{
    public class DishRepository : IDishRepository
    {
        private readonly Context _context;

        public DishRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<Dish>> GetAllDishesAsync()
        {
            return await _context.Dishes.ToListAsync();
        }

        public async Task<Dish> GetDishByIdAsync(int id)
        {
            return await _context.Dishes.FindAsync(id);
        }

        public async Task AddDishAsync(Dish dish)
        {
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDishAsync(Dish dish)
        {
            _context.Entry(dish).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDishAsync(Dish dish)
        {
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
        }

        public bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
        public async Task<IEnumerable<Dish>> GetDishesByCategoryIdAsync(int categoryId)
        {
            return await _context.Dishes.Where(d => d.CategoryId == categoryId).ToListAsync();
        }
        public async Task<IEnumerable<Dish>> SearchByNameAsync(string searchTerm)
        {
            return await _context.Dishes.Where(d => d.Name.Contains(searchTerm)).ToListAsync();
        }

    }

}
