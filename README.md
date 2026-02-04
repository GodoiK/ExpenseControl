# ExpenseControl (Fullstack)

- **Backend:** ASP.NET Core 8 + EF Core + PostgreSQL
- **Frontend:** React + TypeScript (Vite build) servido via Nginx
- **Sem login:** apenas o tomador (você) cadastra pessoas/categorias e registra lançamentos.

## Subir com Docker

Na raiz do projeto:

```bash
docker compose down -v
docker compose up --build
```

## Acessos

- Swagger: http://localhost:7214/swagger
- Frontend: http://localhost:5173

## Regras implementadas

- **Usuário não digita GUID:** o frontend seleciona a pessoa e envia o `userId` na rota.
- **Evitar duplicados:** bloqueia criar usuário com **mesmo Nome + Idade** (também tem índice único no banco).
- **Menor de idade:** não pode registrar **Income**.
- **Descrição opcional:** se vier vazia/branca, o backend salva como **"Sem descrição"**.
- **Categoria x Tipo:** se o tipo é **despesa**, só aceita categoria com finalidade **despesa** ou **ambas**; se o tipo é **receita**, só aceita finalidade **receita** ou **ambas**.

## Banco de dados

O backend executa `db.Database.Migrate()` ao iniciar, aplicando as migrations incluídas no projeto.
