using Northwind.Business.Interfaces;
using Northwind.Data.Domain.DTO.Product;
using Northwind.Data.Domain.Model;
using Northwind.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Supplier> _supplierRepository;

        public ProductService(IRepository<Product> productRepository,
            IRepository<Category> categoryRepository,
            IRepository<Supplier> supplierRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
        }

        public IEnumerable<ProductViewModel> GetAll()
        {
            var products = _productRepository.GetAll()
                .Select(
                    product => new ProductViewModel
                    {
                        CategoryName = product.Category.CategoryName,
                        SupplierName = product.Supplier.CompanyName,
                        QuantityPerUnit = product.QuantityPerUnit,
                        ReorderLevel = product.ReorderLevel,
                        Discontinued = product.Discontinued,
                        UnitsOnOrder = product.UnitsOnOrder,
                        UnitsInStock = product.UnitsInStock,
                        ProductName = product.ProductName,
                        UnitPrice = product.UnitPrice,
                        ProductId = product.ProductId,
                    });
            return products;
        }

        public IEnumerable<ProductViewModel> GetFirst(int productCount)
        {
            var products = _productRepository.GetAll()
                .Select(
                    product => new ProductViewModel
                    {
                        CategoryName = product.Category.CategoryName,
                        SupplierName = product.Supplier.CompanyName,
                        QuantityPerUnit = product.QuantityPerUnit,
                        ReorderLevel = product.ReorderLevel,
                        Discontinued = product.Discontinued,
                        UnitsOnOrder = product.UnitsOnOrder,
                        UnitsInStock = product.UnitsInStock,
                        ProductName = product.ProductName,
                        UnitPrice = product.UnitPrice,
                        ProductId = product.ProductId,
                    });
            return products.Take(productCount);
        }

        public ProductEditModel GetProductForEdit(int id)
        {
            var product = _productRepository.GetById(id);
            var suppliers = _supplierRepository.GetAll()
                .Select(suplier => suplier.CompanyName);
            var categories = _categoryRepository.GetAll()
                .Select(category => category.CategoryName);

            var productForEdit = new ProductEditModel
            {
                CategoryName = product.Category.CategoryName,
                SupplierName = product.Supplier.CompanyName,
                QuantityPerUnit = product.QuantityPerUnit,
                ReorderLevel = product.ReorderLevel,
                Discontinued = product.Discontinued,
                UnitsOnOrder = product.UnitsOnOrder,
                UnitsInStock = product.UnitsInStock,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                ProductId = product.ProductId,
                OtherCategories = categories,
                OtherSuppliers = suppliers
            };

            return productForEdit;
        }

        public Product CreateProduct(ProductEditModel product)
        {
            var newCategory = _categoryRepository.GetAll()
                .First(category => category.CategoryName == product.CategoryName);
            var newSupplier = _supplierRepository.GetAll()
                .First(supplier => supplier.CompanyName == product.SupplierName);

            var newProduct = new Product
            {
                CategoryId = newCategory.CategoryId,
                Discontinued = product.Discontinued,
                ProductName = product.ProductName,
                QuantityPerUnit = product.QuantityPerUnit,
                ReorderLevel = product.ReorderLevel,
                UnitPrice = product.UnitPrice,
                SupplierId = newSupplier.SupplierId,
                UnitsInStock = product.UnitsInStock,
                UnitsOnOrder = product.UnitsOnOrder,
            };

            var createdProduct = _productRepository.Create(newProduct);
            if  (createdProduct != null)
            {
                return createdProduct;
            }
            else
            {
                throw new Exception("Create error");
            }
        }

        public Product UpdateProduct(ProductEditModel product)
        {
            var updatedProduct = _productRepository.GetById(product.ProductId.Value);
            var newCategory = _categoryRepository.GetAll()
                .First(category => category.CategoryName == product.CategoryName);
            var newSupplier = _supplierRepository.GetAll()
                .First(supplier => supplier.CompanyName == product.SupplierName);

            updatedProduct.CategoryId = newCategory.CategoryId;
            updatedProduct.SupplierId = newSupplier.SupplierId;
         
            updatedProduct.UnitPrice = product.UnitPrice;
            updatedProduct.Discontinued = product.Discontinued;
            updatedProduct.ReorderLevel = product.ReorderLevel;
            updatedProduct.UnitsInStock = product.UnitsInStock;
            updatedProduct.UnitsOnOrder = product.UnitsOnOrder;
            updatedProduct.QuantityPerUnit = product.QuantityPerUnit;

            var newEntity = _productRepository.Update(updatedProduct);
            if (newEntity != null)
            {
                return newEntity;
            }
            else
            {
                throw new Exception("Update error");
            }
        }
    }
}
