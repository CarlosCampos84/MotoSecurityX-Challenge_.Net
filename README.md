
## 🚀 MotoSecurityX_.net — Challenge (2TDS 2025)


ASP.NET Core 8 + Clean Architecture + DDD + MongoDB/SQLite (toggle) + Health Checks + Swagger com Versionamento + API Key + ML.NET

API para controle e monitoramento de motos, pátios e usuários, evoluída do CP4 para o CP5 com foco em boas práticas REST, observabilidade e segurança.


## 👥 Integrantes do Grupo

Caio Henrique – RM: 554600 |
Carlos Eduardo – RM: 555223  |
Antônio Lino – RM: 554518

## 🎯 Domínio (Mottu-like)

Domínio inspirado nas operações da Mottu.

    Usuários: administradores/operadores do sistema.

        Regra: e-mail único (índice único).

    Pátios: unidades que recebem/armazenam motos.

    Motos: Placa (Value Object) + Modelo + podem estar dentro/fora de um pátio.

        Regras: placa única (índice único).

    Comportamentos principais (DDD)
        EntrarNoPatio(Guid patioId), SairDoPatio(), AtualizarModelo(string), AtualizarPlaca(string).

Benefício: visibilidade de ativos, alocação por pátio e gestão de usuários.

## 🧭 Arquitetura (Camadas)

```
src/
├─ CP4.MotoSecurityX.Api/          # Endpoints, validações, versionamento, Swagger
│   ├─ Configuration/               # ApiVersioningExtensions, SwaggerExtensions, etc.
│   ├─ Controllers/                 # Motos, Pátios, Usuários, WeatherForecast/Health
│   └─ SwaggerExamples/             # Exemplos de payloads (Swashbuckle.Filters)
├─ CP4.MotoSecurityX.Application/   # Casos de uso (Handlers), DTOs, PagedResult
├─ CP4.MotoSecurityX.Domain/        # Entidades, Value Objects, Repositórios (interfaces)
└─ CP4.MotoSecurityX.Infrastructure/# Persistência (MongoDB), Repos concretos, DI
```

**Princípios aplicados:**

    DIP/ISP, controllers finos, handlers orquestram casos de uso, SRP/DRY/KISS/YAGNI.

ℹ️ Artefatos de SQLite/EF do CP4 podem existir no histórico/pasta de migrations por legado; a persistência ativa no CP5 e Sprint 4 é MongoDB.

## 🧩 ⚙️ Configuração de Ambiente

-   Variáveis/appsettings.*.json

```
{
    "UseMongo": true,
    "Mongo": {
      "ConnectionString": "mongodb://localhost:27017",
      "Database": "motosecurityx"
    },
    "ConnectionStrings": {
      "Default": "Data Source=motosecurityx.db"
    },
    "ApiKeyAuth": {
      "ApiKeyHeaderName": "X-API-KEY",
      "ApiKey": "SUA_CHAVE_SUPER_SECRETA"
    },
    "Https": {
      "EnableRedirection": false
    }
}
```

 - UseMongo: true → Mongo; false → SQLite.

 - ApiKeyAuth: nome do header + chave usada em produção/local.

 - Health: redirecionamento HTTPS é opcional.

**Subir Mongo local (opções)**

 - Docker:

        docker run -d --name mongo -p 27017:27017 -e MONGO_INITDB_DATABASE=motosecurityx mongo:7


 - Ou serviço instalado:

        net start MongoDB
        
        sc query MongoDB


## ▶️ Executar Localmente

    dotnet restore
    
    dotnet build

    dotnet run --project .\CP4.MotoSecurityX.Api\

- Swagger UI: http://localhost:<porta>/swagger

- Versionamento: /swagger/v1/swagger.json (…/v2 quando aplicável)


## 🔐 Autorização no Swagger (API Key)

 - Header: X-API-KEY

 - Valor: sua chave configurada em ApiKeyAuth:ApiKey.

 - Config no arquivo:

 ```
    "ApiKeyAuth": {
        "ApiKeyHeaderName": "X-API-KEY",
    "ApiKey": "SUA_CHAVE_SUPER_SECRETA"
    }
 ```

**Como testar:**

 - dotnet clean && dotnet run --project .\CP4.MotoSecurityX.Api\

 - Abrir /swagger → botão Authorize → informar a chave.

 - Chamar GET /api/v1/usuarios e POST /api/v1/usuarios (deve responder 200/201).

## ❤️‍🩹 Health Checks

    GET /health/live → Liveness (processo OK)

    GET /health/ready → Readiness (ex.: Mongo/SQLite OK)

    GET /health → Consolidado

 Resposta em JSON (compatível com HealthChecks UI).

## 📜 Swagger + Versionamento

 - Versionamento por segmento de URL: api/v1/...

 - UI agrupa por versão: /swagger → selecione v1 (e v2 quando habilitado).

 - Endpoints anotados com SwaggerOperation, ProducesResponseType, exemplos (quando aplicável).

## 🤖 ML.NET — Sentiment

 - Endpoint: POST /api/v1/ml/sentiment

 - Body:


    { "text": "serviço excelente e muito bom!" }


 - Resposta:

       { "isPositive": true, "score": 0.95 }


## 🌐 Endpoints (v1)
    Base: /api/v1

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


## 🧼 Clean Code

 - SRP/DRY/KISS/YAGNI

 - Entidades ricas + VO Placa (normaliza/valida Mercosul)

 - Repos no Domain, implementações no Infrastructure

 - Controllers finos; Handlers coordenam casos de uso

 - Exceções de domínio → HTTP apropriado
 - 

## 📦 Organização do GitHub & Commits

 - Commits semânticos (ex.: feat(motos): mover moto para pátio)

 - README sempre atualizado

 - Swagger funcional e versionado


## 📋 Roteiro de Testes

Documento detalhado:  
[/docs/MotoSecurityX-Challenge_.net_roteiro_de_testes.md](./docs/MotoSecurityX-Challenge_.net_roteiro_de_testes.md)

# ✅ Conformidade com a Rubrica (Sprint 4)

 - Health Checks (10 pts) ✔️

 - Versionamento da API (10 pts) ✔️

 - Segurança (API Key) (25 pts) ✔️

 - Endpoint com ML.NET (25 pts) ✔️

 - Testes xUnit + Integração + Instruções no README (30 pts) ✔️

Atenção às penalidades

 - Swagger atualizado ✔️

 - Projeto compila ✔️

 - README atualizado ✔️

## 📄 Licença

Uso educacional/acadêmico.

## 🌟 Propósito

“Código limpo sempre parece que foi escrito por alguém que se importa.” — Uncle Bob
