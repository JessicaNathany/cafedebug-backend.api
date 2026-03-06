# CafeDebug API
🇺🇸 English | 🇧🇷 [Português](README.pt-BR.md)

![image](https://user-images.githubusercontent.com/11943572/234849730-c6b41618-6c13-4a87-9b5e-5b9d16ba4474.png)

<p align="center">
  <img src="https://img.shields.io/badge/Framework-dotnet-blue"/> 
  <img src="https://img.shields.io/badge/Framework%20version-dotnet%209-blue"/>
  <img src="https://img.shields.io/badge/Language-C%23-blue"/> 
  <img src="https://img.shields.io/badge/Status-development-green"/>
</p>

<p align="center">
  <a href="https://github.com/JessicaNathany/cafedebug-backend.api/actions/workflows/ci-cd.yml">
    <img src="https://github.com/JessicaNathany/cafedebug-backend.api/actions/workflows/ci-cd.yml/badge.svg?branch=main" alt="CI/CD Pipeline Status"/>
  </a>
  <a href="https://github.com/JessicaNathany/cafedebug-backend.api/releases">
    <img src="https://img.shields.io/github/v/release/JessicaNathany/cafedebug-backend.api?display_name=tag" alt="Latest Release"/>
  </a>
  <a href="https://github.com/JessicaNathany/cafedebug-backend.api/pkgs/container/cafedebug-backend.api">
    <img src="https://img.shields.io/badge/registry-GHCR-blue" alt="GitHub Container Registry"/>
  </a>
</p>

 <h4 align="center"> 
	🚧  Project 🚀 under construction...  🚧
 </h4>

## About the project 📑

This repository contains the Café Debug API project. The purpose of this API is to keep the backend separate from the frontend, providing information about the podcast such as episodes and schedule, as well as other technology‑related content. [Café Debug website](wwww.cafedebug.com.br) current.

## Technologies

This project uses the following main technologies:

- .NET 9 (C#) — backend platform
- Entity Framework Core — ORM for MySQL access
- MySQL — relational database
- Docker / docker-compose — ease local environment
- xUnit / Moq — unit testing and mocking framework

## Requirements 📋

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MySQL](https://www.mysql.com/)
- [Docker](https://www.docker.com/)

## Setup 🔧

### 1. Clone or fork the repository
You can clone the repository directly or create a fork on GitHub and clone your fork. Example:

```bash
git clone https://github.com/JessicaNathany/cafedebug-backend.api.git
cd cafedebug-backend.api
```

### 2. Configure the database

To run the local database, clone [this project](https://github.com/JessicaNathany/debug-automation) and execute the commands below:

Give permission to the script:
```bash
chmod +x cafedebug-setup.sh
```

Execute the database script:
```bash
./cafedebug-setup.sh
```

### 3. Configure the appsettings

Copy the template file:
```bash
cp appsettings.json appsettings.Development.json
```

Edit `appsettings.Development.json` and replace the placeholders:
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

**⚠️ IMPORTANT:** Never commit the `appsettings.Development.json` file with real data!

#### Required values:

| Placeholder                         | Description                                                                                | Example                                                                   |
|-------------------------------------|--------------------------------------------------------------------------------------------|---------------------------------------------------------------------------|
| `{connection-string}`               | MySQL connection string                                                                    | `Server=localhost;Port=3306;Database=cafedebug;User=root;Password=senha;` |
| `{issuer}`                          | JWT token issuer                                                                           | `https://api.cafedebug.com.br`                                            |
| `{audience}`                        | JWT token audience                                                                         | `https://cafedebug.com.br`                                                |
| `{signing-key}`                     | Secret key for signing tokens (minimum 32 characters)                                     | Use a strong random string                                                |
| `{valid-for-minutes}`               | Access token validity time in minutes                                                      | `15`                                                                      |
| `{refresh-token-valid-for-minutes}` | Refresh token validity time in minutes                                                     | `10080` (7 days)                                                          |
| `{health-check-uri}`                | Health check URI                                                                           | `http://localhost:5000/health`                                            |
| `{bucket}`                          | S3 bucket name                                                                             | `cafedebug-images`                                                        |
| `{s3-url}`                          | AWS S3 or MinIO URL                                                                        | `http://localhost:9000/cafedebug-uploads`                                 |
| `{region}`                          | Service region (if applicable). MinIO always `null`                                       | `us-east-1` or `null`                                                     |
| `{force-path-style}`                | If `true`, access the bucket using path-style URL (`host/bucket`). For MinIO always `true` | `true` or `false`                                                         |
| `{use-http}`                        | If `true`, use HTTP instead of HTTPS. For MinIO always `true`                             | `true` or `false`                                                         |
| `{service-url}`                     | MinIO service URL.                                                                         | `http://localhost:9000`                                                   |

### 4. Restore dependencies
```bash
dotnet restore
```

### 5. Run the project
```bash
dotnet run --project src/cafedebug-backend.api
```

The API will be available at: `http://localhost:5000` or `https://localhost:5001`

## Tests 🧪
```bash
dotnet test
```

## Endpoints :clipboard: <br/>

_Auth_

- `POST /api/auth/login` - user authentication returning a validation token.

_BannerAdmin_

- `POST /api/banner-admin/novo-banner` - adds a new banner to the admin area.
- `PUT /api/banner-admin/editar-banner` - edits the banner in the admin area.
- `GET /api/banner-admin/banners` - returns a list of banners for the admin area.
- `GET /api/banner-admin/banner/{id}` - returns banner by id.
- `DELETE /api/banner-admin/banner/{id}` - deletes banner by id.

Admin - Episodes

- `GET /api/v1/admin/episodes` — lists episodes
- `GET /api/v1/admin/episodes/{id}` — gets episode by id
- `POST /api/v1/admin/episodes` — creates episode (Authorize)
- `PUT /api/v1/admin/episodes/{id}` — updates episode (Authorize)
- `DELETE /api/v1/admin/episodes/{id}` — removes episode (Authorize)

### Architecture

<img width="1154" height="614" alt="image" src="https://github.com/user-attachments/assets/5bfe0c95-463b-4a38-8f58-f456ba124e1d" />

### Project Structure

The project follows **Clean Architecture** principles with clear separation of concerns organized into four main layers:

#### Layer Organization

```
src/
├── cafedebug-backend.api/           # API Layer (Presentation)
│   ├── Controllers/                 # API endpoints and request handling
│   ├── Filters/                     # Custom filters and middlewares
│   └── Program.cs                   # Application entry point and configuration
│
├── cafedebug.backend.application/   # Application Layer
│   ├── Accounts/                    # Account-related use cases
│   ├── Audience/                    # Audience-related use cases
│   ├── Banners/                     # Banner management use cases
│   ├── Content/                     # Content-related use cases
│   ├── Media/                       # Media handling use cases
│   ├── Podcasts/                    # Podcast management use cases
│   └── Common/                      # Shared application logic (DTOs, validators, mappers)
│
├── cafedebug-backend.domain/        # Domain Layer (Business Logic)
│   ├── Accounts/                    # Account entities and business rules
│   ├── Audience/                    # Audience entities and business rules
│   ├── Banners/                     # Banner entities and business rules
│   ├── Messages/                    # Domain events and messages
│   ├── Podcasts/                    # Podcast entities and business rules
│   └── Shared/                      # Base classes and shared domain logic
│
└── cafedebug-backend.infrastructure/# Infrastructure Layer
    ├── Database/                    # Entity Framework Core DbContext and migrations
    ├── Repositories/                # Data access implementations
    ├── Services/                    # External service integrations (S3, etc)
    └── Configuration/               # Infrastructure setup and configuration
```

#### Layer Responsibilities

- **API Layer** (`cafedebug-backend.api`): Handles HTTP requests/responses, routing, and request validation
- **Application Layer** (`cafedebug.backend.application`): Implements business use cases, DTOs, and application logic
- **Domain Layer** (`cafedebug-backend.domain`): Contains core business entities, value objects, and domain rules
- **Infrastructure Layer** (`cafedebug-backend.infrastructure`): Manages database access, external APIs, and persistence concerns

#### Feature-Based Organization

Each feature (Accounts, Banners, Podcasts, etc.) is organized consistently across all layers:
- **Domain**: Entity definitions and business rules
- **Application**: Use cases and DTOs
- **API**: Controllers and endpoints

This structure enables easy feature addition and maintains clear boundaries between layers.

## How to contribute 🤝

See the contribution guide in [CONTRIBUTING.md](./CONTRIBUTING.md)
