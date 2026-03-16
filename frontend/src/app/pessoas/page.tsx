"use client";

import { useEffect, useState } from "react";
import { Pessoa, CriarPessoaRequest } from "@/types";
import { getPessoas, criarPessoa, atualizarPessoa, deletarPessoa } from "@/services/api";

export default function PessoasPage() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [editingPessoa, setEditingPessoa] = useState<Pessoa | null>(null);
  const [formData, setFormData] = useState<CriarPessoaRequest>({ nome: "", idade: 0 });

  const loadPessoas = async () => {
    try {
      setLoading(true);
      const data = await getPessoas();
      setPessoas(data);
      setError(null);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Erro ao carregar pessoas");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { loadPessoas(); }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingPessoa) {
        await atualizarPessoa(editingPessoa.id, formData);
      } else {
        await criarPessoa(formData);
      }
      setShowForm(false);
      setEditingPessoa(null);
      setFormData({ nome: "", idade: 0 });
      loadPessoas();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Erro ao salvar pessoa");
    }
  };

  const handleEdit = (pessoa: Pessoa) => {
    setEditingPessoa(pessoa);
    setFormData({ nome: pessoa.nome, idade: pessoa.idade });
    setShowForm(true);
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Tem certeza que deseja excluir esta pessoa? Todas as transações serão removidas.")) return;
    try {
      await deletarPessoa(id);
      loadPessoas();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Erro ao deletar pessoa");
    }
  };

  if (loading) return <div className="text-center">Carregando...</div>;

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-800">Pessoas</h1>
        <button onClick={() => { setShowForm(true); setEditingPessoa(null); setFormData({ nome: "", idade: 0 }); }}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">
          Nova Pessoa
        </button>
      </div>

      {error && <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">{error}</div>}

      {showForm && (
        <div className="bg-white p-6 rounded-lg shadow-md mb-6">
          <h2 className="text-xl font-semibold mb-4">{editingPessoa ? "Editar Pessoa" : "Nova Pessoa"}</h2>
          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700">Nome</label>
              <input type="text" value={formData.nome} onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 p-2 border"
                maxLength={200} required />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">Idade</label>
              <input type="number" value={formData.idade} onChange={(e) => setFormData({ ...formData, idade: parseInt(e.target.value) || 0 })}
                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 p-2 border"
                min={1} required />
            </div>
            <div className="flex space-x-2">
              <button type="submit" className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">Salvar</button>
              <button type="button" onClick={() => { setShowForm(false); setEditingPessoa(null); }}
                className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400">Cancelar</button>
            </div>
          </form>
        </div>
      )}

      <div className="bg-white rounded-lg shadow-md overflow-hidden">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Nome</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Idade</th>
              <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Ações</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {pessoas.map((pessoa) => (
              <tr key={pessoa.id}>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{pessoa.nome}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{pessoa.idade} anos</td>
                <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                  <button onClick={() => handleEdit(pessoa)} className="text-blue-600 hover:text-blue-900 mr-4">Editar</button>
                  <button onClick={() => handleDelete(pessoa.id)} className="text-red-600 hover:text-red-900">Excluir</button>
                </td>
              </tr>
            ))}
            {pessoas.length === 0 && (
              <tr><td colSpan={3} className="px-6 py-4 text-center text-gray-500">Nenhuma pessoa cadastrada</td></tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}

