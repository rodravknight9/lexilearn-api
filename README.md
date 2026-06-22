# Lexilearn

Lexilearn is a language-learning REST API for managing flashcard decks, cards, and practice sessions. It integrates with LibreTranslate for text translation.

## Projects

| Project | Description |
|---------|-------------|
| `Lexilearn.WebApi` | ASP.NET Core 9 Web API — main entry point |
| `Lexilearn.Application` | CQRS handlers, contracts, and business logic (MediatR) |
| `Lexilearn.Domain` | Domain entities and enums |
| `Lexilearn.MySql` | EF Core persistence (MySQL) |
| `Lexilearn.Identity` | JWT authentication and user management |
| `Lexilearn.LibreTranslate` | LibreTranslate HTTP integration |
| `Lexilearn.DataTransfer` | API request/response DTOs |
| `Lexilearn.Shared` | Cross-cutting shared types |

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- MySQL server with `LexilearnDb` and `IdentityDb` databases
- [LibreTranslate](https://libretranslate.com/) instance (for translation features)
- EF Core CLI tools (`dotnet tool install --global dotnet-ef`)

## Getting Started

1. Clone the repository and open `Lexilearn.sln` in Visual Studio or your preferred IDE.
2. Update connection strings and service settings in `Lexilearn.WebApi/appsettings.json` (or use User Secrets for local development).
3. Apply database migrations:
   ```bash
   dotnet ef database update --project Lexilearn.MySql
   dotnet ef database update --project Lexilearn.Identity
   ```
4. Run the API:
   ```bash
   dotnet run --project Lexilearn.WebApi
   ```
   The API is available at `http://localhost:5288` (HTTP) or `https://localhost:7241` (HTTPS). Swagger UI is enabled in Development at `/swagger`.

## Architecture

For a full description of the solution architecture, design patterns, technology stack, API endpoints, and deployment details, see **[docs/ARCHITECTURE.md](docs/ARCHITECTURE.md)**.
