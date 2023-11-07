using WebApiMarvelService.Service;
using WebApiMarvelService.Interface;
using WebApiMarvelRepository.Interface;
using WebApiMarvelRepository.Repository;

namespace WebApiMarvel;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabaseExtensions(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment, string environmentName = "Development")
    {
        services.AddHttpClients();
        return services;
    }

    private static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddScoped<IPersonagemService, PersonagemService>();
        services.AddTransient<IPersonagemRepository, PersonagemRepository>();
        return services;
    }
}
