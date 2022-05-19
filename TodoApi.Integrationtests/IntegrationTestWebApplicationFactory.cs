using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TodoApi.Models;
using Xunit;

namespace TodoApi.Integrationtests
{
    public class IntegrationTestWebApplicationFactory:WebApplicationFactory<Program>,IAsyncLifetime
    {


        public IConfiguration Configuration { get; set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                Configuration = BuildConfiguration();

                config.AddConfiguration(Configuration);
            });
        }

        public async Task InitializeAsync()
        {
            var config = BuildConfiguration();
            var connection = config.GetConnectionString("Default");

            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseSqlServer(connection)
                .Options;

            var dbContext = new TodoContext(options);
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();
            
           


        }

        private static IConfiguration BuildConfiguration()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("integrationtest.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }

        public async Task DisposeAsync()
        {
          
        }
    }
}
