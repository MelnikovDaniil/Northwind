using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Api.Models;
using Northwind.Business.Interfaces;
using Northwind.Data.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Northwind.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IEnumerable<Category> GetCategories()
        {
            var categories = _categoryService.GetAll();
            return categories;
        }

        [HttpPut]
        public IActionResult Create([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest(new ApiResponse("Body is empty"));
            }

            var cretedCategory = _categoryService.Create(category);
            if (cretedCategory != null)
            {
                return Ok(new ApiResponse { Data = cretedCategory });
            }
            return NotFound(new ApiResponse("Failed to create category"));
        }

        [HttpPost("{id}")]
        public IActionResult Update(int id, [FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest(new ApiResponse("Body is empty"));
            }

            var updatedCategory = _categoryService.Update(id, category);
            if (updatedCategory != null)
            {
                return Ok(new ApiResponse { Data = updatedCategory });
            }
            return NotFound(new ApiResponse("Category not found"));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_categoryService.Delete(id))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("/api/image/{id}")]
        public FileStreamResult GetCategoryImage(int id)
        {
            var byteImage = _categoryService.GetCategoryImage(id);
            var stream = new MemoryStream(byteImage);
            return File(stream, "image/bmp");
        }

        [HttpPost("/api/image/{id}")]
        public IActionResult UploadImage(int id, IFormFile categoryImage)
        {
            if (categoryImage == null)
            {
                BadRequest("File error: Invalid file");
            }

            var ext = Path.GetExtension(categoryImage.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                BadRequest("File error: Invalid file extension");
            }

            using (var ms = new MemoryStream())
            {
                categoryImage.CopyTo(ms);
                var fileBytes = ms.ToArray();
                _categoryService.UpdateCategoryImage(fileBytes, id);

                var stream = new MemoryStream(fileBytes);
                return File(stream, "image/bmp");
            }
        }
    }
}
