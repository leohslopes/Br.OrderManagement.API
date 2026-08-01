using Br.OrderManagement.Domain.Entities;
using Br.OrderManagement.Domain.Interfaces.Repositories;
using Br.OrderManagement.Repository.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Br.OrderManagement.Repository.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(Guid id)
        => await context.Products.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<Product>> GetAllAsync()
        => await context.Products.AsNoTracking().ToListAsync();

    public async Task AddAsync(Product product)
        => await context.Products.AddAsync(product);

    public void Update(Product product)
        => context.Products.Update(product);

    public void Delete(Product product)
        => context.Products.Remove(product);
}
