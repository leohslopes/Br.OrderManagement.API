using Br.OrderManagement.Domain.Common;
using Br.OrderManagement.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br.OrderManagement.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        protected OrderItem()
        {
        }

        public OrderItem(Product product, int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantidade inválida.");

            ProductId = product.Id;
            Product = product;

            Quantity = quantity;
            UnitPrice = product.Price;

            TotalPrice = quantity * product.Price;
        }

        public Guid OrderId { get; private set; }

        public Guid ProductId { get; private set; }

        public Product Product { get; private set; } = default!;

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal TotalPrice { get; private set; }
    }
}
