using Store.Models;

namespace Store.Contracts
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
        bool CategoryExists(int id);
        Task<IEnumerable<Category>> SearchByNameAsync(string searchTerm);
    }
}