"use client";

interface ErrorAlertProps {
  error: string;
}

const getErrorDetails = (error: string): { title: string; description: string; emoji: string } => {
  const lowerError = error.toLowerCase();

  if (lowerError.includes("failed to fetch") || lowerError.includes("network") || lowerError.includes("connection")) {
    return {
      title: "Erro de conexão com o backend",
      description: "Talvez o projeto de backend não esteja rodando. Não deveria informar esse erro, mas como estamos em ambiente dev, não vejo problemas 🤷‍♂️",
      emoji: "🔌"
    };
  }

  if (lowerError.includes("500") || lowerError.includes("internal server")) {
    return {
      title: "Erro interno do servidor",
      description: "O backend está tendo um dia difícil. Verifique os logs da API para mais detalhes.",
      emoji: "💥"
    };
  }

  if (lowerError.includes("404") || lowerError.includes("not found")) {
    return {
      title: "Recurso não encontrado",
      description: "O endpoint solicitado não existe. Será que mudou de endereço sem avisar?",
      emoji: "🔍"
    };
  }

  if (lowerError.includes("400") || lowerError.includes("bad request")) {
    return {
      title: "Requisição inválida",
      description: "Os dados enviados não estão no formato esperado. Verifique os campos do formulário.",
      emoji: "📝"
    };
  }

  if (lowerError.includes("timeout")) {
    return {
      title: "Tempo limite excedido",
      description: "O servidor demorou demais para responder. Ele pode estar ocupado ou dormindo. ☕",
      emoji: "⏱️"
    };
  }

  return {
    title: "Erro inesperado",
    description: error,
    emoji: "⚠️"
  };
};

export default function ErrorAlert({ error }: ErrorAlertProps) {
  const { title, description, emoji } = getErrorDetails(error);

  return (
    <div className="bg-red-50 border-l-4 border-red-500 p-4 rounded mb-4">
      <div className="flex items-start">
        <span className="text-2xl mr-3">{emoji}</span>
        <div>
          <h3 className="text-red-800 font-semibold">{title}</h3>
          <p className="text-red-700 text-sm mt-1">{description}</p>
          <details className="mt-2">
            <summary className="text-red-600 text-xs cursor-pointer hover:underline">
              Detalhes técnicos
            </summary>
            <code className="text-xs text-red-500 mt-1 block bg-red-100 p-2 rounded">
              {error}
            </code>
          </details>
        </div>
      </div>
    </div>
  );
}

