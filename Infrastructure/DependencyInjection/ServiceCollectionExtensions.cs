using Infrastructure.Abstractions;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<BibliotecaDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("PruebaSD"));
        });

        services.AddScoped<IAutorRepository, AutorRepository>();
        services.AddScoped<ILibroRepository, LibroRepository>();

        return services;
    }
}
