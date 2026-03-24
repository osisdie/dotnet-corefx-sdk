# dotnet-corefx-sdk

[![CI](https://github.com/osisdie/dotnet-corefx-sdk/actions/workflows/ci.yml/badge.svg)](https://github.com/osisdie/dotnet-corefx-sdk/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/CoreFX.Abstractions)](https://www.nuget.org/packages/CoreFX.Abstractions)
[![License](https://img.shields.io/github/license/osisdie/dotnet-corefx-sdk)](LICENSE)

English | [繁體中文](README.zh-TW.md)

Modular .NET SDK for building WebAPIs — authentication, caching, logging, hosting middleware, and more.

## Modules

| Package | Description | Dependencies |
|---------|-------------|--------------|
| **CoreFX.Abstractions** | Base interfaces, DTOs, extensions, serializers | None (foundation) |
| **CoreFX.Common** | SvcContext, string/datetime/file utilities | Abstractions |
| **CoreFX.Auth** | JWT token generation, validation, session management | Abstractions |
| **CoreFX.Hosting** | ASP.NET Core middleware (exception handling, JWT authorization, request/response logging, Swagger) | Auth, Common |
| **CoreFX.Caching.Redis** | IDistributedCache with StackExchange.Redis, failback scoring | Common |
| **CoreFX.Logging.Log4net** | ILoggerProvider adapter for log4net | Standalone |
| **CoreFX.Notification** | Email services via MailKit with scheduled reporting | Abstractions |
| **CoreFX.DataAccess.Mapper** | AutoMapper integration and DI extensions | Abstractions |

## Dependency Graph

```
CoreFX.Abstractions           (foundation)
  |-- CoreFX.Common            -> Abstractions
  |-- CoreFX.Auth              -> Abstractions
  |-- CoreFX.DataAccess.Mapper -> Abstractions
  |-- CoreFX.Notification      -> Abstractions
  |-- CoreFX.Caching.Redis     -> Common
  |-- CoreFX.Logging.Log4net   (standalone)
  '-- CoreFX.Hosting           -> Auth + Common
```

## Target Frameworks

- `net8.0` (.NET 8 LTS)
- `net10.0` (.NET 10 LTS)

## Quick Start

```bash
# Install packages
dotnet add package CoreFX.Abstractions
dotnet add package CoreFX.Hosting
dotnet add package CoreFX.Auth
# ... add modules as needed
```

### Startup.cs / Program.cs

```csharp
// Configure middleware pipeline
app.UseExceptionHandlerMiddleware();
app.UseJwtAuthorization();
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestResponseLogging();

// Health check
app.MapHealthChecks("/health");
```

## Boilerplates Using This SDK

| Boilerplate | Focus |
|-------------|-------|
| [dotnet-webapi-jwt-boilerplate](https://github.com/osisdie/dotnet-webapi-jwt-boilerplate) | JWT authentication, Dapper, Redis |
| [dotnet-minikube-boilerplate](https://github.com/osisdie/dotnet-minikube-boilerplate) | Kubernetes deployment, Entity Framework |
| [dotnet-mediatr-boilerplate](https://github.com/osisdie/dotnet-mediatr-boilerplate) | MediatR CQRS, FluentValidation, AutoMapper |

## Building

```bash
dotnet build CoreFX.slnx
dotnet test
dotnet pack -c Release
```

## License

[MIT](LICENSE)
