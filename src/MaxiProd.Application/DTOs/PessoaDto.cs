namespace MaxiProd.Application.DTOs;

public record PessoaDto(Guid Id, string Nome, int Idade);

public record CriarPessoaRequest(string Nome, int Idade);

public record AtualizarPessoaRequest(string Nome, int Idade);

public record TotalPorPessoaDto(
    Guid PessoaId,
    string PessoaNome,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Saldo
);

public record TotaisGeralDto(
    IEnumerable<TotalPorPessoaDto> TotaisPorPessoa,
    decimal TotalGeralReceitas,
    decimal TotalGeralDespesas,
    decimal SaldoLiquido
);

