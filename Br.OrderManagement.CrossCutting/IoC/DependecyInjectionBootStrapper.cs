using Br.OrderManagement.Application;
using Br.OrderManagement.Application.DTOs.Product;
using Br.OrderManagement.Application.Interfaces;
using Br.OrderManagement.Application.Services;
using Br.OrderManagement.Domain.Interfaces;
using Br.OrderManagement.Domain.Interfaces.Repositories;
using Br.OrderManagement.Repository.Persistence;
using Br.OrderManagement.Repository.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Br.OrderManagement.CrossCutting.IoC;

public static class DependecyInjectionBootStrapper
{
    public static void RegisterAllClasses(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        RegisterDatabase(services, configuration);
        RegisterRepositories(services);
        RegisterServices(services);
    }

    private static void RegisterDatabase(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AppDbContext>());
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        var applicationAssembly = typeof(ProductDto).Assembly;

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(applicationAssembly));

        //services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();

    }
}