import {
  Pessoa,
  CriarPessoaRequest,
  AtualizarPessoaRequest,
  Categoria,
  CriarCategoriaRequest,
  Transacao,
  CriarTransacaoRequest,
  TotaisGeral,
  TotaisCategorias,
} from "@/types";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000/api";

async function handleResponse<T>(response: Response): Promise<T> {
  if (!response.ok) {
    const error = await response.json().catch(() => ({ error: "Erro desconhecido" }));
    throw new Error(error.error || "Erro na requisição");
  }
  return response.json();
}

// Pessoas
export async function getPessoas(): Promise<Pessoa[]> {
  const response = await fetch(`${API_BASE_URL}/pessoas`);
  return handleResponse<Pessoa[]>(response);
}

export async function getPessoa(id: string): Promise<Pessoa> {
  const response = await fetch(`${API_BASE_URL}/pessoas/${id}`);
  return handleResponse<Pessoa>(response);
}

export async function criarPessoa(data: CriarPessoaRequest): Promise<Pessoa> {
  const response = await fetch(`${API_BASE_URL}/pessoas`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  return handleResponse<Pessoa>(response);
}

export async function atualizarPessoa(id: string, data: AtualizarPessoaRequest): Promise<Pessoa> {
  const response = await fetch(`${API_BASE_URL}/pessoas/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  return handleResponse<Pessoa>(response);
}

export async function deletarPessoa(id: string): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/pessoas/${id}`, {
    method: "DELETE",
  });
  if (!response.ok) {
    const error = await response.json().catch(() => ({ error: "Erro desconhecido" }));
    throw new Error(error.error || "Erro ao deletar pessoa");
  }
}

export async function getTotaisPorPessoa(): Promise<TotaisGeral> {
  const response = await fetch(`${API_BASE_URL}/pessoas/totais`);
  return handleResponse<TotaisGeral>(response);
}

// Categorias
export async function getCategorias(): Promise<Categoria[]> {
  const response = await fetch(`${API_BASE_URL}/categorias`);
  return handleResponse<Categoria[]>(response);
}

export async function criarCategoria(data: CriarCategoriaRequest): Promise<Categoria> {
  const response = await fetch(`${API_BASE_URL}/categorias`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  return handleResponse<Categoria>(response);
}

export async function getTotaisPorCategoria(): Promise<TotaisCategorias> {
  const response = await fetch(`${API_BASE_URL}/categorias/totais`);
  return handleResponse<TotaisCategorias>(response);
}

// Transações
export async function getTransacoes(): Promise<Transacao[]> {
  const response = await fetch(`${API_BASE_URL}/transacoes`);
  return handleResponse<Transacao[]>(response);
}

export async function criarTransacao(data: CriarTransacaoRequest): Promise<Transacao> {
  const response = await fetch(`${API_BASE_URL}/transacoes`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  return handleResponse<Transacao>(response);
}

