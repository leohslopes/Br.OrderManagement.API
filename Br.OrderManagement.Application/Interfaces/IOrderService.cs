using Br.OrderManagement.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br.OrderManagement.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();

        Task<OrderDto?> GetByIdAsync(Guid id);

        Task<Guid> CreateAsync(CreateOrderDto dto);

        Task ConfirmAsync(Guid id);

        Task CancelAsync(Guid id);

        Task FinishAsync(Guid id);
    }
}
