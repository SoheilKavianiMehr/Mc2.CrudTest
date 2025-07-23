using Mc2.CrudTest.Application.Basic.Interfaces;
using Mc2.CrudTest.Domain.Customers;
using Mc2.CrudTest.Infrastructure.Customers;
using Mc2.CrudTest.Infrastructure.Customers.Projections;
using Mc2.CrudTest.Infrastructure.Data;
using Mc2.CrudTest.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mc2.CrudTest.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("CrudTestDatabase"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));
            }

            services.AddScoped<IEventStore, EventStore.EventStore>();
            services.AddScoped<ITransactionManager, TransactionManager>();
            
            services.AddScoped<IEventProjector>(provider => 
                new EventProjector(
                    provider.GetRequiredService<IEnumerable<IProjectionHandler>>(),
                    provider.GetRequiredService<ILogger<EventProjector>>(),
                    provider.GetRequiredService<ApplicationDbContext>()));
            services.AddScoped<IProjectionHandler, CustomerCreatedEventHandler>();
            services.AddScoped<IProjectionHandler, CustomerUpdatedEventHandler>();
            services.AddScoped<IProjectionHandler, CustomerDeletedEventHandler>();
            
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }

        public static IServiceCollection AddInfrastructureInMemory(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            services.AddScoped<IEventStore, EventStore.EventStore>();
            services.AddScoped<ITransactionManager, TransactionManager>();
            
            services.AddScoped<IEventProjector>(provider => 
                new EventProjector(
                    provider.GetRequiredService<IEnumerable<IProjectionHandler>>(),
                    provider.GetRequiredService<ILogger<EventProjector>>(),
                    provider.GetRequiredService<ApplicationDbContext>()));
            services.AddScoped<IProjectionHandler, CustomerCreatedEventHandler>();
            services.AddScoped<IProjectionHandler, CustomerUpdatedEventHandler>();
            services.AddScoped<IProjectionHandler, CustomerDeletedEventHandler>();
            
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }
    }
}