# TubeMonitor API

API RESTful para monitoramento de canais e vídeos do YouTube, desenvolvida em C# .NET 10 com Entity Framework Core.

## Integrantes

| Nome | RM |
|------|----|
| Nome do integrante 1 | RM00000 |
| Nome do integrante 2 | RM00000 |
| Nome do integrante 3 | RM00000 |
| Nome do integrante 4 | RM00000 |
| Nome do integrante 5 | RM00000 |

## Contexto do projeto

**O que é:** uma API para cadastrar canais do YouTube e acompanhar as métricas dos seus vídeos (visualizações, curtidas e comentários).

**Problema que resolve:** criadores de conteúdo têm dificuldade para acompanhar o desempenho de canais concorrentes e identificar quais vídeos estão ganhando tração. Essas informações ficam espalhadas e não há um lugar centralizado para comparar os números.

**Para quem é destinado:** criadores de conteúdo, agências e analistas de marketing digital que precisam acompanhar canais de um nicho.

**Principais recursos:**
- Cadastro de canais monitorados, com filtro por nicho
- Cadastro de vídeos vinculados a cada canal
- Cálculo automático da **taxa de engajamento** ((curtidas + comentários) / visualizações)
- Cálculo da **média de views por dia** desde a publicação
- Ranking de **vídeos em alta**, ordenado por views por dia

## Tecnologias

- .NET 10 / ASP.NET Core Web API (Controllers)
- Entity Framework Core 10
- Asp.Versioning (versionamento por URL)
- OpenAPI + Swagger UI

## Banco de dados

**SQLite**, escolhido por não exigir instalação de servidor: o arquivo `tubemonitor.db` é criado automaticamente na primeira execução.

Tabelas:
- `Canais`: Id, Nome, Handle (único), Nicho, Inscritos, CadastradoEm, AtualizadoEm
- `Videos`: Id, CanalId (FK), YoutubeVideoId (único), Titulo, Visualizacoes, Curtidas, Comentarios, PublicadoEm, CadastradoEm, AtualizadoEm

Relacionamento: um canal possui vários vídeos (1:N), com exclusão em cascata. O banco já inicia com 3 canais e 5 vídeos de exemplo (seed).

## Estrutura do projeto

```
src/TubeMonitor.Api/
├── Controllers/V1/     Endpoints da versão 1
├── Data/               AppDbContext, configurações das entidades e Migrations
├── DTOs/               Objetos de entrada (Request) e saída (Response)
├── Exceptions/         Exceções de domínio e tratador global de erros
├── Mappings/           Conversão entre entidades e DTOs
├── Models/             Entidades (Canal, Video)
├── Services/           Regras de negócio e acesso a dados via EF Core
└── Program.cs          Configuração da aplicação
```

## Como rodar localmente

**Pré-requisito:** [.NET 10 SDK](https://dotnet.microsoft.com/download)

```bash
git clone <url-do-repositorio>
cd tubemonitor-api/src/TubeMonitor.Api
dotnet run
```

Acesse o Swagger em: **http://localhost:5080/swagger**

As migrations são aplicadas automaticamente ao iniciar a aplicação.

## Migrations

A migration `InitialCreate` está em `src/TubeMonitor.Api/Data/Migrations` e cria as tabelas `Canais` e `Videos` com os dados iniciais.

Comandos utilizados:

```bash
# Instalar a ferramenta do EF Core (uma vez)
dotnet tool install --global dotnet-ef

# Criar a migration
dotnet ef migrations add InitialCreate -o Data/Migrations

# Aplicar no banco manualmente (opcional, a API já faz isso ao iniciar)
dotnet ef database update
```

## Endpoints

Base URL: `http://localhost:5080/api/v1`

### Canais

| Método | Rota | Descrição | Status |
|--------|------|-----------|--------|
| GET | `/api/v1/canais` | Lista os canais (filtro opcional `?nicho=`) | 200 |
| GET | `/api/v1/canais/{id}` | Busca um canal pelo id | 200, 404 |
| GET | `/api/v1/canais/{id}/videos` | Lista os vídeos de um canal | 200, 404 |
| POST | `/api/v1/canais` | Cadastra um canal | 201, 400, 409 |
| PUT | `/api/v1/canais/{id}` | Atualiza um canal | 204, 400, 404, 409 |
| DELETE | `/api/v1/canais/{id}` | Remove um canal e seus vídeos | 204, 404 |

### Vídeos

| Método | Rota | Descrição | Status |
|--------|------|-----------|--------|
| GET | `/api/v1/videos` | Lista todos os vídeos | 200 |
| GET | `/api/v1/videos/em-alta?top=10` | Ranking de vídeos por views/dia (top de 1 a 50) | 200, 400 |
| GET | `/api/v1/videos/{id}` | Busca um vídeo pelo id | 200, 404 |
| POST | `/api/v1/videos` | Cadastra um vídeo | 201, 400, 409 |
| PUT | `/api/v1/videos/{id}` | Atualiza um vídeo | 204, 400, 404, 409 |
| DELETE | `/api/v1/videos/{id}` | Remove um vídeo | 204, 404 |

### Exemplos de corpo

**POST /api/v1/canais**
```json
{
  "nome": "Viagem Barata",
  "handle": "@viagembarata",
  "nicho": "Viagem",
  "inscritos": 58000
}
```

**POST /api/v1/videos**
```json
{
  "canalId": 1,
  "youtubeVideoId": "Lm3nOp7QrSt",
  "titulo": "Versionamento de APIs em .NET",
  "visualizacoes": 5200,
  "curtidas": 610,
  "comentarios": 48,
  "publicadoEm": "2026-09-15T18:00:00Z"
}
```

O arquivo `src/TubeMonitor.Api/TubeMonitor.Api.http` contém todas as requisições prontas para teste.

## Status codes e tratamento de erros

| Código | Quando ocorre |
|--------|---------------|
| 200 OK | Consulta realizada com sucesso |
| 201 Created | Recurso criado (retorna o header `Location`) |
| 204 No Content | Atualização ou exclusão realizada |
| 400 Bad Request | Dados inválidos ou regra de negócio violada |
| 404 Not Found | Recurso não encontrado |
| 409 Conflict | Handle de canal ou ID de vídeo já cadastrado |
| 500 Internal Server Error | Erro inesperado (sem exposição de detalhes internos) |

Todos os erros seguem o padrão **ProblemDetails** (RFC 9457), tratados de forma centralizada pelo `GlobalExceptionHandler`.

Regras de negócio validadas:
- O handle do canal deve começar com `@` e ser único
- O ID do vídeo no YouTube deve ter 11 caracteres e ser único
- A data de publicação não pode estar no futuro
- Curtidas não podem ser maiores que visualizações
- O canal informado no cadastro de vídeo deve existir

## Evidências de testes

Prints do Swagger em `docs/evidencias/`.

### Canais
| Teste | Print |
|-------|-------|
| GET /canais (200) | ![](docs/evidencias/01-get-canais.png) |
| GET /canais/{id} (200) | ![](docs/evidencias/02-get-canal-id.png) |
| GET /canais/{id} (404) | ![](docs/evidencias/03-get-canal-404.png) |
| GET /canais/{id}/videos (200) | ![](docs/evidencias/04-get-videos-canal.png) |
| POST /canais (201) | ![](docs/evidencias/05-post-canal-201.png) |
| POST /canais (400) | ![](docs/evidencias/06-post-canal-400.png) |
| PUT /canais/{id} (204) | ![](docs/evidencias/07-put-canal-204.png) |
| DELETE /canais/{id} (204) | ![](docs/evidencias/08-delete-canal-204.png) |

### Vídeos
| Teste | Print |
|-------|-------|
| GET /videos (200) | ![](docs/evidencias/09-get-videos.png) |
| GET /videos/em-alta (200) | ![](docs/evidencias/10-get-em-alta.png) |
| GET /videos/{id} (200) | ![](docs/evidencias/11-get-video-id.png) |
| POST /videos (201) | ![](docs/evidencias/12-post-video-201.png) |
| POST /videos (400) | ![](docs/evidencias/13-post-video-400.png) |
| PUT /videos/{id} (204) | ![](docs/evidencias/14-put-video-204.png) |
| DELETE /videos/{id} (204) | ![](docs/evidencias/15-delete-video-204.png) |
