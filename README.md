# 💰 ExpenseControl - API de Controle de Despesas Pessoais

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat&logo=csharp)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC292B?style=flat&logo=microsoftsqlserver&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0-512BD4?style=flat)
![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?style=flat&logo=swagger&logoColor=black)

Uma Web API RESTful desenvolvida em **C# / .NET** projetada para gerenciamento e controle de finanças pessoais. A aplicação permite cadastrar, categorizar e acompanhar receitas e despesas, fornecendo métricas para auxílio na tomada de decisões financeiras.

---

## 🎯 Objetivo do Projeto

O **ExpenseControl** foi desenvolvido para solucionar a necessidade de organização financeira individual através de uma solução robusta, escalável e segura no backend. O foco principal do projeto é aplicar boas práticas de desenvolvimento de software, padrões de arquitetura e manipulação eficiente de dados relacionais.

---

## ⚙️ Funcionalidades Principais

- **Gestão de Usuários:** Cadastro, autenticação e controle de permissões.
- **Controle de Transações:**
  - Registro de Entradas (Receitas) e Saídas (Despesas).
  - Categorização personalizada (ex: Alimentação, Transporte, Moradia, Lazer).
  - Associação de formas de pagamento (Cartão de Crédito, PIX, Boleto, Dinheiro).
- **Relatórios e Resumos Financeiros:**
  - Consulta de saldo consolidado e histórico por período (mensal/anual).
  - Agrupamento de gastos por categoria.
- **Validações de Negócio:**
  - Impede lançamentos com valores inválidos ou datas futuras incoerentes.
  - Garante o isolamento dos dados entre diferentes usuários.

---

## 🛠️ Tecnologias e Ferramentas

- **Linguagem & Framework:** C# 12 / .NET 8 ASP.NET Core Web API
- **Persistência de Dados:** Entity Framework Core (Approach *Code-First* / Migrations)
- **Banco de Dados:** Microsoft SQL Server
- **Validações:** FluentValidation
- **Mapeamento:** AutoMapper
- **Documentação da API:** Swagger UI / OpenAPI
- **Autenticação & Segurança:** JWT (JSON Web Token) e BCrypt para hash de senhas

---

## 🏗️ Arquitetura e Boas Práticas

A aplicação segue os princípios da **Clean Architecture** (ou Arquitetura em Camadas), promovendo o baixo acoplamento e alta coesão entre os componentes:

```text
ExpenseControl/
├── src/
│   ├── ExpenseControl.Domain/          # Entidades, Interfaces de Repositório e Regras de Negócio
│   ├── ExpenseControl.Application/     # Casos de Uso, DTOs, Mapeamentos e Validações
│   ├── ExpenseControl.Infrastructure/  # DbContext, Repositórios, Migrations e Serviços Externos
│   └── ExpenseControl.API/             # Controllers, Middlewares, Configurações e Swagger
└── tests/
    └── ExpenseControl.Tests/           # Testes Unitários e de Integração'''
