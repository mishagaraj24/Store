using Store.Models;

namespace Store.ViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<Dish> Dishes { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
