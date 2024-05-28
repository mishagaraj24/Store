using AutoMapper;
using Store.Models;
using Store.ViewModels;

namespace Store
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Dish, DishViewModel>().ReverseMap();

            //CreateMap<Dish, DishDetailsViewModel>();
            CreateMap<Order, OrderViewModel>()
                       .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.OrderItems.Sum(item => item.Quantity * item.Dish.Price)));
            CreateMap<OrderItem, OrderItemViewModel>();
            CreateMap<User, UserViewModel>();
        }
    }
}
