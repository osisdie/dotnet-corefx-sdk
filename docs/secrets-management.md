# Secrets Management

This document covers how to manage secrets (API keys, credentials) used by the CI/CD pipeline and the SDK at runtime.

## CI/CD Secrets

### NUGET_API_KEY

Used by GitHub Actions to push NuGet packages on version tag.

#### Setup

1. **Get a NuGet API Key**
   - Sign in to [nuget.org](https://www.nuget.org/)
   - Go to account menu → **API Keys** → **Create**
   - Configure:
     - **Key Name**: `dotnet-corefx-sdk-ci`
     - **Package owner**: your account
     - **Glob Pattern**: `CoreFX.*` (restricts to CoreFX packages only)
     - **Scopes**: check **Push**
   - Click **Create** → copy the key (shown only once)

2. **Add to GitHub**
   - Go to repo → **Settings** → **Secrets and variables** → **Actions**
   - Click **New repository secret**
     - **Name**: `NUGET_API_KEY`
     - **Secret**: paste the key
   - Click **Add secret**

#### Key Rotation

- NuGet API keys expire after **365 days** by default
- Set a calendar reminder to rotate before expiry
- To rotate: generate a new key on nuget.org → update the GitHub secret → revoke the old key

#### Security Notes

- GitHub Secrets are **not exposed** in pull requests from forks
- Secrets are **masked** in CI logs — if the value appears in output, GitHub replaces it with `***`
- The `publish` job only runs on `v*` tags pushed to the main repo, not on PRs

### Optional: GitHub Environments

For stricter control, configure a `production` environment:

1. Go to repo → **Settings** → **Environments** → **New environment**
2. Name it `production`
3. Enable **Required reviewers** — add maintainers who must approve before publish
4. Move `NUGET_API_KEY` to this environment's secrets
5. Update `ci.yml`:
   ```yaml
   publish:
     needs: build
     runs-on: ubuntu-latest
     environment: production    # <-- add this line
     if: startsWith(github.ref, 'refs/tags/v')
   ```

This adds a manual approval step before any NuGet publish.

---

## Runtime Secrets

The SDK reads these secrets from **environment variables** at runtime. Never put them in `appsettings.json` or source code.

### Required

| Variable | Used By | Description |
|----------|---------|-------------|
| `COREFX_API_NAME` | `SvcContext` | Application identifier (throws if missing) |

### Optional

| Variable | Used By | Description |
|----------|---------|-------------|
| `COREFX_API_KEY` | `SvcContext` | API key for service authentication |
| `COREFX_API_TOKEN` | `SvcContext` | Bearer token for service authentication |
| `COREFX_DEPLOY_NAME` | `SvcContext` | Deployment name (defaults to API name) |
| `COREFX_SMTP_PWD` | `EmailService` | SMTP password for sending emails |
| `ASPNETCORE_ENVIRONMENT` | `SvcContext` | Environment name: `Debug`, `Development`, `Testing`, `Staging`, `Production` |

### Setting Environment Variables

**Local development** — use `.env` file or `launchSettings.json`:

```json
// Properties/launchSettings.json
{
  "profiles": {
    "MyApp": {
      "environmentVariables": {
        "COREFX_API_NAME": "my-api",
        "ASPNETCORE_ENVIRONMENT": "Debug"
      }
    }
  }
}
```

**Docker**:

```dockerfile
ENV COREFX_API_NAME=my-api
ENV ASPNETCORE_ENVIRONMENT=Production
```

**Kubernetes**:

```yaml
env:
  - name: COREFX_API_NAME
    value: "my-api"
  - name: COREFX_SMTP_PWD
    valueFrom:
      secretKeyRef:
        name: smtp-credentials
        key: password
```

### JWT Configuration

JWT secrets are configured via `IConfiguration` (typically `appsettings.json`), but **production values should come from a secret manager**:

```json
{
  "AuthConfig": {
    "JwtConfig": {
      "Secret": "-- use secret manager, not plaintext --",
      "Issuer": "your-issuer",
      "Audience": "your-audience"
    }
  }
}
```

**Recommended secret managers**:
- Azure Key Vault + `Azure.Extensions.AspNetCore.Configuration.Secrets`
- AWS Secrets Manager + `AWSSDK.SecretsManager`
- HashiCorp Vault
- Kubernetes Secrets (for K8s deployments)

### Best Practices

1. **Never commit secrets** — add `*.env`, `appsettings.*.json` (except `appsettings.json`) to `.gitignore`
2. **Use strong JWT secrets** — at least 32 bytes, randomly generated
3. **Rotate regularly** — especially SMTP passwords and API keys
4. **Least privilege** — NuGet API key glob pattern should be `CoreFX.*`, not `*`
5. **Audit access** — review who has access to GitHub secrets and secret managers periodically
