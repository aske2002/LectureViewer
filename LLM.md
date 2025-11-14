Project name: LectureViewer (monorepo)

Purpose
- LectureViewer is a web application for hosting, browsing, and interacting with lecture videos and related learning resources. It supports uploading lecture content, playing media, auto-generated transcripts and flashcards, keyword-based searching, course and lecture management, and role-based access control for users.

High-level architecture
- Monorepo with two main apps: a React-based Single Page Application (Client) and an ASP.NET Core backend (Server).
- Client: modern TypeScript + React app built with Vite. Communicates with the Server via a typed web API client. Includes UI components for courses, lectures, transcript viewer, media player, flashcard generation, upload dialogs, and search.
- Server: ASP.NET Core Web API (C#) with layered architecture (Application, Domain, Infrastructure, Web). Uses Entity Framework Core for persistence, exposes REST endpoints and serves the client app. Infra contains IaC Bicep templates for Azure.
- Tests: unit, integration, and acceptance tests live under `tests/` and cover Application, Domain, Infrastructure, and Web layers respectively.
- Dev infra: `azure.yaml` and `infra/main.bicep` provide Azure deployment scaffolding.

Key folders and files (important ones)
- Client/
  - `package.json`, `vite.config.ts`, `src/main.tsx`: Vite + React entry.
  - `src/api/*`: typed web API client and hooks (use-course-api, use-lecture-api, file upload).
  - `src/components/*`: UI components (media player, lecture header/tabs, flashcard generator/list/viewer, upload dialogs).
  - `src/db/schema.ts`, `src/db/index.ts`: client-side database (local dev data).
  - `src/lib/*`: helpers (auth, config, mock-data, generate-flashcards).
  - README.md: front-end docs.
- Server/
  - `Server.sln`, `src/`: the backend solution
  - `src/Application/`: application services, DTOs, use-cases.
  - `src/Domain/`: domain entities, value objects, domain services.
  - `src/Infrastructure/`: EF Core, data access, external integrations.
  - `src/Web/Program.cs`: ASP.NET Core host and middleware (entry point).
  - `azure.yaml`, `infra/main.bicep`: Azure deployment templates.
  - `build.cake`, `Directory.Build.props`: build and versioning helpers.
- tests/
  - `Application.UnitTests`, `Domain.UnitTests`, `Infrastructure.IntegrationTests`, `Web.AcceptanceTests`: test projects with their own dependencies and runners.
- wwwroot/api/specification.json: OpenAPI/Swagger specification for the Server.

Primary responsibilities per module
- Client: rendering UI, local caching, calling web API, handling uploads, media playback controls, and generating flashcards from transcripts.
- Server: authentication & authorization, business logic implementing use-cases, persistence via EF Core, file handling for uploads, serving the client, and providing OpenAPI.
- Infra: provisioning cloud resources (App Service / Container Apps, storage, databases) and deployment configuration.
- Tests: CI-quality checks and acceptance scenarios that exercise real behavior and integrations.

Important features
- Upload lecture content (video, slides, transcripts).
- Media player with transcript sync and chapters.
- Auto-generation of flashcards from transcripts.
- Keyword sidebar and search across lectures and transcripts.
- Course and lecture CRUD with role-based access.
- Typed client API hooks for easier front-end development.
- Bicep-based infra templates and an `azure.yaml` pipeline.

Technology stack
- Client: TypeScript, React, Vite, Yarn, TanStack Query, likely some UI primitives and custom components.
- Server: .NET 9 / ASP.NET Core, C#, Entity Framework Core, MediatR-style application layer (or equivalent), xUnit/MSTest/NUnit for tests.
- Database: EF Core (backed by SQL Server / Azure SQL in production).
- Infra/Deployment: Bicep templates and `azure.yaml` for AZD/CI.
- Local and CI tooling: dotnet CLI, EF tools, yarn, Vite.

Development & common commands
- Frontend
  - Install deps:
    - Yarn: yarn install
  - Run dev server (Vite):
    - yarn start
  - Build production:
    - yarn build
- Backend
  - Build:
    - dotnet build --configuration Debug --no-restore --output '<path>/Server/src/Web/bin/Debug/net9.0'
  - Run (from Web):
    - dotnet run
  - Add EF Core migration (task exists in workspace tasks):
    - dotnet ef migrations add <MigrationName> --project src/Infrastructure --startup-project src/Web --output-dir Data/Migrations
  - Update DB:
    - dotnet ef database update --project src/Infrastructure --startup-project src/Web
- Tests
  - Run .NET tests:
    - dotnet test
  - Frontend tests (if present): yarn test (depends on setup)

Environment & configuration hints
- ASPNETCORE_ENVIRONMENT: Development for local dev (set in the Vite dev task in workspace).
- Connection strings and secrets: stored in environment variables or Azure Key Vault in production. Check `src/Web/appsettings*.json` or `Program.cs` for configuration patterns.
- Azure deployment: `azure.yaml` + `infra/main.bicep` drive cloud resource creation and pipeline.

Quick developer contract (2-4 bullets)
- Inputs: uploaded lecture files (video, slides, transcript), API requests from the client to create/read/update lectures and courses.
- Outputs: persisted lecture metadata and files, playable media and transcripts served to clients, generated flashcards, and search results.
- Error modes: file upload errors, transcript parse errors, DB migration/version mismatch, auth failures.
- Success criteria: end-to-end upload -> playback -> transcript + flashcard generation works; API returns correct data and tests pass.

Likely edge cases
- Missing or malformed transcripts.
- Large video file uploads and timeouts.
- DB migrations out of sequence in staging/prod.
- Auth/role mismatch causing forbidden errors.
- Concurrent edits to course/lecture metadata.

Tests & quality gates
- The repository already organizes tests by layer. Use dotnet test and run Web.AcceptanceTests to verify end-to-end behavior. After code changes, run build and tests:
  - dotnet build
  - dotnet test
  - yarn build (frontend) for static hosting verification

## Compact ChatGPT-ready summary (paste this when asking for help)
LectureViewer is a monorepo with a Vite + React frontend (Client) and an ASP.NET Core backend (Server). The frontend (TypeScript) uses typed API hooks and components for courses, lectures, media playback, transcript viewing, flashcard generation, search, and upload dialogs. The backend (C#, .NET 9) follows a layered architecture (Application, Domain, Infrastructure, Web), uses EF Core for persistence, provides REST APIs (OpenAPI spec at specification.json), and contains Bicep templates for Azure infra (`infra/main.bicep`) and `azure.yaml` CI/deploy config. Common workflows: run `yarn start` in Client for frontend dev, `dotnet run` in Web for backend, `dotnet ef migrations add` / `dotnet ef database update` for DB migrations, and `dotnet test` / `yarn build` for verification. Main concerns: handling large uploads, transcript generation reliability, and DB migrations across environments.