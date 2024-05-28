using Microsoft.AspNetCore.Mvc.Rendering;

namespace Store.ViewModels
{
    public class DishViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public IFormFile ImageFile { get; set; } = null;
        public string ImagePath { get; set; } = string.Empty;
        public string Description { get; set; }
        public int CategoryId { get; set; }

        // Свойство для списка категорий
        public List<SelectListItem> Categories { get; set; }
    }
}