using Br.OrderManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br.OrderManagement.Application.DTOs.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderItemDto> Items { get; set; } = [];
    }
}
