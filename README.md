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

## Observações

### API de CEP

Para validação de CEP e autocomplete de endereço nos formulários, foi utilizada a **[ViaCEP](https://viacep.com.br/)** em substituição à API sugerida pelo desafio, devido à instabilidade desta última durante o desenvolvimento.
