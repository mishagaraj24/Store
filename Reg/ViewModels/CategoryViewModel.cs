using Store.Models;

namespace Store.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public IEnumerable<DishViewModel> Dishes { get; set; } = new List<DishViewModel>();
    }
}