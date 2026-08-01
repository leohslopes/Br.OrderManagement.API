using Br.OrderManagement.Application.DTOs.Order;
using Br.OrderManagement.Application.Interfaces;
using Br.OrderManagement.Domain.Entities;
using Br.OrderManagement.Domain.Enums;
using Br.OrderManagement.Domain.Interfaces;
using Br.OrderManagement.Domain.Interfaces.Repositories;

namespace Br.OrderManagement.Application.Services;

public class OrderService(
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IOrderService
{
    public async Task<Guid> CreateAsync(CreateOrderDto dto)
    {
        var order = Order.Create();

        foreach (var item in dto.Items)
        {
            var product = await productRepository.GetByIdAsync(item.ProductId) ?? throw new Exception("Produto não encontrado.");
            order.AddItem(product, item.Quantity);
        }

        await orderRepository.AddAsync(order);
        await unitOfWork.SaveChangesAsync();

        return order.Id;
    }

    public async Task ConfirmAsync(Guid id)
    {
        var order = await orderRepository.GetWithItemsAsync(id) ?? throw new Exception("Pedido não encontrado.");

        order.Confirm();

        foreach (var item in order.Items)
        {
            var product = await productRepository.GetByIdAsync(item.ProductId)  ?? throw new Exception($"Produto {item.ProductId} não encontrado.");

            product.DecreaseStock(item.Quantity);
            productRepository.Update(product);
        }

        orderRepository.Update(order);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task CancelAsync(Guid id)
    {
        var order = await orderRepository.GetWithItemsAsync(id) ?? throw new Exception("Pedido não encontrado.");


        if (order.Status.Equals(OrderStatus.Confirmed))
        {
            foreach (var item in order.Items)
            {
                var product = await productRepository.GetByIdAsync(item.ProductId) ?? throw new Exception($"Produto {item.ProductId} não encontrado.");

                product.IncreaseStock(item.Quantity);
                productRepository.Update(product);
            }
        }

        order.Cancel();
        orderRepository.Update(order);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task FinishAsync(Guid id)
    {
        var order = await orderRepository.GetWithItemsAsync(id) ?? throw new Exception("Pedido não encontrado.");

        order.Finish();
        orderRepository.Update(order);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await orderRepository.GetAllAsync();

        return orders.Select(MapOrder);
    }

    public async Task<OrderDto?> GetByIdAsync(Guid id)
    {
        var order = await orderRepository.GetWithItemsAsync(id);

        if (order == null)
            return null;

        return MapOrder(order);
    }

    private static OrderDto MapOrder(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            Items = [.. order.Items.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.TotalPrice
            })]
        };
    }
}
