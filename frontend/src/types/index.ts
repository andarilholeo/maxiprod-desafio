export interface Pessoa {
  id: string;
  nome: string;
  idade: number;
}

export interface CriarPessoaRequest {
  nome: string;
  idade: number;
}

export interface AtualizarPessoaRequest {
  nome: string;
  idade: number;
}

export enum Finalidade {
  Despesa = "Despesa",
  Receita = "Receita",
  Ambas = "Ambas",
}

export interface Categoria {
  id: string;
  descricao: string;
  finalidade: Finalidade;
}

export interface CriarCategoriaRequest {
  descricao: string;
  finalidade: Finalidade;
}

export enum TipoTransacao {
  Despesa = "Despesa",
  Receita = "Receita",
}

export interface Transacao {
  id: string;
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  categoriaId: string;
  categoriaDescricao: string;
  pessoaId: string;
  pessoaNome: string;
}

export interface CriarTransacaoRequest {
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  categoriaId: string;
  pessoaId: string;
}

export interface TotalPorPessoa {
  pessoaId: string;
  pessoaNome: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

export interface TotaisGeral {
  totaisPorPessoa: TotalPorPessoa[];
  totalGeralReceitas: number;
  totalGeralDespesas: number;
  saldoLiquido: number;
}

export interface TotalPorCategoria {
  categoriaId: string;
  categoriaDescricao: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

export interface TotaisCategorias {
  totaisPorCategoria: TotalPorCategoria[];
  totalGeralReceitas: number;
  totalGeralDespesas: number;
  saldoLiquido: number;
}

