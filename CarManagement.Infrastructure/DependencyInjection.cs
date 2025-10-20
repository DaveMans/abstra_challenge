using CarManagement.Domain.Interfaces;
using CarManagement.Domain.Interfaces.Repositories;
using CarManagement.Infrastructure.Persistence;
using CarManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Server=(localdb)\\mssqllocaldb;Database=CarManagement;Trusted_Connection=True;MultipleActiveResultSets=true";
        services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseSqlServer(connectionString);
        });

        services.AddMemoryCache();

        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<ILineRepository, LineRepository>();
        services.AddScoped<IModelYearRepository, ModelYearRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
