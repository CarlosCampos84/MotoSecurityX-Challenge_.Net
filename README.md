
## 🚀 CP5MotoSecurityX_.net (2TDS 2025)


ASP.NET Core 8 + Clean Architecture + DDD + MongoDB + Health Checks + Swagger com Versionamento

API para controle e monitoramento de motos, pátios e usuários, evoluída do CP4 para o CP5 com:

    - MongoDB (conexão e CRUD completo),

    - Health Check para app e banco,
     
    - Swagger/OpenAPI com versionamento (v1/v2),
    
    - Clean Architecture + DDD + Clean Code.

## 👥 Integrantes do Grupo

Caio Henrique – RM: 554600
Carlos Eduardo – RM: 555223
Antônio Lino – RM: 554518

## 🎯 Objetivo e Domínio

Domínio inspirado nas operações da Mottu.

    Usuários: administradores/operadores.

    - Regra: e-mail único (índice único).

    Pátios: unidades que recebem/armazenam motos.

    Motos: possuem Placa (Value Object), Modelo e podem estar dentro/fora de pátio.

        Regra: placa única (índice único).

        Comportamentos de domínio:

            EntrarNoPatio(Guid patioId)
        
            SairDoPatio()
        
            AtualizarModelo(string)
        
            AtualizarPlaca(string)

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

- DDD: Entidades ricas + VO Placa; regras encapsuladas.

- Interfaces de repositório no Domain (ISP & DIP) e implementações no Infrastructure.

- Controllers finos, Handlers orquestram casos de uso.

- SRP/DRY/KISS/YAGNI, nomeação clara, validações consistentes.

ℹ️ Artefatos de SQLite/EF do CP4 podem existir no histórico/pasta de migrations por legado; a persistência ativa no CP5 é MongoDB.

## 🧩 Modelagem de Domínio (DDD)

- Entidades:

    - Usuario { Id, Nome, Email }
  
    - Patio { Id, Nome, Endereco } (agregado raiz com relação às motos)
  
    - Moto { Id, Modelo, Placa (VO), PatioId? }

Value Object

    - Placa

        - Normaliza (ex.: ABC1D23)
         
        - Valida no construtor (formato mercosul)
         
        - Comparação por valor

Repositórios (interfaces em Domain)

    - IUsuarioRepository
     
    - IPatioRepository
     
    - IMotoRepository

Implementações (Infrastructure)

    - Repositórios Mongo (MongoCollection<T>, índices únicos para usuarios.email e motos.placa).

## 🗃️ Persistência – MongoDB (CP5)

Suporte MongoDB Atlas ou local.

**Variáveis de ambiente**

  -  MONGODB_URI – string de conexão
    
  -  MONGODB_DATABASE – nome do banco (ex.: motosecurityx)
    
    A API lê essas variáveis (com fallback configurável em appsettings.Development.json).

**Índices (criados no startup)**

    - usuarios.email – unique
     
    - motos.placa – unique

**Subir MongoDB local**

```
docker run -d --name mongo \
-p 27017:27017 \
-e MONGO_INITDB_DATABASE=motosecurityx \
mongo:7
```
**OU**

```
net start MongoDB

sc query MongoDB
```

## ❤️‍🩹 Health Checks

    - Endpoint: GET /health

    - Checks:

        Liveness (processo)

        MongoDB (conectividade e ping – AddMongoDb())

    - Resposta: JSON com status Healthy/Degraded/Unhealthy

    Opcional: HealthChecks UI pode ser plugado se desejar dashboard visual.

## 📜 Swagger + Versionamento

 - Versionamento por endpoint com API Versioning:

    v1 (estável) e v2 (evolução)


 - Documentos:

    /swagger/v1/swagger.json

    /swagger/v2/swagger.json

 
- UI: /swagger com seletor de versão

 - Anotações: SwaggerOperation, ProducesResponseType, payload examples em SwaggerExamples/.


## ▶️ Como executar localmente

**1) Pré-requisitos** 

    .NET 8 SDK

    MongoDB (local via Docker ou Atlas)

    Opcional: HTTP REPL/Insomnia/Postman

**2) Configurar ambiente** 

Crie um .env (ou defina no sistema):

```
MONGODB_URI=mongodb://localhost:27017
MONGODB_DATABASE=motosecurityx
ASPNETCORE_ENVIRONMENT=Development
```

Opcional (appsettings.Development.json):

```
{
"Mongo": {
"ConnectionString": "mongodb://localhost:27017",
"Database": "motosecurityx"
},
"Swagger": {
"Title": "MotoSecurityX API",
"Description": "CP5 - Motos, Pátios e Usuários com MongoDB, Health e Versionamento",
"Contact": { "Name": "2TDS 2025" }
}
}
```

**3) Restaurar, compilar e subir**

```
   dotnet restore
   dotnet build
   dotnet run --project .\CP4.MotoSecurityX.Api\
```

Saídas esperadas (exemplos do console):

    Now listening on: http://localhost:5102

    GET /swagger/v1/swagger.json → 200

    GET /health → 200 (Healthy)

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

SRP: classes/métodos com responsabilidade única

DRY: reutilização de DTOs/helpers

KISS/YAGNI: soluções objetivas, sem over-engineering

Controllers finos: orquestram Handlers

Exceções de domínio mapeadas para HTTP adequado

## 📦 Organização do GitHub & Commits

Commits semânticos:

    feat(motos): mover moto para pátio

    fix(usuarios): e-mail duplicado retorna 409

    docs(readme): instruções CP5


Estrutura enxuta, sem artefatos temporários

README atualizado e Swagger funcional (v1/v2)

## 📋 Testes

O roteiro detalhado de testes da API está disponível em:  
[/docs/MotoSecurityX-Challenge_.net_roteiro_de_testes.md](./docs/MotoSecurityX-Challenge_.net_roteiro_de_testes.md)

# 🧠 Status de Conformidade com o CP5

Clean Architecture ✔️

DDD (Entidades ricas + VO + agregado raiz) ✔️

Clean Code (SRP/DRY/KISS/YAGNI) ✔️

MongoDB (Conexão + CRUD + índices únicos) ✔️

Health Check (app + MongoDB) ✔️

Swagger + Versionamento (v1/v2) ✔️

Organização do GitHub + commits semânticos ✔️

## 📄 Licença

Uso educacional/acadêmico.

## 🌟 Propósito

“Código limpo sempre parece que foi escrito por alguém que se importa.” — Uncle Bob
