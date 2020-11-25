using Northwind.Data.Domain.DTO.Product;
using Northwind.Data.Domain.Model;
using System.Collections.Generic;

namespace Northwind.Business.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductViewModel> GetAll();

        IEnumerable<ProductViewModel> GetFirst(int productCount);

        ProductEditModel GetProductForEdit(int id);

        Product Update(ProductEditModel product);

        Product Create(ProductEditModel product);

        bool Delete(int id);
    }
}
