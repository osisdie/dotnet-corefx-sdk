# 機密管理

本文件說明 CI/CD 管線和 SDK 在執行時所使用的機密（API 金鑰、憑證）管理方式。

## CI/CD 機密

### NUGET_API_KEY

GitHub Actions 在推送版本標籤時使用此金鑰發佈 NuGet 套件。

#### 設定步驟

1. **取得 NuGet API Key**
   - 登入 [nuget.org](https://www.nuget.org/)
   - 點右上角帳號選單 → **API Keys** → **Create**
   - 設定：
     - **Key Name**: `dotnet-corefx-sdk-ci`
     - **Package owner**: 選擇你的帳號
     - **Glob Pattern**: `CoreFX.*`（限定只能推送 CoreFX 開頭的套件）
     - **Scopes**: 勾選 **Push**
   - 點 **Create** → 複製產生的 Key（只會顯示一次）

2. **加入 GitHub Secrets**
   - 到 repo 頁面 → **Settings** → **Secrets and variables** → **Actions**
   - 點 **New repository secret**
     - **Name**: `NUGET_API_KEY`
     - **Secret**: 貼上剛才複製的 Key
   - 點 **Add secret**

#### 金鑰輪替

- NuGet API Key 預設 **365 天過期**
- 建議設定行事曆提醒，在到期前輪替
- 輪替步驟：在 nuget.org 產生新金鑰 → 更新 GitHub Secret → 撤銷舊金鑰

#### 安全注意事項

- GitHub Secrets 在來自 fork 的 PR 中**不會被注入**，外部貢獻者無法讀取
- Secrets 在 CI 日誌中會被**遮蔽** — 若值出現在輸出中，GitHub 會替換成 `***`
- `publish` 工作只在主 repo 推送 `v*` 標籤時觸發，不會在 PR 中執行

### 進階：使用 GitHub Environments

若需更嚴格的控管，可設定 `production` 環境：

1. 到 repo → **Settings** → **Environments** → **New environment**
2. 命名為 `production`
3. 啟用 **Required reviewers** — 加入需要審核的維護者
4. 將 `NUGET_API_KEY` 移至此環境的 Secrets
5. 更新 `ci.yml`：
   ```yaml
   publish:
     needs: build
     runs-on: ubuntu-latest
     environment: production    # <-- 加入此行
     if: startsWith(github.ref, 'refs/tags/v')
   ```

這樣在發佈 NuGet 前會多一道人工審核步驟。

---

## 執行時機密

SDK 透過**環境變數**讀取以下機密。絕對不要將機密寫在 `appsettings.json` 或原始碼中。

### 必要

| 變數 | 使用者 | 說明 |
|------|--------|------|
| `COREFX_API_NAME` | `SvcContext` | 應用程式識別名稱（未設定會拋出例外） |

### 選用

| 變數 | 使用者 | 說明 |
|------|--------|------|
| `COREFX_API_KEY` | `SvcContext` | 服務驗證用 API 金鑰 |
| `COREFX_API_TOKEN` | `SvcContext` | 服務驗證用 Bearer Token |
| `COREFX_DEPLOY_NAME` | `SvcContext` | 部署名稱（預設為 API 名稱） |
| `COREFX_SMTP_PWD` | `EmailService` | 寄送電子郵件用的 SMTP 密碼 |
| `ASPNETCORE_ENVIRONMENT` | `SvcContext` | 環境名稱：`Debug`、`Development`、`Testing`、`Staging`、`Production` |

### 設定環境變數

**本地開發** — 使用 `.env` 檔案或 `launchSettings.json`：

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

**Docker**：

```dockerfile
ENV COREFX_API_NAME=my-api
ENV ASPNETCORE_ENVIRONMENT=Production
```

**Kubernetes**：

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

### JWT 設定

JWT 機密透過 `IConfiguration`（通常為 `appsettings.json`）設定，但**正式環境的值應來自機密管理工具**：

```json
{
  "AuthConfig": {
    "JwtConfig": {
      "Secret": "-- 請使用機密管理工具，勿寫明文 --",
      "Issuer": "your-issuer",
      "Audience": "your-audience"
    }
  }
}
```

**建議的機密管理工具**：
- Azure Key Vault + `Azure.Extensions.AspNetCore.Configuration.Secrets`
- AWS Secrets Manager + `AWSSDK.SecretsManager`
- HashiCorp Vault
- Kubernetes Secrets（適用於 K8s 部署環境）

### 最佳實踐

1. **絕不提交機密** — 將 `*.env`、`appsettings.*.json`（`appsettings.json` 除外）加入 `.gitignore`
2. **使用強 JWT 密鑰** — 至少 32 位元組，隨機產生
3. **定期輪替** — 特別是 SMTP 密碼和 API 金鑰
4. **最小權限原則** — NuGet API Key 的 Glob Pattern 應設為 `CoreFX.*` 而非 `*`
5. **定期稽核存取權限** — 檢查誰有權限存取 GitHub Secrets 和機密管理工具
