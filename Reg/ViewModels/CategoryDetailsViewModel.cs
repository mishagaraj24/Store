using Store.Models;

namespace Store.ViewModels
{
    public class CategoryDetailsViewModel
    {
        public string CategoryName { get; set; }
        public List<Dish> Dishes { get; set; } = new List<Dish>();
        public string NoDishesMessage { get; set; }
        public bool ShowCreateDishButton { get; set; }
    }
}
