using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Business.Interfaces;
using Northwind.Data.Domain.DTO.Product;
using Serilog;

namespace Northwind.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;
        private readonly ILogger _logger;
        private readonly int productsPerPage;

        public ProductController(IProductService productService,
            ICategoryService categoryService,
            ISupplierService supplierService,
            IConfiguration configuration,
            ILogger logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _logger = logger;
            productsPerPage = configuration.GetValue<int>("ProductsPerPage");
        }

        public IActionResult Products()
        {
            IEnumerable<ProductViewModel> products;
            if (productsPerPage == 0)
            {
                products = _productService.GetAll();
            }
            else
            {
                products = _productService.GetFirst(productsPerPage);
            }
            return View(products);
        }

        public IActionResult Update(int id)
        {
            var product = _productService.GetProductForEdit(id);
            _logger.Information("Displaing product with Id:" + id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Update(ProductEditModel product)
        {
            if (ModelState.IsValid)
            {
                var updatedProduct = _productService.UpdateProduct(product);
                _logger.Information($"Product [Id: {updatedProduct.ProductId}, Name: {updatedProduct.ProductName}] was updated");
                return RedirectToAction("Products");
            }

            _logger.Warning("Model has incorrect form");
            return View();
        }

        public IActionResult Create()
        {
            var emptyProduct = new ProductEditModel
            {
                OtherCategories = _categoryService.GetAll().Select(category => category.CategoryName),
                OtherSuppliers = _supplierService.GetAll().Select(supplier => supplier.CompanyName),
            };

            return View("Update", emptyProduct);
        }

        [HttpPost]
        public IActionResult Create(ProductEditModel product)
        {
            if (ModelState.IsValid)
            {
                var createdProduct = _productService.CreateProduct(product);

                _logger.Information($"Product [Id: {createdProduct.ProductId}, Name: {createdProduct.ProductName}] was created");
                return RedirectToAction("Products");
            }

            _logger.Information("Model has incorrect form");
            return View();
        }
    }
}
