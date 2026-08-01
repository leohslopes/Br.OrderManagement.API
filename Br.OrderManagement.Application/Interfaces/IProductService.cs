using Br.OrderManagement.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br.OrderManagement.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();

        Task<ProductDto?> GetByIdAsync(Guid id);

        Task<Guid> CreateAsync(CreateProductDto dto);

        Task UpdateAsync(UpdateProductDto dto);

        Task DeleteAsync(Guid id);
    }
}
