using MaxiProd.Domain.Enums;

namespace MaxiProd.Application.DTOs;

public record TransacaoDto(
    Guid Id,
    string Descricao,
    decimal Valor,
    TipoTransacao Tipo,
    Guid CategoriaId,
    string CategoriaDescricao,
    Guid PessoaId,
    string PessoaNome
);

public record CriarTransacaoRequest(
    string Descricao,
    decimal Valor,
    TipoTransacao Tipo,
    Guid CategoriaId,
    Guid PessoaId
);

