using Br.OrderManagement.Domain.Common;
using Br.OrderManagement.Domain.Enums;
using Br.OrderManagement.Domain.Events;
using Br.OrderManagement.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br.OrderManagement.Domain.Entities
{
    public class Order : AggregateRoot
    {
        private readonly List<OrderItem> _items = [];

        protected Order()
        {
        }

        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public OrderStatus Status { get; private set; }

        public decimal TotalAmount { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public static Order Create()
        {
            return new Order
            {
                Status = OrderStatus.Created,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void AddItem(Product product, int quantity)
        {
            ValidateCanChange();

            if (product.StockQuantity < quantity)
                throw new DomainException("Estoque insuficiente.");

            var item = new OrderItem(product, quantity);

            _items.Add(item);

            CalculateTotal();
        }

        public void RemoveItem(Guid productId)
        {
            ValidateCanChange();

            var item = _items.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
                throw new DomainException("Item não encontrado.");

            _items.Remove(item);

            CalculateTotal();
        }

        public void Confirm()
        {
            if (Status != OrderStatus.Created)
                throw new DomainException("Somente pedidos criados podem ser confirmados.");

            if (!_items.Any())
                throw new DomainException("O pedido deve possuir pelo menos um item.");

            Status = OrderStatus.Confirmed;

            AddDomainEvent(new OrderConfirmedEvent(Id));
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Finished)
                throw new DomainException("Pedido finalizado não pode ser cancelado.");

            if (Status == OrderStatus.Canceled)
                throw new DomainException("Pedido já cancelado.");

            if (Status == OrderStatus.Confirmed)
            {
                AddDomainEvent(new OrderCanceledEvent(Id));
            }

            Status = OrderStatus.Canceled;
        }

        public void Finish()
        {
            if (Status != OrderStatus.Confirmed)
                throw new DomainException("Somente pedidos confirmados podem ser finalizados.");

            Status = OrderStatus.Finished;
        }

        private void CalculateTotal()
        {
            TotalAmount = _items.Sum(x => x.TotalPrice);
        }

        private void ValidateCanChange()
        {
            if (Status == OrderStatus.Finished)
                throw new DomainException("Pedido finalizado não pode ser alterado.");

            if (Status == OrderStatus.Canceled)
                throw new DomainException("Pedido cancelado não pode ser alterado.");
        }
    }
}
