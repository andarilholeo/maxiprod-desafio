using DesafioTecnico.Domain.Enums;

namespace DesafioTecnico.Application.DTOs;

public record CategoriaDto(Guid Id, string Descricao, Finalidade Finalidade);

public record CriarCategoriaRequest(string Descricao, Finalidade Finalidade);

public record TotalPorCategoriaDto(
    Guid CategoriaId,
    string CategoriaDescricao,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Saldo
);

public record TotaisCategoriasDto(
    IEnumerable<TotalPorCategoriaDto> TotaisPorCategoria,
    decimal TotalGeralReceitas,
    decimal TotalGeralDespesas,
    decimal SaldoLiquido
);

