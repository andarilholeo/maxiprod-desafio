using MaxiProd.Application.Interfaces;
using MaxiProd.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MaxiProd.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPessoaService, PessoaService>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<ITransacaoService, TransacaoService>();

        return services;
    }
}

