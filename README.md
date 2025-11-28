# cafedebug-backend.api

![image](https://user-images.githubusercontent.com/11943572/234849730-c6b41618-6c13-4a87-9b5e-5b9d16ba4474.png)

<p align="center">
  <img src="https://img.shields.io/badge/Framework-dotnet-blue"/> 
  <img src="https://img.shields.io/badge/Framework%20version-dotnet%209-blue"/>
  <img src="https://img.shields.io/badge/Language-C%23-blue"/> 
  <img src="https://img.shields.io/badge/Status-development-green"/>
</p>

<p align="center">
  <a href="https://github.com/RegisBarros/cafedebug-backend.api/actions/workflows/ci-cd.yml">
    <img src="https://github.com/RegisBarros/cafedebug-backend.api/actions/workflows/ci-cd.yml/badge.svg" alt="CI/CD Pipeline Status"/>
  </a>
  <a href="https://github.com/RegisBarros/cafedebug-backend.api/actions/workflows/deploy-on-release.yml">
    <img src="https://github.com/RegisBarros/cafedebug-backend.api/actions/workflows/deploy-on-release.yml/badge.svg" alt="Deploy Status"/>
  </a>
  <a href="https://github.com/RegisBarros/cafedebug-backend.api/releases">
    <img src="https://img.shields.io/github/v/release/RegisBarros/cafedebug-backend.api" alt="Latest Release"/>
  </a>
  <a href="https://github.com/RegisBarros/cafedebug-backend.api/pkgs/container/cafedebug-backend.api">
    <img src="https://img.shields.io/badge/registry-GHCR-blue" alt="GitHub Container Registry"/>
  </a>
</p>

 <h4 align="center"> 
	🚧  Projeto 🚀 em construção...  🚧
 </h4>

## Sobre o projeto 📑

Este é o repositório do projeto API Café Debug. Essa API tem como objetivo manter o backend separado do frontend<br/>
trazendo informações do podcast como episódios e agenda e outros conteúdos relacionado a tecnologia.<br />
[site café debug](wwww.cafedebug.com.br) atual.

## Tecnologias

Este projeto utiliza as seguintes tecnologias principais:

- .NET 9 (C#) — plataforma do backend
- Entity Framework Core — ORM para acesso ao MySQL
- MySQL — banco de dados relacional
- Docker / docker-compose — facilitação do ambiente local
- xUnit / Moq — framework de testes unitários e mocking

## Requisitos 📋

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MySQL](https://www.mysql.com/)
- [Docker](https://www.docker.com/)


## Setup 🔧

### 1. Clonar ou fazer fork do repositório
Você pode clonar o repositório diretamente ou criar um fork no GitHub e clonar seu fork. Exemplo:

```bash
git clone https://github.com/JessicaNathany/cafedebug-backend.api.git
cd cafedebug-backend.api
```

### 2. Configure o banco de dados

Para rodar a base local, faça um [clone deste projeto](https://github.com/JessicaNathany/debug-automation) e execute os comandos abaixo:

Dê permissão ao arquivo .sh:
```bash
chmod +x cafedebug-setup.sh
```

Execute o script do banco de dados:
```bash
./cafedebug-setup.sh
```

### 3. Configure o appsettings

Copie o arquivo de template:
```bash
cp appsettings.json appsettings.Development.json
```

Edite o arquivo `appsettings.Development.json` e substitua os placeholders:
```json
{
  "ConnectionStrings": {
    "CafedebugConnectionStringMySQL": "Server=localhost;Port=3306;Database=cafedebug;User=root;Password=sua-senha;"
  },
  "JwtSettings": {
    "Issuer": "https://api.cafedebug.com.br",
    "Audience": "https://cafedebug.com.br",
    "SigningKey": "sua-chave-secreta-minimo-32-caracteres-aqui",
    "ValidForMinutes": 15,
    "RefreshTokenValidForMinutes": 10080
  },
  "HealthChecksUI": {
    "HealthChecks": [
      {
        "Name": "Cafe Debug API Health",
        "Uri": "http://localhost:5000/health"
      }
    ]
  },
  "Storage": {
    "AWS": {
      "S3": {
        "Bucket": "cafedebug-images",
        "ServiceUrl": "http://localhost:9000",
        "BaseUrl": "http://localhost:9000/cafedebug-uploads",
        "Region": null,
        "ForcePathStyle": true,
        "UseHttp": true
      }
    }
  }
}
```

**⚠️ IMPORTANTE:** Nunca commite o arquivo `appsettings.Development.json` com dados reais!

#### Valores necessários:

| Placeholder                         | Descrição                                                                                | Exemplo                                                                   |
|-------------------------------------|------------------------------------------------------------------------------------------|---------------------------------------------------------------------------|
| `{connection-string}`               | String de conexão MySQL                                                                  | `Server=localhost;Port=3306;Database=cafedebug;User=root;Password=senha;` |
| `{issuer}`                          | Emissor do token JWT                                                                     | `https://api.cafedebug.com.br`                                            |
| `{audience}`                        | Audiência do token JWT                                                                   | `https://cafedebug.com.br`                                                |
| `{signing-key}`                     | Chave secreta para assinar tokens (mínimo 32 caracteres)                                 | Use uma string aleatória forte                                            |
| `{valid-for-minutes}`               | Tempo de validade do access token em minutos                                             | `15`                                                                      |
| `{refresh-token-valid-for-minutes}` | Tempo de validade do refresh token em minutos                                            | `10080` (7 dias)                                                          |
| `{health-check-uri}`                | URI do health check                                                                      | `http://localhost:5000/health`                                            |
| `{bucket}`                          | Nome do bucket do S3                                                                     | `cafedebug-images`                                                        |
| `{s3-url}`                          | Url da AWS S3 ou do MinIO                                                                | `http://localhost:9000/cafedebug-uploads`                                 |
| `{region}`                          | Região do serviço AWS (se aplicável). MinIO usar sempre `null`                           | `us-east-1` ou `null`                                                     |
| `{force-path-style}`                | Se `true`, acessa o bucket como caminho da URL (`host/bucket`). MinIO usar sempre `true` | `true` ou `false`                                                         |
| `{use-http}`                        | Se `true`, usa HTTP ao invés de HTTPS. MinIO usar sempre `true`                          | `true` ou `false`                                                         |
| `{service-url}`                     | Url do serviço do MinIO.                                                                 | `http://localhost:9000`                                                   |


### 4. Restaure as dependências
```bash
dotnet restore
```

### 5. Execute o projeto
```bash
dotnet run --project src/cafedebug-backend.api
```

A API estará disponível em: `http://localhost:5000` ou `https://localhost:5001`

## Tests 🧪
```bash
dotnet test
```

## Endpoints :clipboard: <br/>

_Auth_

- `POST /api/auth/login` - autenticação do usuário retornando um token de validação.

_BannerAdmin_

- `POST /api/banner-admin/novo-banner` - adiciona um novo banner aa área administrativa.
- `PUT /api/banner-admin/editar-banner` - edita o banner da área administrativa.
- `GET /api/banner-admin/banners` - retorna uma lilsta de banners da área administrativa.
- `GET /api/banner-admin/banner/{id}` - retorna banner por id.
- `DELETE /api/banner-admin/banner/{id}` - apaga banner por id.


Admin - Episódios

- `GET /api/v1/admin/episodes` — lista episódios
- `GET /api/v1/admin/episodes/{id}` — obtém episódio por id
- `POST /api/v1/admin/episodes` — cria episódio (Authorize)
- `PUT /api/v1/admin/episodes/{id}` — atualiza episódio (Authorize)
- `DELETE /api/v1/admin/episodes/{id}` — remove episódio (Authorize)

### Architecture

<img width="1154" height="614" alt="image" src="https://github.com/user-attachments/assets/5bfe0c95-463b-4a38-8f58-f456ba124e1d" />

## Como contribuir 🤝

Confira o guia de contribuição em [CONTRIBUTING.md](./CONTRIBUTING.md)
