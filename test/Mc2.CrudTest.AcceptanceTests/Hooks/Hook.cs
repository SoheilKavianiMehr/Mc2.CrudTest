using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;
using Mc2.CrudTest.Application;
using Mc2.CrudTest.Infrastructure;
using Mc2.CrudTest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mc2.CrudTest.AcceptanceTests.Hooks
{
    [Binding]
    public class Hooks
    {
        private static IServiceProvider? _serviceProvider;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var services = new ServiceCollection();
            
            services.AddLogging();
            
            services.AddApplication();
            
            services.AddInfrastructureInMemory();
            
            _serviceProvider = services.BuildServiceProvider();
            
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            if (_serviceProvider != null)
            {
                scenarioContext.ScenarioContainer.RegisterInstanceAs(_serviceProvider);
            }
        }
    }
}