# CI/CD 配置說明

本專案使用 GitHub Actions 實現 CI/CD 自動化流程。

## 🔄 工作流程

### 1. 簡化版 CI (`ci-simple.yml`)
- **觸發**: Push 到 master 分支或 Pull Request
- **流程**:
  - ✅ 建置專案
  - ✅ 執行所有測試
  - ✅ 產生測試報告

### 2. 完整版 CI/CD (`ci-cd.yml`)
- **觸發**: 多分支策略
- **流程**:
  - 🧪 測試階段
  - 🐳 Docker 建置
  - 🚀 自動部署
  - 🔍 程式碼品質檢查
  - 🛡️ 安全掃描
  - 📈 效能測試
  - 📧 通知

## 📋 設定 GitHub Secrets

為了讓 CI/CD 正常運作，需要在 GitHub 專案設定以下 Secrets：

### 基本設定
```
DOCKER_USERNAME=your-docker-username
DOCKER_PASSWORD=your-docker-password
RAILWAY_TOKEN=your-railway-token
```

### 進階設定 (可選)
```
SONAR_TOKEN=your-sonarcloud-token
SLACK_WEBHOOK=your-slack-webhook-url
```

## 🚀 部署策略

### Development
- 任何 Push 到 `develop` 分支觸發測試
- 部署到開發環境

### Staging
- Pull Request 觸發完整測試套件
- 部署到測試環境進行驗證

### Production
- Push 到 `master` 分支觸發生產部署
- 包含安全掃描和效能測試

## 📊 測試覆蓋率

CI/CD 會自動產生測試覆蓋率報告：
- 單元測試覆蓋率
- 整合測試結果
- 程式碼品質指標

## 🔍 品質閘門

每個 Pull Request 必須通過：
- ✅ 所有測試通過
- ✅ 程式碼覆蓋率 > 80%
- ✅ 無安全漏洞
- ✅ 程式碼品質檢查通過

## 📈 監控和通知

### 成功部署
- Slack 通知
- 電子郵件通知
- GitHub Status 更新

### 失敗處理
- 自動回滾機制
- 錯誤通知
- 詳細錯誤報告

## 🛠️ 本地測試

在推送前，建議本地執行：

```bash
# 建置測試
dotnet build

# 執行所有測試
dotnet test

# 程式碼格式檢查
dotnet format --verify-no-changes

# Docker 建置測試
docker build -t recipe-api-test .
```

## 🔄 工作流程觸發條件

| 分支 | 觸發動作 | 執行內容 |
|------|----------|----------|
| `master` | Push | 完整 CI/CD + 生產部署 |
| `develop` | Push | CI 測試 + 開發環境部署 |
| `feature/*` | Pull Request | CI 測試 + 品質檢查 |

## 📝 新增工作流程

要新增自定義工作流程：

1. 在 `.github/workflows/` 建立新的 `.yml` 檔案
2. 定義觸發條件和工作流程
3. 設定必要的 Secrets
4. 測試和部署

## 🎯 最佳實踐

### 測試策略
- 快速測試在 CI 執行
- 長時間測試在夜間執行
- 效能測試在 Pull Request 觸發

### 部署策略
- Blue-Green 部署
- Canary 釋出
- Feature Flags

### 安全策略
- 依賴漏洞掃描
- 容器映像掃描
- 程式碼靜態分析
