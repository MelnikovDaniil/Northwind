﻿using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Interfaces;
using Northwind.Core.Filters;
using Northwind.Data.Domain.DTO.Category;
using System.IO;
using System.Linq;

namespace Northwind.Controllers
{
    [ServiceFilter(typeof(LoggingFilter))]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Route("Category")]
        public IActionResult Index()
        {
            var categories = _categoryService.GetAll();
            return View(categories);
        }

        [Route("images/{categoryId}")]
        [ResponseCache(Duration = 300)]
        public FileStreamResult GetCategoryImage(int categoryId)
        {
            var byteImage = _categoryService.GetCategoryImage(categoryId);
            var stream = new MemoryStream(byteImage);
            return File(stream, "image/bmp");
        }

        [HttpGet]
        public IActionResult UploadImage(int categoryId)
        {
            var model = new CategoryImageViewModel { CategoryId = categoryId };
            return View(model);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 5242880)]
        public IActionResult UploadImage(CategoryImageViewModel categoryImage)
        {
            var ext = Path.GetExtension(categoryImage.FormFile.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                ModelState.AddModelError("File error", "Invalid file extension");
            }

            if (ModelState.IsValid)
            {
                using (var ms = new MemoryStream())
                {
                    categoryImage.FormFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    _categoryService.UpdateCategoryImage(fileBytes, categoryImage.CategoryId);
                }
                return RedirectToAction("Categories");
            }
            else
            {
                return View(categoryImage);
            }

        }
    }
}
