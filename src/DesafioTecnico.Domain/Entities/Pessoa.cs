namespace DesafioTecnico.Domain.Entities;

public class Pessoa
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public int Idade { get; private set; }
    public ICollection<Transacao> Transacoes { get; private set; } = new List<Transacao>();

    private Pessoa() { }

    public static Pessoa Criar(string nome, int idade)
    {
        return new Pessoa
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Idade = idade
        };
    }

    public void Atualizar(string nome, int idade)
    {
        Nome = nome;
        Idade = idade;
    }

    public bool EhMenorDeIdade() => Idade < 18;
}

