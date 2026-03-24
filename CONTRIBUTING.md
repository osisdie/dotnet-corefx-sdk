# Contributing to CoreFX SDK

Thank you for your interest in contributing! This guide will help you get started.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Git

### Setup

```bash
git clone https://github.com/osisdie/dotnet-corefx-sdk.git
cd dotnet-corefx-sdk
dotnet restore CoreFX.slnx
dotnet build CoreFX.slnx
dotnet test CoreFX.slnx
```

## How to Contribute

### Reporting Bugs

- Search [existing issues](https://github.com/osisdie/dotnet-corefx-sdk/issues) first to avoid duplicates
- Open a new issue with a clear title and description
- Include steps to reproduce, expected behavior, and actual behavior
- Add the target framework version (`net8.0` or `net10.0`)

### Suggesting Features

- Open an issue with the `enhancement` label
- Describe the use case and why the feature would be valuable
- If possible, outline a proposed API design

### Submitting Pull Requests

1. **Fork** the repository and create a branch from `main`
2. **Follow** the existing code style (see `.editorconfig`)
3. **Add tests** for any new functionality — each module has a corresponding test project under `tests/`
4. **Run** the full test suite before submitting:
   ```bash
   dotnet test CoreFX.slnx
   ```
5. **Write** a clear PR description explaining what changed and why
6. **Keep** PRs focused — one feature or fix per PR

### Commit Messages

Use clear, descriptive commit messages:

```
Add JWT token refresh validation for expired tokens

- Validate refresh token expiration before generating new access token
- Return descriptive error when refresh token is expired
```

## Project Structure

```
src/
  CoreFX.Abstractions/     # Foundation — interfaces, DTOs, extensions
  CoreFX.Common/           # String/file utilities
  CoreFX.Auth/             # JWT authentication
  CoreFX.Hosting/          # ASP.NET Core middleware
  CoreFX.Caching.Redis/    # Redis caching
  CoreFX.Logging.Log4net/  # log4net adapter
  CoreFX.Notification/     # Email services
  CoreFX.DataAccess.Mapper/ # AutoMapper integration
tests/
  CoreFX.*.Tests/          # One test project per module
```

## Coding Guidelines

- Follow the `.editorconfig` settings
- Use `var` when the type is apparent
- Prefer extension methods for cross-cutting concerns
- Wrap service responses in `SvcResponseDto<T>` with proper `Success()` / `Error()` status
- Add `[ModuleInitializer]` in test projects that reference `CoreFX.Abstractions` to set `COREFX_API_NAME`

## Testing

- Each module has a corresponding test project: `CoreFX.{Module}.Tests`
- Shared test dependencies are in `tests/Directory.Build.props`
- Use `[Fact]` for single-case tests, `[Theory]` with `[InlineData]` for parameterized tests
- Mock external dependencies (SMTP, Redis) — test only validation logic and DI registration
- For JWT tests, use `JwtTestHelper` to set up in-memory `IConfiguration`

## Releasing

Releases are automated via GitHub Actions:

1. Update `CHANGELOG.md` with the new version
2. Create and push a version tag:
   ```bash
   git tag v1.1.0
   git push origin v1.1.0
   ```
3. CI will build, test, pack, and publish to NuGet automatically

## License

By contributing, you agree that your contributions will be licensed under the [MIT License](LICENSE).
