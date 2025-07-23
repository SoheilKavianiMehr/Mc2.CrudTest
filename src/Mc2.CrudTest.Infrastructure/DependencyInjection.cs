using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mc2.CrudTest.Domain.Repositories;
using Mc2.CrudTest.Infrastructure.Data;
using Mc2.CrudTest.Infrastructure.Repositories;

namespace Mc2.CrudTest.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }

        public static IServiceCollection AddInfrastructureInMemory(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }
    }
}