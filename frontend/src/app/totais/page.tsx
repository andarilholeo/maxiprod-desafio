"use client";

import { useEffect, useState } from "react";
import { TotaisGeral, TotaisCategorias } from "@/types";
import { getTotaisPorPessoa, getTotaisPorCategoria } from "@/services/api";

export default function TotaisPage() {
  const [totaisPessoa, setTotaisPessoa] = useState<TotaisGeral | null>(null);
  const [totaisCategoria, setTotaisCategoria] = useState<TotaisCategorias | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<"pessoa" | "categoria">("pessoa");

  useEffect(() => {
    const loadData = async () => {
      try {
        setLoading(true);
        const [pessoaData, categoriaData] = await Promise.all([
          getTotaisPorPessoa(),
          getTotaisPorCategoria()
        ]);
        setTotaisPessoa(pessoaData);
        setTotaisCategoria(categoriaData);
        setError(null);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Erro ao carregar totais");
      } finally {
        setLoading(false);
      }
    };
    loadData();
  }, []);

  const formatCurrency = (value: number) => `R$ ${value.toFixed(2)}`;
  const getSaldoClass = (saldo: number) => saldo >= 0 ? "text-green-600" : "text-red-600";

  if (loading) return <div className="text-center">Carregando...</div>;
  if (error) return <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">{error}</div>;

  return (
    <div>
      <h1 className="text-2xl font-bold text-gray-800 mb-6">Consulta de Totais</h1>

      <div className="mb-6">
        <div className="border-b border-gray-200">
          <nav className="-mb-px flex space-x-8">
            <button onClick={() => setActiveTab("pessoa")}
              className={`py-4 px-1 border-b-2 font-medium text-sm ${activeTab === "pessoa"
                ? "border-blue-500 text-blue-600" : "border-transparent text-gray-500 hover:text-gray-700"}`}>
              Por Pessoa
            </button>
            <button onClick={() => setActiveTab("categoria")}
              className={`py-4 px-1 border-b-2 font-medium text-sm ${activeTab === "categoria"
                ? "border-blue-500 text-blue-600" : "border-transparent text-gray-500 hover:text-gray-700"}`}>
              Por Categoria
            </button>
          </nav>
        </div>
      </div>

      {activeTab === "pessoa" && totaisPessoa && (
        <div className="bg-white rounded-lg shadow-md overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Pessoa</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Receitas</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Despesas</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Saldo</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {totaisPessoa.totaisPorPessoa.map((t) => (
                <tr key={t.pessoaId}>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{t.pessoaNome}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-right text-green-600">{formatCurrency(t.totalReceitas)}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-right text-red-600">{formatCurrency(t.totalDespesas)}</td>
                  <td className={`px-6 py-4 whitespace-nowrap text-sm text-right font-medium ${getSaldoClass(t.saldo)}`}>{formatCurrency(t.saldo)}</td>
                </tr>
              ))}
            </tbody>
            <tfoot className="bg-gray-100">
              <tr className="font-bold">
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">TOTAL GERAL</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-right text-green-600">{formatCurrency(totaisPessoa.totalGeralReceitas)}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-right text-red-600">{formatCurrency(totaisPessoa.totalGeralDespesas)}</td>
                <td className={`px-6 py-4 whitespace-nowrap text-sm text-right ${getSaldoClass(totaisPessoa.saldoLiquido)}`}>{formatCurrency(totaisPessoa.saldoLiquido)}</td>
              </tr>
            </tfoot>
          </table>
        </div>
      )}

      {activeTab === "categoria" && totaisCategoria && (
        <div className="bg-white rounded-lg shadow-md overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Categoria</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Receitas</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Despesas</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Saldo</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {totaisCategoria.totaisPorCategoria.map((t) => (
                <tr key={t.categoriaId}>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{t.categoriaDescricao}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-right text-green-600">{formatCurrency(t.totalReceitas)}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-right text-red-600">{formatCurrency(t.totalDespesas)}</td>
                  <td className={`px-6 py-4 whitespace-nowrap text-sm text-right font-medium ${getSaldoClass(t.saldo)}`}>{formatCurrency(t.saldo)}</td>
                </tr>
              ))}
            </tbody>
            <tfoot className="bg-gray-100">
              <tr className="font-bold">
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">TOTAL GERAL</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-right text-green-600">{formatCurrency(totaisCategoria.totalGeralReceitas)}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-right text-red-600">{formatCurrency(totaisCategoria.totalGeralDespesas)}</td>
                <td className={`px-6 py-4 whitespace-nowrap text-sm text-right ${getSaldoClass(totaisCategoria.saldoLiquido)}`}>{formatCurrency(totaisCategoria.saldoLiquido)}</td>
              </tr>
            </tfoot>
          </table>
        </div>
      )}
    </div>
  );
}

