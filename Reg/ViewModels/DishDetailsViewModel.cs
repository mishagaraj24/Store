namespace Store.ViewModels
{
    public class DishDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float? Weight { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; } = 1; // Default quantity is 1
    }
}
