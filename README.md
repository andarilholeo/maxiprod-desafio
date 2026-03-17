# Desafio

Sistema de gestão de pessoas, categorias e transações financeiras.

## Tecnologias

### Backend
- .NET 10
- Entity Framework Core
- PostgreSQL
- Clean Architecture (Domain, Infrastructure, Application, API)
- Result Pattern

### Frontend
- React 19
- Next.js 15
- TypeScript
- Tailwind CSS

## Pré-requisitos

- [Docker](https://www.docker.com/products/docker-desktop/)
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)

## Execução com Docker (Recomendado)

### 1. Iniciar todos os serviços

```bash
docker-compose up -d
```

Isso irá iniciar:
- **PostgreSQL** na porta 5432
- **API .NET** na porta 5000 (com migrations automáticas)
- **pgAdmin** na porta 8080

### 2. Verificar se os containers estão rodando

```bash
docker-compose ps
```

### 3. Acessar o pgAdmin

Abra o navegador e acesse: **http://localhost:8080**

**Credenciais de acesso:**
| Campo    | Valor               |
|----------|---------------------|
| Email    | admin@maxiprod.com  |
| Password | admin123            |

### 4. Conectar ao banco de dados no pgAdmin

Após fazer login no pgAdmin, clique com o botão direito em **Servers** > **Register** > **Server** e configure:

**Aba General:**
| Campo | Valor       |
|-------|-------------|
| Name  | MaxiProd    |

**Aba Connection:**
| Campo              | Valor           |
|--------------------|-----------------|
| Host name/address  | maxiprod-db-dev |
| Port               | 5432            |
| Maintenance database | MaxiProdDb_Dev |
| Username           | maxiprod        |
| Password           | maxiprod123     |

Marque a opção **Save password** e clique em **Save**.

## Execução Local (Desenvolvimento)

### Backend

#### 1. Subir apenas o banco de dados

```bash
docker-compose up -d maxiprod-db-dev
```

#### 2. Rodar a API localmente

```bash
dotnet run --project src/MaxiProd.API
```

A API estará disponível em: **http://localhost:5000**

As migrations são aplicadas automaticamente ao iniciar a aplicação.

## Executando o Frontend

### 1. Instalar dependências

```bash
cd frontend
npm install
```

### 2. Rodar em modo desenvolvimento

```bash
npm run dev
```

O frontend estará disponível em: **http://localhost:3000**

## Estrutura do Projeto

```
maxiprod/
├── src/
│   ├── MaxiProd.Domain/        # Entidades, Enums, Interfaces
│   ├── MaxiProd.Infrastructure/ # DbContext, Repositórios, EF Core
│   ├── MaxiProd.Application/   # Services, DTOs, Regras de negócio
│   └── MaxiProd.API/           # Controllers, Program.cs
├── frontend/
│   └── src/
│       ├── app/                # Páginas (Next.js App Router)
│       ├── services/           # Chamadas à API
│       └── types/              # Tipos TypeScript
├── docker-compose.yml
└── init-db.sql
```

## Regras de Negócio

- Menores de 18 anos só podem ter transações do tipo **Despesa**
- Ao excluir uma pessoa, todas as suas transações são removidas
- A categoria deve ser compatível com o tipo da transação (Finalidade)

## Comandos Úteis

```bash
# Parar os containers
docker-compose down

# Parar e remover volumes (apaga dados)
docker-compose down -v

# Ver logs da API
docker-compose logs -f maxiprod-api

# Ver logs do banco
docker-compose logs -f maxiprod-db-dev

# Rebuild da API após alterações
docker-compose up -d --build maxiprod-api

# Rebuild do projeto .NET local
dotnet build

# Gerar nova migration (local)
cd src/MaxiProd.API
dotnet ef migrations add NomeDaMigration --project ../MaxiProd.Infrastructure
```

