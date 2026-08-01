using Br.OrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br.OrderManagement.Domain.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id);

        Task<Order?> GetWithItemsAsync(Guid id);

        Task<IEnumerable<Order>> GetAllAsync();

        Task AddAsync(Order order);

        void Update(Order order);
    }
}
