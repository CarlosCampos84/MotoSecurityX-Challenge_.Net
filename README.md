## 🚀 CP5MotoSecurityX_.net (2TDS 2025)

Clean Architecture + DDD + EF Core + Swagger (ASP.NET Core 8)

API para controle e monitoramento de motos, pátios e usuários.
O projeto aplica Clean Architecture, DDD (Entidades ricas + VO) e boas práticas de Clean Code.

## 👥 Integrantes do Grupo

Caio Henrique – RM: 554600
Carlos Eduardo – RM: 555223
Antônio Lino – RM: 554518

## 🎯 Objetivo e Domínio

O domínio simula operações da Mottu:

Usuários: administradores/operadores do sistema.

Pátios: unidades que recebem/armazenam motos.

Motos: possuem Placa (Value Object), Modelo e podem estar dentro ou fora de um pátio.

**Regras:**

    - Placa única (constraint UNIQUE).

    - Email de usuário único (constraint UNIQUE).

    - Entrada/saída de motos em pátios via métodos de comportamento no domínio.

**Benefício de negócio:**
visibilidade de ativos, rastreio de alocação por pátio e gestão de usuários.

## 🧭 Arquitetura (Camadas)

CP4.MotoSecurityX.Api/ -> Controllers, Program.cs, Swagger, appsettings
CP4.MotoSecurityX.Application/ -> Use cases (Handlers), DTOs
CP4.MotoSecurityX.Domain/ -> Entidades, Value Objects, Interfaces (Repos)
CP4.MotoSecurityX.Infrastructure/ -> EF Core (DbContext, Migrations), Repositórios, DI

**Princípios aplicados:**

- Inversão de Dependência: interfaces no Domain; implementações no Infrastructure.

- Baixo acoplamento entre camadas; a API não referencia EF diretamente.

- Regra de negócio no domínio (métodos em entidades) + use cases no Application.

- Clean Code: SRP/DRY/KISS/YAGNI, nomes claros, controllers finos.

## 🧩 Modelagem de Domínio (DDD)

- Entidades:

  Moto (rich model):

      EntrarNoPatio(Guid patioId) 
      
      SairDoPatio() 
      
      AtualizarModelo(string)

      AtualizarPlaca(string)

  Patio (Agregado Raiz):

      Mantém coleção de motos via navegação EF.

- Value Object:

  Placa: normalizada (ex.: "ABC1D23"), validada no construtor, mapeada como owned type no EF com índice único.

✅ Status atual:

3 entidades implementadas (Usuário, Pátio, Moto)

1 VO (Placa)

CRUD completo, paginação, HATEOAS, Swagger documentado com exemplos.

🧱 Backlog de evolução futura: incluir entidade extra (ex.: Ocorrencia ou Manutencao) para enriquecer o domínio.

## 🔧 Requisitos

.NET 8 SDK

(Opcional) dotnet-ef (já incluso no dotnet-tools.json)

SQLite (desenvolvimento) ou Azure SQL (produção)

## ▶️ Como executar localmente

- Na raiz do repositório:

  # Restaurar e compilar

  dotnet restore
  dotnet build

  # (uma vez) restaurar a ferramenta local dotnet-ef

  dotnet tool restore

  # Criar/atualizar o banco Sqlite (se ainda não existir)
  dotnet ef database update -p .\CP4.MotoSecurityX.Infrastructure\ -s .\CP4.MotoSecurityX.Api\

  # Subir a API
  dotnet run --project .\CP4.MotoSecurityX.Api\

  Swagger: http://localhost:7102/swagger (a porta pode variar, confira no terminal)

  Banco: motosecurityx.db no appsettings.json

## 🌐 Endpoints (exemplos)

### Usuários
- Criar
  POST /api/usuarios
  ```json
  {
    "nome": "Admin",
    "email": "admin@mottu.com"
  }
  ```
- Listar
  GET /api/usuarios?page=1&pageSize=5

- Obter por ID
  GET /api/usuarios/{id}

- Atualizar
  PUT /api/usuarios/{id}
  ```json
  {
    "nome": "Admin Atualizado",
    "email": "admin2@mottu.com"
  }
  ```
- Deletar
  DELETE /api/usuarios/{id}

### Pátios
- Criar
  POST /api/patios
  ```json
  {
    "nome": "Pátio Central",
    "endereco": "Rua das Entregas, 100"
  }
  ```
- Listar
  GET /api/patios?page=1&pageSize=5

- Obter por ID
  GET /api/patios/{id}

- Atualizar
  PUT /api/patios/{id}
  ```json
  {
    "nome": "Pátio Mooca",
    "endereco": "Rua do Oratório, 788"
  }
  ```
- Deletar
  DELETE /api/patios/{id}

### Motos
- Criar
  POST /api/motos
  ```json
  {
    "placa": "abc1d23",
    "modelo": "Mottu 110i"
  }
  ```
- Listar
  GET /api/motos?page=1&pageSize=5

- Obter por ID
  GET /api/motos/{id}

- Mover para Pátio
  POST /api/motos/{id}/mover
  ```json
  {
    "patioId": "PASTE_AQUI_O_GUID_DO_PATIO"
  }
  ```

- Atualizar
  PUT /api/motos/{id}
  ```json
  {
    "modelo": "Mottu 125i",
    "placa": "XYZ9A88"
  }
  ```
- Deletar
  DELETE /api/motos/{id}

## 🗃️ Persistência & Migrations

EF Core 8 + SQLite (dev)

Azure SQL (cloud)

Migration InitialCreate e AddUsuarios em Infrastructure/Data/Migrations

Connection string configurável via appsettings.json ou variáveis de ambiente.

## 📜 Swagger / OpenAPI

Swagger UI habilitado em Development.

Todos os endpoints documentados com:

    [SwaggerOperation] (sumário/descrição)

    [SwaggerRequestExample] (exemplos de payloads)

    [ProducesResponseType] (status codes)

DTOs descritos automaticamente nos Schemas.

## 🧼 Clean Code

SRP/DRY/KISS/YAGNI

Controllers finos, DTOs + Handlers

Nomes claros e métodos pequenos

ExceptionMiddleware simples para tratar erros previsíveis (ex.: duplicidade → 409 Conflict).

## 📋 Testes

O roteiro detalhado de testes da API está disponível em:  
[/docs/MotoSecurityX-Challenge_.net_roteiro_de_testes.md](./docs/MotoSecurityX-Challenge_.net_roteiro_de_testes.md)

## 📄 Licença

Uso educacional/acadêmico.

## 🌟 Propósito

“Código limpo sempre parece que foi escrito por alguém que se importa.” — Uncle Bob
