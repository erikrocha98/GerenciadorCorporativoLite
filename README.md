# GerenciadorCorporativoLite

Sistema fullstack para gerenciamento corporativo de pessoas físicas e jurídicas, incluindo fornecedores e empresas com seus relacionamentos.

---

## Tecnologias

### Backend
- **[.NET 9 / ASP.NET Core](https://dotnet.microsoft.com/)** — API REST
- **[Entity Framework Core 9](https://learn.microsoft.com/ef/core/)** — ORM com SQL Server
- **[AutoMapper](https://automapper.org/)** — mapeamento entre entidades e DTOs
- **[Swagger / Swashbuckle](https://swagger.io/)** — documentação interativa da API

### Frontend
- **[Vue 3](https://vuejs.org/)** + **[TypeScript](https://www.typescriptlang.org/)** — framework reativo com tipagem estática
- **[Vite](https://vitejs.dev/)** — ferramenta de build e dev server
- **[Vuetify 3](https://vuetifyjs.com/)** — biblioteca de componentes Material Design
- **[Vue Router](https://router.vuejs.org/)** — roteamento SPA
- **[Pinia](https://pinia.vuejs.org/)** — gerenciamento de estado
- **[Axios](https://axios-http.com/)** — cliente HTTP

### Infraestrutura
- **[Docker](https://www.docker.com/)** + **Docker Compose** — orquestração de containers
- **SQL Server (Azure SQL Edge)** — banco de dados relacional

---

## Arquitetura do Backend

O backend segue os princípios da **Clean Architecture**, organizado em quatro projetos com dependências unidirecionais:

```
DesafioFullStack.API          → Camada de apresentação (controllers, configuração HTTP)
DesafioFullStack.Application  → Casos de uso (DTOs, mapeamentos com AutoMapper)
DesafioFullStack.Domain       → Núcleo do negócio (entidades, interfaces, serviços de domínio, value objects, validadores)
DesafioFullStack.Infrastructure → Implementação de persistência (EF Core, repositórios, migrations)
```

**Fluxo de dependência:**
```
API → Application → Domain ← Infrastructure
```

- **Domain** não depende de nenhuma outra camada — contém entidades (`Empresa`, `Fornecedor`, `EmpresaFornecedor`), interfaces de repositório (`IRepository`, `IEmpresaRepository`, `IFornecedorRepository`), value objects com validação própria (`Cpf`, `Cnpj`, `Cep`), validadores e serviços de domínio.
- **Application** orquestra os casos de uso, expõe DTOs e configura os mapeamentos entre entidades e DTOs via AutoMapper.
- **Infrastructure** implementa as interfaces do Domain com Entity Framework Core (repositório genérico + repositórios especializados) e gerencia o `ApplicationDbContext`.
- **API** expõe os endpoints REST via controllers e injeta as dependências das camadas inferiores.

---

## Estrutura de Pastas do Frontend

```
frontend/src/
├── views/              # Páginas da aplicação (uma por rota)
│   ├── DashboardView.vue
│   ├── EmpresasView.vue
│   ├── EmpresaDetalhesView.vue
│   ├── FornecedoresView.vue
│   └── FornecedorDetalhesView.vue
├── components/         # Componentes reutilizáveis
├── stores/             # Estado global com Pinia
│   ├── empresaStore.ts
│   ├── fornecedorStore.ts
│   └── themeStore.ts
├── services/           # Comunicação com a API
│   ├── api.ts          # Instância configurada do Axios
│   ├── empresaService.ts
│   ├── fornecedorService.ts
│   └── cepService.ts   # Integração com ViaCEP
├── types/              # Interfaces e tipos TypeScript
├── router/             # Definição das rotas (Vue Router)
├── plugins/            # Configuração de plugins (Vuetify)
├── utils/              # Funções utilitárias (validadores de CPF/CNPJ)
├── __tests__/          # Testes unitários (Vitest)
├── App.vue             # Componente raiz
└── main.ts             # Ponto de entrada da aplicação
```

---

## Pré-requisitos

**Com Docker:**
- [Docker](https://docs.docker.com/get-docker/) e Docker Compose instalados

---

## Como executar

### Docker Compose

Sobe o banco de dados e a API automaticamente:

```bash
docker-compose build
docker-compose up -d
```

A API ficará disponível em `http://localhost:5253` e o Swagger em `http://localhost:5253/swagger`.
As migrations são aplicadas automaticamente na inicialização.

Em seguida, inicie o frontend separadamente:

```bash
cd frontend
npm install
npm run dev
```

O frontend estará em `http://localhost:5173`.

---

## Testes

**Backend:**
```bash
cd DesafioFullstack.Tests
dotnet test
```

**Frontend:**
```bash
cd frontend
npm run test
```

---

## Telas e fluxo da aplicação

### Telas disponíveis

| Tela | Rota | Descrição |
|---|---|---|
| **Dashboard** | `/` | Visão geral com totais de empresas e fornecedores, atalhos de navegação e tabela com as empresas mais recentes |
| **Listagem de Empresas** | `/empresas` | Grid com todas as empresas cadastradas, busca por nome/CNPJ/cidade/estado, cadastro e edição via modal, exclusão com confirmação |
| **Detalhes da Empresa** | `/empresas/:id` | Dados completos da empresa e lista de fornecedores vinculados, com opções para vincular e desvincular fornecedores |
| **Listagem de Fornecedores** | `/fornecedores` | Grid com todos os fornecedores, busca por nome/CPF/CNPJ, cadastro e edição via modal, exclusão com confirmação |
| **Detalhes do Fornecedor** | `/fornecedores/:id` | Dados completos do fornecedor (incluindo RG e idade para PF) e lista de empresas vinculadas |

### Fluxo da aplicação

```
Dashboard (/)
├── → Listagem de Empresas (/empresas)
│        ├── → Detalhes da Empresa (/empresas/:id)
│        │        └── → Detalhes do Fornecedor (/fornecedores/:id)
│        └── Modal criar/editar empresa
│
└── → Listagem de Fornecedores (/fornecedores)
         ├── → Detalhes do Fornecedor (/fornecedores/:id)
         │        └── → Detalhes da Empresa (/empresas/:id)
         └── Modal criar/editar fornecedor
```

Os formulários de empresa e fornecedor realizam autocomplete de endereço ao digitar o CEP (via ViaCEP). O formulário de fornecedor exibe campos adicionais de RG e data de nascimento quando o tipo selecionado é Pessoa Física.

---

## Observações

### API de CEP

Para validação de CEP e autocomplete de endereço nos formulários, foi utilizada a **[ViaCEP](https://viacep.com.br/)** em substituição à API sugerida pelo desafio, devido à instabilidade desta última durante o desenvolvimento.
