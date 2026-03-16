"use client";

import { useEffect, useState } from "react";
import { Categoria, CriarCategoriaRequest, Finalidade } from "@/types";
import { getCategorias, criarCategoria } from "@/services/api";

export default function CategoriasPage() {
  const [categorias, setCategorias] = useState<Categoria[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [formData, setFormData] = useState<CriarCategoriaRequest>({ 
    descricao: "", 
    finalidade: Finalidade.Ambas 
  });

  const loadCategorias = async () => {
    try {
      setLoading(true);
      const data = await getCategorias();
      setCategorias(data);
      setError(null);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Erro ao carregar categorias");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { loadCategorias(); }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await criarCategoria(formData);
      setShowForm(false);
      setFormData({ descricao: "", finalidade: Finalidade.Ambas });
      loadCategorias();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Erro ao criar categoria");
    }
  };

  const getFinalidadeLabel = (finalidade: Finalidade) => {
    const labels = { [Finalidade.Despesa]: "Despesa", [Finalidade.Receita]: "Receita", [Finalidade.Ambas]: "Ambas" };
    return labels[finalidade] || finalidade;
  };

  const getFinalidadeBadgeClass = (finalidade: Finalidade) => {
    const classes = {
      [Finalidade.Despesa]: "bg-red-100 text-red-800",
      [Finalidade.Receita]: "bg-green-100 text-green-800",
      [Finalidade.Ambas]: "bg-blue-100 text-blue-800"
    };
    return classes[finalidade] || "bg-gray-100 text-gray-800";
  };

  if (loading) return <div className="text-center">Carregando...</div>;

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-800">Categorias</h1>
        <button onClick={() => setShowForm(true)}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">
          Nova Categoria
        </button>
      </div>

      {error && <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">{error}</div>}

      {showForm && (
        <div className="bg-white p-6 rounded-lg shadow-md mb-6">
          <h2 className="text-xl font-semibold mb-4">Nova Categoria</h2>
          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700">Descrição</label>
              <input type="text" value={formData.descricao}
                onChange={(e) => setFormData({ ...formData, descricao: e.target.value })}
                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 p-2 border"
                maxLength={400} required />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">Finalidade</label>
              <select value={formData.finalidade}
                onChange={(e) => setFormData({ ...formData, finalidade: e.target.value as Finalidade })}
                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 p-2 border">
                <option value={Finalidade.Despesa}>Despesa</option>
                <option value={Finalidade.Receita}>Receita</option>
                <option value={Finalidade.Ambas}>Ambas</option>
              </select>
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
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Finalidade</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {categorias.map((categoria) => (
              <tr key={categoria.id}>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{categoria.descricao}</td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <span className={`px-2 py-1 rounded-full text-xs font-medium ${getFinalidadeBadgeClass(categoria.finalidade)}`}>
                    {getFinalidadeLabel(categoria.finalidade)}
                  </span>
                </td>
              </tr>
            ))}
            {categorias.length === 0 && (
              <tr><td colSpan={2} className="px-6 py-4 text-center text-gray-500">Nenhuma categoria cadastrada</td></tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}

