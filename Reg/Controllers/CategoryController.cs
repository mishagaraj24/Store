using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Contracts;
using Store.Models;
using Store.ViewModels;

namespace Store.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        public CategoryController(IDishRepository dishRepository, ICategoryRepository categoryRepository, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _dishRepository = dishRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize(Policy = "AdminOnly")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel categoryViewModel, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(categoryViewModel);

                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    category.ImagePath = Path.Combine("/images", uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                }

                category.Dishes = new List<Dish>();

                await _categoryRepository.AddCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(categoryViewModel);
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return View(categories);
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            var categoryViewModel = _mapper.Map<CategoryViewModel>(category);
            return View(categoryViewModel);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel categoryViewModel, IFormFile imageFile)
        {
            if (id != categoryViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var category = await _categoryRepository.GetCategoryByIdAsync(id);

                    _mapper.Map(categoryViewModel, category);

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        category.ImagePath = Path.Combine("/images", uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }
                    }

                    await _categoryRepository.UpdateCategoryAsync(category);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(categoryViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(categoryViewModel);
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            await _categoryRepository.DeleteCategoryAsync(category);
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _categoryRepository.CategoryExists(id);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var dishes = await _dishRepository.GetDishesByCategoryIdAsync(id);

            var viewModel = new CategoryDetailsViewModel
            {
                CategoryName = category.Name,
                Dishes = dishes.Select(d => new Dish
                {
                    Id = d.Id,
                    Name = d.Name,
                    ImagePath = d.ImagePath,
                }).ToList()
            };


            if (viewModel.Dishes.Count == 0)
            {
                viewModel.NoDishesMessage = "No dishes available for this category.";
                viewModel.ShowCreateDishButton = true;
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new SearchViewModel());
            }

            var dishes = await _dishRepository.SearchByNameAsync(query);
            var categories = await _categoryRepository.SearchByNameAsync(query);

            var viewModel = new SearchViewModel
            {
                Dishes = dishes,
                Categories = categories
            };

            return View("SearchResults", viewModel);
        }
    }
}
