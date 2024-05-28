namespace Store.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; } = string.Empty;

        // Навигационное свойство для списка блюд, принадлежащих данной категории
        public IEnumerable<Dish> Dishes { get; set; } = new List<Dish>();

    }
}