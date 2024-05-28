using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Store.Contracts;
using Store.Models;
using Store.Repository;
using Store.ViewModels;

namespace Store.Controllers
{

    public class DishController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOrderRepository _orderRepository;

        public DishController(ICategoryRepository categoryRepository, IOrderRepository orderRepository, IDishRepository dishRepository, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _dishRepository = dishRepository;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            var viewModel = new DishCreateViewModel
            {
                Dish = new DishViewModel(),
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: /Dish/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DishCreateViewModel viewModel, IFormFile imageFile)
        {
                var dish = _mapper.Map<Dish>(viewModel.Dish);

                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    dish.ImagePath = Path.Combine("/images", uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                }

                await _dishRepository.AddDishAsync(dish);
           
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            viewModel.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(viewModel);
        }

        // GET: /Dish/Index
        public async Task<IActionResult> Index()
        {
            var dishes = await _dishRepository.GetAllDishesAsync();
            var dishViewModels = _mapper.Map<IEnumerable<DishViewModel>>(dishes);
            return View(dishViewModels);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dish = await _dishRepository.GetDishByIdAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllCategoriesAsync();
            var viewModel = new DishViewModel
            {
                Id = dish.Id,
                Name = dish.Name,
                Weight = dish.Weight,
                ImagePath = dish.ImagePath,
                Description = dish.Description,
                CategoryId = dish.CategoryId,
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DishViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return View(viewModel);
            }

            var dish = await _dishRepository.GetDishByIdAsync(viewModel.Id);
            if (dish == null)
            {
                return NotFound();
            }

            dish.Name = viewModel.Name;
            dish.Weight = viewModel.Weight;
            dish.Description = viewModel.Description;
            dish.CategoryId = viewModel.CategoryId;

            if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewModel.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                dish.ImagePath = Path.Combine("/images", uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.ImageFile.CopyToAsync(fileStream);
                }
            }

            await _dishRepository.UpdateDishAsync(dish);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Dish/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _dishRepository.GetDishByIdAsync(id.Value);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: /Dish/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _dishRepository.GetDishByIdAsync(id);
            await _dishRepository.DeleteDishAsync(dish);
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _dishRepository.DishExists(id);
        }

        public async Task<IActionResult> AboutDish(int id)
        {
            var dish = await _dishRepository.GetDishByIdAsync(id);

            if (dish == null)
            {
                return NotFound(); // Если блюдо с указанным id не найдено, возвращаем ошибку 404
            }

            var viewModel = new DishDetailsViewModel
            {
                Id = dish.Id,
                Name = dish.Name,
                Weight = dish.Weight,
                ImagePath = dish.ImagePath,
                Description = dish.Description
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            var dish = await _dishRepository.GetDishByIdAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<DishDetailsViewModel>(dish);
            return View(viewModel);
        }

        // Метод для добавления блюда в заказ
        //[HttpPost]
        //public async Task<IActionResult> Deatails(int dishId)
        //{
           
        //}
    }
}


