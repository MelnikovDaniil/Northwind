using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Interfaces;

namespace Northwind.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Categories()
        {
            var categories = _categoryService.GetAll();
            return View(categories);
        }
    }
}
