using FluentAssertions;
using Moq;
using Northwind.Business.Interfaces;
using Northwind.Business.Services;
using Northwind.Data.Domain.DTO.Product;
using Northwind.Data.Domain.Model;
using Northwind.DataAccess.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.UnitTest.Services
{
    class ProductServiceTest
    {
        private IProductService _productService;
        private Mock<IRepository<Product>> _moqProductRepository;
        private Mock<IRepository<Category>> _moqCategoryRepository;
        private Mock<IRepository<Supplier>> _moqSupplierRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _moqProductRepository = new Mock<IRepository<Product>>();
            _moqCategoryRepository = new Mock<IRepository<Category>>();
            _moqSupplierRepository = new Mock<IRepository<Supplier>>();
        }

        [Test]
        public void GetAll_WhereProductExists_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product
                { 
                    CategoryId = 0,
                    Discontinued = true,
                    ProductId = 0,
                    ProductName = "product0",
                    QuantityPerUnit = "quantiry0",
                    Category = new Category { CategoryId = 0, CategoryName = "category0", Description = "desc0"},
                    Supplier = new Supplier { SupplierId = 0, CompanyName = "supplie0"}
                },
                new Product
                {
                    CategoryId = 1,
                    Discontinued = true,
                    ProductId = 1,
                    ProductName = "product1",
                    QuantityPerUnit = "quantiry1",
                    Category = new Category { CategoryId = 1, CategoryName = "category1", Description = "desc1"},
                    Supplier = new Supplier { SupplierId = 1, CompanyName = "supplie1"}
                },
                new Product
                {
                    CategoryId = 2,
                    Discontinued = true,
                    ProductId = 2,
                    ProductName = "product2",
                    QuantityPerUnit = "quantiry2",
                    Category = new Category { CategoryId = 2, CategoryName = "category2", Description = "desc2"},
                    Supplier = new Supplier { SupplierId = 2, CompanyName = "supplie2"}
                },
            };
            var productViewModels = new List<ProductViewModel>
            {
                new ProductViewModel { Discontinued = true, ProductId = 0, ProductName = "product0", QuantityPerUnit = "quantiry0", CategoryName = "category0", SupplierName = "supplie0" },
                new ProductViewModel { Discontinued = true, ProductId = 1, ProductName = "product1", QuantityPerUnit = "quantiry1", CategoryName = "category1", SupplierName = "supplie1" },
                new ProductViewModel { Discontinued = true, ProductId = 2, ProductName = "product2", QuantityPerUnit = "quantiry2", CategoryName = "category2", SupplierName = "supplie2" },
            };

            _moqProductRepository.Setup(x => x.GetAll()).Returns(products.AsQueryable());
            _productService = new ProductService(_moqProductRepository.Object,
                _moqCategoryRepository.Object, _moqSupplierRepository.Object);

            // Act
            var result = _productService.GetAll();

            // Assert
            result.Should().BeEquivalentTo(productViewModels);
        }


        [Test]
        public void GetFirstTwo_WhereProductExists_ShouldReturnTwoProducts()
        {
            // Arrange
            var productCount = 2;
            var products = new List<Product>
            {
                new Product
                {
                    CategoryId = 0,
                    Discontinued = true,
                    ProductId = 0,
                    ProductName = "product0",
                    QuantityPerUnit = "quantiry0",
                    Category = new Category { CategoryId = 0, CategoryName = "category0", Description = "desc0"},
                    Supplier = new Supplier { SupplierId = 0, CompanyName = "supplie0"}
                },
                new Product
                {
                    CategoryId = 1,
                    Discontinued = true,
                    ProductId = 1,
                    ProductName = "product1",
                    QuantityPerUnit = "quantiry1",
                    Category = new Category { CategoryId = 1, CategoryName = "category1", Description = "desc1"},
                    Supplier = new Supplier { SupplierId = 1, CompanyName = "supplie1"}
                },
                new Product
                {
                    CategoryId = 2,
                    Discontinued = true,
                    ProductId = 2,
                    ProductName = "product2",
                    QuantityPerUnit = "quantiry2",
                    Category = new Category { CategoryId = 2, CategoryName = "category2", Description = "desc2"},
                    Supplier = new Supplier { SupplierId = 2, CompanyName = "supplie2"}
                },
            };
            var productViewModels = new List<ProductViewModel>
            {
                new ProductViewModel { Discontinued = true, ProductId = 0, ProductName = "product0", QuantityPerUnit = "quantiry0", CategoryName = "category0", SupplierName = "supplie0" },
                new ProductViewModel { Discontinued = true, ProductId = 1, ProductName = "product1", QuantityPerUnit = "quantiry1", CategoryName = "category1", SupplierName = "supplie1" },
            };

            _moqProductRepository.Setup(x => x.GetAll()).Returns(products.AsQueryable());
            _productService = new ProductService(_moqProductRepository.Object,
                _moqCategoryRepository.Object, _moqSupplierRepository.Object);

            // Act
            var result = _productService.GetFirst(productCount);

            // Assert
            result.Should().BeEquivalentTo(productViewModels);
        }

        [Test]
        public void GetProductForEdit_WhereProductExists_ShouldReturnProductForEdit()
        {
            // Arrange
            var productId = 1;
            var product = new Product
            {
                CategoryId = 1,
                Discontinued = true,
                ProductId = productId,
                ProductName = "product1",
                QuantityPerUnit = "quantity1",
                Category = new Category { CategoryId = 1, CategoryName = "category1", Description = "desc1" },
                Supplier = new Supplier { SupplierId = 1, CompanyName = "supplie1" }
            };

            var suppliers = new List<Supplier>
            {
                new Supplier { SupplierId = 0, CompanyName = "supplier0" },
                new Supplier { SupplierId = 1, CompanyName = "supplier1" },
                new Supplier { SupplierId = 2, CompanyName = "supplier2" },
                new Supplier { SupplierId = 3, CompanyName = "supplier3" },
            };

            var categories = new List<Category> 
            {
                new Category { CategoryId = 0, CategoryName = "category0"},
                new Category { CategoryId = 1, CategoryName = "category1"},
                new Category { CategoryId = 2, CategoryName = "category2"},
            };

            var productForEdit = new ProductEditModel
            {
                CategoryName = "category1",
                Discontinued = true,
                ProductId = productId,
                ProductName = "product1",
                QuantityPerUnit = "quantity1",
                SupplierName = "supplie1",
                OtherCategories = new string[] { "category0", "category1", "category2" },
                OtherSuppliers = new string[] { "supplier0", "supplier1", "supplier2", "supplier3" }
            };

            _moqProductRepository.Setup(x => x.GetById(productId)).Returns(product);
            _moqSupplierRepository.Setup(x => x.GetAll()).Returns(suppliers.AsQueryable());
            _moqCategoryRepository.Setup(x => x.GetAll()).Returns(categories.AsQueryable());

            _productService = new ProductService(_moqProductRepository.Object,
                _moqCategoryRepository.Object, _moqSupplierRepository.Object);

            // Act
            var result = _productService.GetProductForEdit(productId);

            // Assert
            result.Should().BeEquivalentTo(productForEdit);
        }

        [Test]
        public void CreateProduct_WhereProductExists_ShouldReturnCreatedProduct()
        {
            // Arrange
            var productId = 1;
            var product = new Product
            {
                ProductId = 0,
                CategoryId = 1,
                SupplierId = 1,
                Discontinued = true,
                ProductName = "product1",
                QuantityPerUnit = "quantity1",
            };

            var suppliers = new List<Supplier>
            {
                new Supplier { SupplierId = 0, CompanyName = "supplier0" },
                new Supplier { SupplierId = 1, CompanyName = "supplier1" },
                new Supplier { SupplierId = 2, CompanyName = "supplier2" },
                new Supplier { SupplierId = 3, CompanyName = "supplier3" },
            };

            var categories = new List<Category>
            {
                new Category { CategoryId = 0, CategoryName = "category0"},
                new Category { CategoryId = 1, CategoryName = "category1"},
                new Category { CategoryId = 2, CategoryName = "category2"},
            };

            var productForEdit = new ProductEditModel
            {
                CategoryName = "category1",
                Discontinued = true,
                ProductName = "product1",
                QuantityPerUnit = "quantity1",
                SupplierName = "supplier1",
                OtherCategories = new string[] { "category0", "category1", "category2" },
                OtherSuppliers = new string[] { "supplier0", "supplier1", "supplier2", "supplier3" }
            };

            _moqProductRepository.Setup(x => x.Create(It.IsAny<Product>())).Returns(product);
            _moqSupplierRepository.Setup(x => x.GetAll()).Returns(suppliers.AsQueryable());
            _moqCategoryRepository.Setup(x => x.GetAll()).Returns(categories.AsQueryable());

            _productService = new ProductService(_moqProductRepository.Object,
                _moqCategoryRepository.Object, _moqSupplierRepository.Object);

            // Act
            var result = _productService.CreateProduct(productForEdit);

            // Assert
            result.Should().BeEquivalentTo(product);
        }

        [Test]
        public void UpdateProduct_WhereProductExists_ShouldReturnUpdatedProduct()
        {
            // Arrange
            var productId = 1;
            var product = new Product
            {
                ProductId = productId,
                CategoryId = 1,
                SupplierId = 1,
                Discontinued = true,
                ProductName = "product1",
                QuantityPerUnit = "quantity1",
            };

            var suppliers = new List<Supplier>
            {
                new Supplier { SupplierId = 0, CompanyName = "supplier0" },
                new Supplier { SupplierId = 1, CompanyName = "supplier1" },
                new Supplier { SupplierId = 2, CompanyName = "supplier2" },
                new Supplier { SupplierId = 3, CompanyName = "supplier3" },
            };

            var categories = new List<Category>
            {
                new Category { CategoryId = 0, CategoryName = "category0"},
                new Category { CategoryId = 1, CategoryName = "category1"},
                new Category { CategoryId = 2, CategoryName = "category2"},
            };

            var productForEdit = new ProductEditModel
            {
                ProductId = productId,
                CategoryName = "category1",
                Discontinued = true,
                ProductName = "product1",
                QuantityPerUnit = "quantity1",
                SupplierName = "supplier1",
                OtherCategories = new string[] { "category0", "category1", "category2" },
                OtherSuppliers = new string[] { "supplier0", "supplier1", "supplier2", "supplier3" }
            };

            _moqProductRepository.Setup(x => x.Update(It.IsAny<Product>())).Returns(product);
            _moqProductRepository.Setup(x => x.GetById(productId)).Returns(product);
            _moqSupplierRepository.Setup(x => x.GetAll()).Returns(suppliers.AsQueryable());
            _moqCategoryRepository.Setup(x => x.GetAll()).Returns(categories.AsQueryable());

            _productService = new ProductService(_moqProductRepository.Object,
                _moqCategoryRepository.Object, _moqSupplierRepository.Object);

            // Act
            var result = _productService.UpdateProduct(productForEdit);

            // Assert
            result.Should().BeEquivalentTo(product);
        }
    }
}
