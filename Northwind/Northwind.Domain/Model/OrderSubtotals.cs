using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Data.Domain.Model
{
    public partial class OrderSubtotals
    {
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Column(TypeName = "money")]
        public decimal? Subtotal { get; set; }
    }
}
