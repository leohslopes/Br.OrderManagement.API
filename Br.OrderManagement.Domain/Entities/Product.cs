using Br.OrderManagement.Domain.Common;
using Br.OrderManagement.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br.OrderManagement.Domain.Entities
{
    public class Product : BaseEntity
    {
        protected Product()
        {
        }

        public Product(
            string name,
            string description,
            decimal price,
            int stockQuantity,
            byte[]? image)
        {
            SetName(name);
            SetDescription(description);
            SetPrice(price);
            SetStock(stockQuantity);

            CreatedAt = DateTime.UtcNow;
            Image = image;

        }

        public string Name { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public decimal Price { get; private set; }

        public int StockQuantity { get; private set; }

        public byte[]? Image { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public void Update(string name, string description, decimal price)
        {
            SetName(name);
            SetDescription(description);
            SetPrice(price);
        }

        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("A quantidade deve ser maior que zero.");

            if (StockQuantity < quantity)
                throw new DomainException("Estoque insuficiente.");

            StockQuantity -= quantity;
        }

        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("A quantidade deve ser maior que zero.");

            StockQuantity += quantity;
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Nome do produto é obrigatório.");

            Name = name.Trim();
        }

        private void SetDescription(string description)
        {
            Description = description?.Trim() ?? string.Empty;
        }

        private void SetPrice(decimal price)
        {
            if (price <= 0)
                throw new DomainException("Preço inválido.");

            Price = price;
        }

        private void SetStock(int stock)
        {
            if (stock < 0)
                throw new DomainException("Quantidade em estoque inválida.");

            StockQuantity = stock;
        }

        public void SetImage(byte[]? image)
        {
            Image = image;
        }
    }
}
