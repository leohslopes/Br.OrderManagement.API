using Br.OrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br.OrderManagement.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);

        Task<IEnumerable<Product>> GetAllAsync();

        Task AddAsync(Product product);

        void Update(Product product);

        void Delete(Product product);
    }
}
