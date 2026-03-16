using MaxiProd.Domain.Enums;

namespace MaxiProd.Domain.Entities;

public class Transacao
{
    public Guid Id { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public decimal Valor { get; private set; }
    public TipoTransacao Tipo { get; private set; }
    public Guid CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; } = null!;
    public Guid PessoaId { get; private set; }
    public Pessoa Pessoa { get; private set; } = null!;

    private Transacao() { }

    public static Transacao Criar(string descricao, decimal valor, TipoTransacao tipo, Guid categoriaId, Guid pessoaId)
    {
        return new Transacao
        {
            Id = Guid.NewGuid(),
            Descricao = descricao,
            Valor = valor,
            Tipo = tipo,
            CategoriaId = categoriaId,
            PessoaId = pessoaId
        };
    }
}

