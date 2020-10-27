﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Data.Domain.Model
{
    [Table("Order Details")]
    public partial class OrderDetail
    {
        [Key]
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Key]
        [Column("ProductID")]
        public int ProductId { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }

        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(Model.Order.OrderDetails))]
        public virtual Order Order { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(Model.Product.OrderDetails))]
        public virtual Product Product { get; set; }
    }
}