using MaxiProd.Domain.Enums;

namespace MaxiProd.Domain.Entities;

public class Categoria
{
    public Guid Id { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public Finalidade Finalidade { get; private set; }
    public ICollection<Transacao> Transacoes { get; private set; } = new List<Transacao>();

    private Categoria() { }

    public static Categoria Criar(string descricao, Finalidade finalidade)
    {
        return new Categoria
        {
            Id = Guid.NewGuid(),
            Descricao = descricao,
            Finalidade = finalidade
        };
    }

    public bool AceitaTipoTransacao(TipoTransacao tipo)
    {
        return Finalidade == Finalidade.Ambas ||
               (Finalidade == Finalidade.Despesa && tipo == TipoTransacao.Despesa) ||
               (Finalidade == Finalidade.Receita && tipo == TipoTransacao.Receita);
    }
}

