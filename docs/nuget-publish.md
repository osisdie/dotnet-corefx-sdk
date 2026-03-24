# NuGet Publishing Guide

This project uses GitHub Actions to automatically publish NuGet packages when a version tag is pushed.

## How It Works

```
git tag v1.2.3  →  CI extracts "1.2.3"  →  dotnet pack -p:Version=1.2.3  →  dotnet nuget push
```

The `publish` job in `.github/workflows/ci.yml` triggers on `v*` tags and:

1. Extracts the version from the tag (e.g., `v1.2.3` → `1.2.3`)
2. Builds all projects with that version
3. Packs all 8 modules into `.nupkg` files
4. Pushes to [nuget.org](https://www.nuget.org/) using `NUGET_API_KEY`
5. Creates a GitHub Release with the `.nupkg` artifacts attached

## Publishing a New Version

### 1. Update CHANGELOG.md

Add a new version section following [Keep a Changelog](https://keepachangelog.com/) format:

```markdown
## [1.2.3] - 2026-04-01

### Added
- New feature description

### Fixed
- Bug fix description
```

### 2. Commit and Tag

```bash
git add CHANGELOG.md
git commit -m "Prepare release v1.2.3"
git tag v1.2.3
git push origin main --tags
```

### 3. Verify

- Check the [Actions tab](https://github.com/osisdie/dotnet-corefx-sdk/actions) for CI status
- Verify packages appear on [nuget.org](https://www.nuget.org/profiles/osisdie)

## Packages Published

Each tag push publishes all 8 packages with the same version:

| Package | NuGet |
|---------|-------|
| `CoreFX.Abstractions` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.Abstractions)](https://www.nuget.org/packages/CoreFX.Abstractions) |
| `CoreFX.Common` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.Common)](https://www.nuget.org/packages/CoreFX.Common) |
| `CoreFX.Auth` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.Auth)](https://www.nuget.org/packages/CoreFX.Auth) |
| `CoreFX.Hosting` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.Hosting)](https://www.nuget.org/packages/CoreFX.Hosting) |
| `CoreFX.Caching.Redis` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.Caching.Redis)](https://www.nuget.org/packages/CoreFX.Caching.Redis) |
| `CoreFX.Logging.Log4net` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.Logging.Log4net)](https://www.nuget.org/packages/CoreFX.Logging.Log4net) |
| `CoreFX.Notification` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.Notification)](https://www.nuget.org/packages/CoreFX.Notification) |
| `CoreFX.DataAccess.Mapper` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.DataAccess.Mapper)](https://www.nuget.org/packages/CoreFX.DataAccess.Mapper) |

## Version Strategy

- Version is **not** hardcoded in source — `Directory.Build.props` has a default `1.0.0`, but CI overrides it with `-p:Version=` from the git tag
- Follow [Semantic Versioning](https://semver.org/):
  - **MAJOR** — breaking API changes
  - **MINOR** — new features, backward-compatible
  - **PATCH** — bug fixes, backward-compatible

## Troubleshooting

| Problem | Solution |
|---------|----------|
| `403 Forbidden` on push | API key expired or lacks push scope — regenerate and update secret |
| `409 Conflict` on push | Package version already exists — `--skip-duplicate` handles this silently |
| Packages not appearing | NuGet indexing can take 5–15 minutes after push |
| Wrong version in package | Ensure tag format is `v1.2.3` (with `v` prefix) |
