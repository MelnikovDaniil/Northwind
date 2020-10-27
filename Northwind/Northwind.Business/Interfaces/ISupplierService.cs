using Northwind.Data.Domain.Model;
using System.Collections.Generic;

namespace Northwind.Business.Interfaces
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> GetAll();
    }
}
