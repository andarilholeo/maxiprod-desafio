"use client";

import { useEffect, useState } from "react";
import { Transacao, CriarTransacaoRequest, TipoTransacao, Pessoa, Categoria, Finalidade } from "@/types";
import { getTransacoes, criarTransacao, getPessoas, getCategorias } from "@/services/api";

export default function TransacoesPage() {
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [categorias, setCategorias] = useState<Categoria[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [formData, setFormData] = useState<CriarTransacaoRequest>({
    descricao: "", valor: 0, tipo: TipoTransacao.Despesa, categoriaId: "", pessoaId: ""
  });

  const loadData = async () => {
    try {
      setLoading(true);
      const [transacoesData, pessoasData, categoriasData] = await Promise.all([
        getTransacoes(), getPessoas(), getCategorias()
      ]);
      setTransacoes(transacoesData);
      setPessoas(pessoasData);
      setCategorias(categoriasData);
      setError(null);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Erro ao carregar dados");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { loadData(); }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await criarTransacao(formData);
      setShowForm(false);
      setFormData({ descricao: "", valor: 0, tipo: TipoTransacao.Despesa, categoriaId: "", pessoaId: "" });
      loadData();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Erro ao criar transação");
    }
  };

  const getCategoriasDisponiveis = () => {
    return categorias.filter(c => {
      if (formData.tipo === TipoTransacao.Despesa) {
        return c.finalidade === Finalidade.Despesa || c.finalidade === Finalidade.Ambas;
      }
      return c.finalidade === Finalidade.Receita || c.finalidade === Finalidade.Ambas;
    });
  };

  const getTipoBadgeClass = (tipo: TipoTransacao) => {
    return tipo === TipoTransacao.Receita ? "bg-green-100 text-green-800" : "bg-red-100 text-red-800";
  };

  if (loading) return <div className="text-center">Carregando...</div>;

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-800">Transações</h1>
        <button onClick={() => setShowForm(true)}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">
          Nova Transação
        </button>
      </div>

      {error && <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">{error}</div>}

      {showForm && (
        <div className="bg-white p-6 rounded-lg shadow-md mb-6">
          <h2 className="text-xl font-semibold mb-4">Nova Transação</h2>
          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-medium text-gray-700">Pessoa</label>
                <select value={formData.pessoaId}
                  onChange={(e) => setFormData({ ...formData, pessoaId: e.target.value })}
                  className="mt-1 block w-full rounded-md border-gray-300 shadow-sm p-2 border" required>
                  <option value="">Selecione...</option>
                  {pessoas.map(p => <option key={p.id} value={p.id}>{p.nome} ({p.idade} anos)</option>)}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700">Tipo</label>
                <select value={formData.tipo}
                  onChange={(e) => setFormData({ ...formData, tipo: e.target.value as TipoTransacao, categoriaId: "" })}
                  className="mt-1 block w-full rounded-md border-gray-300 shadow-sm p-2 border">
                  <option value={TipoTransacao.Despesa}>Despesa</option>
                  <option value={TipoTransacao.Receita}>Receita</option>
                </select>
              </div>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-medium text-gray-700">Categoria</label>
                <select value={formData.categoriaId}
                  onChange={(e) => setFormData({ ...formData, categoriaId: e.target.value })}
                  className="mt-1 block w-full rounded-md border-gray-300 shadow-sm p-2 border" required>
                  <option value="">Selecione...</option>
                  {getCategoriasDisponiveis().map(c => <option key={c.id} value={c.id}>{c.descricao}</option>)}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700">Valor</label>
                <input type="number" step="0.01" value={formData.valor}
                  onChange={(e) => setFormData({ ...formData, valor: parseFloat(e.target.value) || 0 })}
                  className="mt-1 block w-full rounded-md border-gray-300 shadow-sm p-2 border" min="0.01" required />
              </div>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">Descrição</label>
              <input type="text" value={formData.descricao}
                onChange={(e) => setFormData({ ...formData, descricao: e.target.value })}
                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm p-2 border" maxLength={400} required />
            </div>
            <div className="flex space-x-2">
              <button type="submit" className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">Salvar</button>
              <button type="button" onClick={() => setShowForm(false)}
                className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400">Cancelar</button>
            </div>
          </form>
        </div>
      )}

      <div className="bg-white rounded-lg shadow-md overflow-hidden">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Descrição</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Pessoa</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Categoria</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Tipo</th>
              <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Valor</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {transacoes.map((t) => (
              <tr key={t.id}>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{t.descricao}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{t.pessoaNome}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{t.categoriaDescricao}</td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <span className={`px-2 py-1 rounded-full text-xs font-medium ${getTipoBadgeClass(t.tipo)}`}>{t.tipo}</span>
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-right font-medium">R$ {t.valor.toFixed(2)}</td>
              </tr>
            ))}
            {transacoes.length === 0 && (
              <tr><td colSpan={5} className="px-6 py-4 text-center text-gray-500">Nenhuma transação cadastrada</td></tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}

