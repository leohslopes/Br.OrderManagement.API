using Br.OrderManagement.Domain.Entities;
using Br.OrderManagement.Domain.Interfaces.Repositories;
using Br.OrderManagement.Repository.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Br.OrderManagement.Repository.Repositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(Guid id)
        => await context.Orders.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Order?> GetWithItemsAsync(Guid id)
        => await context.Orders
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<Order>> GetAllAsync()
        =>  await context.Orders
             .Include(order => order.Items)
             .ThenInclude(item => item.Product)
             .AsNoTracking()
             .ToListAsync();

    public async Task AddAsync(Order order)
        => await context.Orders.AddAsync(order);

    public void Update(Order order)
        => context.Orders.Update(order);
}