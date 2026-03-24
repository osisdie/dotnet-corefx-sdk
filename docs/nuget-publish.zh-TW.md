# NuGet 發佈指南

本專案使用 GitHub Actions 在推送版本標籤時自動發佈 NuGet 套件。

## 運作方式

```
git tag v1.2.3  →  CI 擷取 "1.2.3"  →  dotnet pack -p:Version=1.2.3  →  dotnet nuget push
```

`.github/workflows/ci.yml` 中的 `publish` 工作會在 `v*` 標籤時觸發，流程如下：

1. 從標籤擷取版本號（例如 `v1.2.3` → `1.2.3`）
2. 以該版本號建置所有專案
3. 將 8 個模組打包成 `.nupkg` 檔案
4. 使用 `NUGET_API_KEY` 推送至 [nuget.org](https://www.nuget.org/)
5. 建立 GitHub Release 並附加 `.nupkg` 產物

## 發佈新版本

### 1. 更新 CHANGELOG.md

依照 [Keep a Changelog](https://keepachangelog.com/) 格式新增版本區段：

```markdown
## [1.2.3] - 2026-04-01

### Added
- 新功能說明

### Fixed
- 錯誤修正說明
```

### 2. 提交並建立標籤

```bash
git add CHANGELOG.md
git commit -m "Prepare release v1.2.3"
git tag v1.2.3
git push origin main --tags
```

### 3. 驗證

- 到 [Actions 頁面](https://github.com/osisdie/dotnet-corefx-sdk/actions) 確認 CI 狀態
- 到 [nuget.org](https://www.nuget.org/profiles/osisdie) 確認套件已上架

## 發佈的套件

每次標籤推送會以相同版本號發佈全部 8 個套件：

| 套件 | NuGet |
|------|-------|
| `CoreFX.Abstractions` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.Abstractions)](https://www.nuget.org/packages/CoreFX.Abstractions) |
| `CoreFX.Common` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.Common)](https://www.nuget.org/packages/CoreFX.Common) |
| `CoreFX.Auth` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.Auth)](https://www.nuget.org/packages/CoreFX.Auth) |
| `CoreFX.Hosting` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.Hosting)](https://www.nuget.org/packages/CoreFX.Hosting) |
| `CoreFX.Caching.Redis` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.Caching.Redis)](https://www.nuget.org/packages/CoreFX.Caching.Redis) |
| `CoreFX.Logging.Log4net` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.Logging.Log4net)](https://www.nuget.org/packages/CoreFX.Logging.Log4net) |
| `CoreFX.Notification` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.Notification)](https://www.nuget.org/packages/CoreFX.Notification) |
| `CoreFX.DataAccess.Mapper` | [![NuGet](https://img.shields.io/nuget/v/CoreFX.DataAccess.Mapper)](https://www.nuget.org/packages/CoreFX.DataAccess.Mapper) |

## 版本策略

- 版本號**不寫死在原始碼中** — `Directory.Build.props` 有預設值 `1.0.0`，但 CI 會透過 `-p:Version=` 從 git 標籤覆寫
- 遵循[語意化版本](https://semver.org/lang/zh-TW/)：
  - **MAJOR** — 不相容的 API 變更
  - **MINOR** — 新增功能，向下相容
  - **PATCH** — 錯誤修正，向下相容

## 疑難排解

| 問題 | 解決方式 |
|------|----------|
| 推送時出現 `403 Forbidden` | API Key 過期或缺少推送權限 — 重新產生並更新 Secret |
| 推送時出現 `409 Conflict` | 套件版本已存在 — `--skip-duplicate` 會自動略過 |
| 套件未出現在 NuGet | NuGet 索引需要 5–15 分鐘 |
| 套件版本號錯誤 | 確認標籤格式為 `v1.2.3`（需有 `v` 前綴） |
