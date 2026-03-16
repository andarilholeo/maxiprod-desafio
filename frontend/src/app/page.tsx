export default function Home() {
  return (
    <div className="text-center">
      <h1 className="text-4xl font-bold text-gray-800 mb-8">
        Bem-vindo ao MaxiProd
      </h1>
      <p className="text-gray-600 mb-8">
        Sistema de gestão de pessoas, categorias e transações financeiras
      </p>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 max-w-4xl mx-auto">
        <a
          href="/pessoas"
          className="bg-white p-6 rounded-lg shadow-md hover:shadow-lg transition-shadow"
        >
          <h2 className="text-xl font-semibold text-blue-600 mb-2">Pessoas</h2>
          <p className="text-gray-500">Gerenciar cadastro de pessoas</p>
        </a>
        <a
          href="/categorias"
          className="bg-white p-6 rounded-lg shadow-md hover:shadow-lg transition-shadow"
        >
          <h2 className="text-xl font-semibold text-blue-600 mb-2">Categorias</h2>
          <p className="text-gray-500">Gerenciar categorias de transações</p>
        </a>
        <a
          href="/transacoes"
          className="bg-white p-6 rounded-lg shadow-md hover:shadow-lg transition-shadow"
        >
          <h2 className="text-xl font-semibold text-blue-600 mb-2">Transações</h2>
          <p className="text-gray-500">Registrar receitas e despesas</p>
        </a>
      </div>
    </div>
  );
}

