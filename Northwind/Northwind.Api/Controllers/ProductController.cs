using Microsoft.AspNetCore.Mvc;
using Northwind.Api.Models;
using Northwind.Business.Interfaces;
using Northwind.Data.Domain.DTO.Product;
using System.Collections.Generic;

namespace Northwind.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IEnumerable<ProductViewModel> GetCategories()
        {
            var product = _productService.GetAll();
            return product;
        }

        [HttpPut]
        public IActionResult Create([FromBody] ProductEditModel product)
        {
            if (product == null)
            {
                return BadRequest(new ApiResponse("Body is empty"));
            }

            var cretedCategory = _productService.Create(product);
            if (cretedCategory != null)
            {
                return Ok(new ApiResponse { Data = cretedCategory });
            }
            return NotFound(new ApiResponse("Failed to create category"));
        }

        [HttpPost("{id}")]
        public IActionResult Update(int id, [FromBody] ProductEditModel product)
        {
            if (product == null)
            {
                return BadRequest(new ApiResponse("Body is empty"));
            }
            product.ProductId = id;

            var updatedProduct = _productService.Update(product);
            if (updatedProduct != null)
            {
                return Ok(new ApiResponse { Data = updatedProduct });
            }
            return NotFound(new ApiResponse("Category not found"));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_productService.Delete(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
