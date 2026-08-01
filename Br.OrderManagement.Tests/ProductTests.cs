using Br.OrderManagement.Domain.Entities;
using Br.OrderManagement.Domain.Exceptions;
using FluentAssertions;

namespace Br.OrderManagement.Tests;


public class ProductTests
{

    [Fact]
    public void Should_Decrease_Product_Stock()
    {
        var product = new Product("Notebook", "Dell", 5000, 10, null);

        product.DecreaseStock(3);

        product.StockQuantity.Should().Be(7);

    }

    [Fact]
    public void Should_Not_Decrease_When_Stock_Is_Invalid()
    {
        var product = new Product("Notebook", "Dell", 5000, 2, null);

        Action action = () => product.DecreaseStock(5);

        action.Should().Throw<DomainException>().WithMessage("Estoque insuficiente.");
    }

    [Fact]
    public void Should_Increase_Product_Stock()
    {
        var product = new Product("Mouse", "Logitech", 200, 5, null);

        product.IncreaseStock(3);

        product.StockQuantity.Should().Be(8);
    }

}