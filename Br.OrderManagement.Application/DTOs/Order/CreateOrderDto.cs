using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br.OrderManagement.Application.DTOs.Order
{
    public class CreateOrderDto
    {
        public List<OrderItemDto> Items { get; set; } = [];
    }
}
