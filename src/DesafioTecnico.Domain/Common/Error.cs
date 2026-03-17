namespace DesafioTecnico.Domain.Common;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}

public static class DomainErrors
{
    public static class Pessoa
    {
        public static readonly Error NomeVazio = new("Pessoa.NomeVazio", "O nome da pessoa não pode ser vazio.");
        public static readonly Error NomeMuitoLongo = new("Pessoa.NomeMuitoLongo", "O nome da pessoa não pode ter mais de 200 caracteres.");
        public static readonly Error IdadeInvalida = new("Pessoa.IdadeInvalida", "A idade deve ser maior que zero.");
        public static readonly Error NaoEncontrada = new("Pessoa.NaoEncontrada", "Pessoa não encontrada.");
    }

    public static class Categoria
    {
        public static readonly Error DescricaoVazia = new("Categoria.DescricaoVazia", "A descrição da categoria não pode ser vazia.");
        public static readonly Error DescricaoMuitoLonga = new("Categoria.DescricaoMuitoLonga", "A descrição da categoria não pode ter mais de 400 caracteres.");
        public static readonly Error FinalidadeInvalida = new("Categoria.FinalidadeInvalida", "A finalidade informada é inválida.");
        public static readonly Error NaoEncontrada = new("Categoria.NaoEncontrada", "Categoria não encontrada.");
    }

    public static class Transacao
    {
        public static readonly Error DescricaoVazia = new("Transacao.DescricaoVazia", "A descrição da transação não pode ser vazia.");
        public static readonly Error DescricaoMuitoLonga = new("Transacao.DescricaoMuitoLonga", "A descrição da transação não pode ter mais de 400 caracteres.");
        public static readonly Error ValorInvalido = new("Transacao.ValorInvalido", "O valor deve ser maior que zero.");
        public static readonly Error TipoInvalido = new("Transacao.TipoInvalido", "O tipo de transação informado é inválido.");
        public static readonly Error CategoriaIncompativel = new("Transacao.CategoriaIncompativel", "A categoria não é compatível com o tipo de transação.");
        public static readonly Error MenorSomenteDespesa = new("Transacao.MenorSomenteDespesa", "Menores de idade só podem registrar despesas.");
        public static readonly Error NaoEncontrada = new("Transacao.NaoEncontrada", "Transação não encontrada.");
    }
}

