# dotnet-corefx-sdk

[English](README.md) | 繁體中文

模組化 .NET SDK，用於建構 WebAPI — 驗證、快取、日誌、中介軟體等。

## 模組

| 套件 | 說明 | 依賴 |
|------|------|------|
| **CoreFX.Abstractions** | 基礎介面、DTO、擴充方法、序列化器 | 無（基礎層） |
| **CoreFX.Common** | SvcContext、字串/日期/檔案工具方法 | Abstractions |
| **CoreFX.Auth** | JWT 權杖產生、驗證、Session 管理 | Abstractions |
| **CoreFX.Hosting** | ASP.NET Core 中介軟體（例外處理、JWT 授權、請求/回應記錄、Swagger） | Auth, Common |
| **CoreFX.Caching.Redis** | 使用 StackExchange.Redis 的 IDistributedCache | Common |
| **CoreFX.Logging.Log4net** | log4net 的 ILoggerProvider 轉接器 | 獨立 |
| **CoreFX.Notification** | 透過 MailKit 的電子郵件服務，支援排程報表 | Abstractions |
| **CoreFX.DataAccess.Mapper** | AutoMapper 整合與 DI 擴充 | Abstractions |

## 依賴關係圖

```
CoreFX.Abstractions           （基礎層）
  |-- CoreFX.Common            -> Abstractions
  |-- CoreFX.Auth              -> Abstractions
  |-- CoreFX.DataAccess.Mapper -> Abstractions
  |-- CoreFX.Notification      -> Abstractions
  |-- CoreFX.Caching.Redis     -> Common
  |-- CoreFX.Logging.Log4net   （獨立）
  '-- CoreFX.Hosting           -> Auth + Common
```

## 目標框架

- `net8.0`（.NET 8 LTS）
- `net10.0`（.NET 10）

## 快速開始

```bash
# 安裝套件
dotnet add package CoreFX.Abstractions
dotnet add package CoreFX.Hosting
dotnet add package CoreFX.Auth
# ... 依需求加入其他模組
```

### Startup.cs / Program.cs

```csharp
// 設定中介軟體管線
app.UseExceptionHandlerMiddleware();
app.UseJwtAuthorization();
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestResponseLogging();

// 健康檢查
app.MapHealthChecks("/health");
```

## 使用此 SDK 的範本專案

| 範本 | 重點 |
|------|------|
| [dotnet-webapi-jwt-boilerplate](https://github.com/osisdie/dotnet-webapi-jwt-boilerplate) | JWT 驗證、Dapper、Redis |
| [dotnet-minikube-boilerplate](https://github.com/osisdie/dotnet-minikube-boilerplate) | Kubernetes 部署、Entity Framework |
| [dotnet-mediatr-boilerplate](https://github.com/osisdie/dotnet-mediatr-boilerplate) | MediatR CQRS、FluentValidation、AutoMapper |

## 建置與測試

```bash
dotnet build CoreFX.slnx
dotnet test CoreFX.slnx
dotnet pack -c Release
```

## 發佈流程

透過 GitHub Actions 自動化：

1. 更新 `CHANGELOG.md`
2. 建立並推送版本標籤：
   ```bash
   git tag v1.1.0
   git push origin v1.1.0
   ```
3. CI 將自動建置、測試、打包並發佈至 NuGet

## 貢獻

請參閱 [CONTRIBUTING.md](CONTRIBUTING.md) 了解如何貢獻。

## 授權

[MIT](LICENSE)
