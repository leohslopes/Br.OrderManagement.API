using Br.OrderManagement.Domain.Entities;
using Br.OrderManagement.Domain.Enums;
using Br.OrderManagement.Domain.Events;
using Br.OrderManagement.Domain.Exceptions;
using FluentAssertions;
using Xunit;


namespace Br.OrderManagement.Tests;


public class OrderTests
{

    [Fact]
    public void Should_Create_Order_With_Status_Created()
    {
        var order = Order.Create();

        order.Status.Should().Be(OrderStatus.Created);
        order.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Should_Add_Item_To_Order()
    {
        var product = new Product("Notebook", "Dell", 5000, 10, null);
        var order = Order.Create();

        order.AddItem(product, 2);

        order.Items.Should().HaveCount(1);


        order.TotalAmount.Should().Be(10000);
    }

    [Fact]
    public void Should_Not_Add_Item_When_Stock_Is_Insufficient()
    {
        var product = new Product( "Mouse", "Gamer", 100, 1, null);
        var order = Order.Create();

        Action action = () =>  order.AddItem(product, 2);

        action.Should().Throw<DomainException>().WithMessage("Estoque insuficiente.");
    }

    [Fact]
    public void Should_Confirm_Order_And_Create_Domain_Event()
    {
        var product = new Product("Teclado", "Mecânico", 300, 5, null);
        var order = Order.Create();

        order.AddItem(product, 1);
        order.Confirm();



        order.Status.Should().Be(OrderStatus.Confirmed);
        order.DomainEvents.Should().ContainSingle();
        order.DomainEvents.First().Should().BeOfType<OrderConfirmedEvent>();
    }

    [Fact]
    public void Should_Not_Confirm_Order_Without_Items()
    {
        var order = Order.Create();

        Action action = () => order.Confirm();

        action.Should().Throw<DomainException>().WithMessage("O pedido deve possuir pelo menos um item.");
    }

    [Fact]
    public void Should_Cancel_Confirmed_Order_And_Create_Event()
    {

        var product = new Product("Monitor", "LG", 1200, 5, null);
        var order = Order.Create();


        order.AddItem(product, 1);
        order.Confirm();
        order.Cancel();



        order.Status.Should().Be(OrderStatus.Canceled);
        order.DomainEvents.Should().Contain(x => x is OrderConfirmedEvent);
        order.DomainEvents.Should().Contain(x => x is OrderCanceledEvent);
    }

}