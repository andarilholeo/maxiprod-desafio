import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "MaxiProd - Sistema de Gestão Financeira",
  description: "Sistema de gestão de pessoas, categorias e transações",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="pt-BR">
      <body className="bg-gray-100 min-h-screen">
        <nav className="bg-blue-600 text-white shadow-lg">
          <div className="max-w-7xl mx-auto px-4">
            <div className="flex justify-between items-center h-16">
              <a href="/" className="text-xl font-bold">MaxiProd</a>
              <div className="flex space-x-4">
                <a href="/pessoas" className="hover:bg-blue-700 px-3 py-2 rounded">Pessoas</a>
                <a href="/categorias" className="hover:bg-blue-700 px-3 py-2 rounded">Categorias</a>
                <a href="/transacoes" className="hover:bg-blue-700 px-3 py-2 rounded">Transações</a>
                <a href="/totais" className="hover:bg-blue-700 px-3 py-2 rounded">Totais</a>
              </div>
            </div>
          </div>
        </nav>
        <main className="max-w-7xl mx-auto px-4 py-8">
          {children}
        </main>
      </body>
    </html>
  );
}

