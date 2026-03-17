using DesafioTecnico.Application.Interfaces;
using DesafioTecnico.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioTecnico.Application;

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

