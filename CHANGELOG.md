# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.0] - 2026-03-24

### Added

- **CoreFX.Abstractions** — Base interfaces, DTOs (`ResponseBaseDto`, `SvcResponseDto<T>`), fluent extensions (`Success()`, `Error()`, `SetData()`), enums (`SvcCodeEnum`), `LazySingleton<T>`, `SdkRuntime`, `SvcContext`, logging utilities, serializers
- **CoreFX.Common** — String extensions (`ToInt`, `ToBool`, `ToMD5`, `MaskLeft`, `MaskRight`), file I/O utilities, `NetworkUtil`
- **CoreFX.Auth** — JWT token generation and validation (`JwtUtil`), session management (`SessionAdmin`), HS256 signing, dual token strategy (access + refresh)
- **CoreFX.Hosting** — ASP.NET Core middleware: `ExceptionHandler_Middleware`, `JwtAuthorization_Middleware`, `RequestResponseLogging_Middleware`, `ValidationActionFilter`, Swagger integration
- **CoreFX.Caching.Redis** — `IDistributedCache` registration with StackExchange.Redis via `AddRedisCache()`
- **CoreFX.Logging.Log4net** — `ILoggerProvider` adapter bridging log4net with `Microsoft.Extensions.Logging`
- **CoreFX.Notification** — Email service via MailKit (`EmailService.SendAsync()`), scheduled reporting support
- **CoreFX.DataAccess.Mapper** — AutoMapper integration (`AutoMapperProvider`), auto-discovery of profiles, DI extensions
- **xUnit test suite** — 174 unit tests across 8 test projects covering all modules
- **CI/CD** — GitHub Actions workflow with build, test, pack, and NuGet publish on version tags
- Multi-target support: `net8.0` and `net10.0`

[Unreleased]: https://github.com/osisdie/dotnet-corefx-sdk/compare/v1.0.0...HEAD
[1.0.0]: https://github.com/osisdie/dotnet-corefx-sdk/releases/tag/v1.0.0
