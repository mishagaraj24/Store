namespace Store.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserViewModel User { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
        public decimal TotalPrice { get; set; }
    }
}