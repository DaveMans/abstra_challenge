using System.Linq;
using CarManagement.API;
using CarManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CarManagement.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registration
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add InMemory database for tests
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("CarManagement_TestDb");
            });

            // Build the provider and seed data
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Seed basic data
            if (!db.Brands.Any())
            {
                db.Brands.Add(new CarManagement.Domain.Entities.Brand
                {
                    Id = Guid.NewGuid(),
                    Name = "Toyota",
                    Country = "Japan",
                    FoundedYear = 1937
                });
                db.SaveChanges();
            }
        });
    }
}
