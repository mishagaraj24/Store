using Microsoft.AspNetCore.Mvc.Rendering;

namespace Store.ViewModels
{
    public class DishCreateViewModel
    {
        public DishViewModel Dish { get; set; }
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IFormFile ImageFile { get; set; }
    }


}
