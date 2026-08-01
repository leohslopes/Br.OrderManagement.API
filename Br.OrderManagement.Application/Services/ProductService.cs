using Br.OrderManagement.Application.DTOs.Product;
using Br.OrderManagement.Application.Interfaces;
using Br.OrderManagement.Domain.Entities;
using Br.OrderManagement.Domain.Interfaces;
using Br.OrderManagement.Domain.Interfaces.Repositories;

namespace Br.OrderManagement.Application.Services;

public class ProductService(
    IProductRepository repository,
    IUnitOfWork unitOfWork) : IProductService
{
    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await repository.GetAllAsync();

        return products.Select(x => new ProductDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
            StockQuantity = x.StockQuantity,
            ImageBase64 = x.Image != null ? Convert.ToBase64String(x.Image) : null
        });
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await repository.GetByIdAsync(id);

        if (product == null)
            return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            ImageBase64 = product.Image != null ? Convert.ToBase64String(product.Image) : null
        };
    }

    public async Task<Guid> CreateAsync(CreateProductDto dto)
    {
        byte[]? image = !string.IsNullOrWhiteSpace(dto.ImageBase64) ? Convert.FromBase64String(dto.ImageBase64) : null;

        var product = new Product(dto.Name, dto.Description, dto.Price, dto.StockQuantity, image);

        await repository.AddAsync(product);
        await unitOfWork.SaveChangesAsync();

        return product.Id;
    }

    public async Task UpdateAsync(UpdateProductDto dto)
    {
        var product = await repository.GetByIdAsync(dto.Id) ?? throw new Exception("Produto não encontrado.");

        product.Update(dto.Name, dto.Description, dto.Price);

        if (!string.IsNullOrWhiteSpace(dto.ImageBase64))
        {
            product.SetImage(Convert.FromBase64String(dto.ImageBase64));
        }
        else
        {
            product.SetImage(null);
        }

        repository.Update(product);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await repository.GetByIdAsync(id) ?? throw new Exception("Produto não encontrado.");
        repository.Delete(product);

        await unitOfWork.SaveChangesAsync();
    }
}