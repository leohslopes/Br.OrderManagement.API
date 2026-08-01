using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Br.OrderManagement.Domain.Common;
using Br.OrderManagement.Domain.Entities;
using Br.OrderManagement.Domain.Interfaces;
using MediatR;


namespace Br.OrderManagement.Repository.Persistence
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<Product> Products => Set<Product>();

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<DomainEvent>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEvents = ChangeTracker.Entries<AggregateRoot>().SelectMany(x => x.Entity.DomainEvents).ToList();

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainEvents)
                await _mediator.Publish(domainEvent, cancellationToken);

            foreach (var entity in ChangeTracker.Entries<AggregateRoot>())
                entity.Entity.ClearDomainEvents();

            return result;
        }
    }
}
