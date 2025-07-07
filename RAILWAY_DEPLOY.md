# Railway 部署指南 - 配方管理 API

## 🚀 快速部署到 Railway

### 第一步：準備 GitHub 倉庫

1. **推送程式碼到 GitHub**:
   ```bash
   git init
   git add .
   git commit -m "初始提交：配方管理 API 支援 Railway PostgreSQL 部署"
   git branch -M master
   git remote add origin https://github.com/你的用戶名/RecipeManagementApi.git
   git push -u origin master
   ```

### 第二步：建立 Railway 專案

1. **登入 Railway**: https://railway.app/dashboard
2. **建立新專案**: 點擊 "New Project"
3. **選擇從 GitHub 部署**: "Deploy from GitHub repo"
4. **選擇 RecipeManagementApi 倉庫**

### 第三步：添加 PostgreSQL 資料庫

1. **在專案中點擊 "+ New"**
2. **選擇 "Database" → "Add PostgreSQL"**
3. **等待資料庫服務啟動**

### 第四步：配置環境變數

1. **進入 Web 服務** (不是資料庫服務)
2. **點擊 "Variables" 標籤**
3. **添加環境變數**:
   - 點擊 "+ New Variable"
   - 選擇 "Add Reference"
   - 選擇 PostgreSQL 資料庫服務
   - 選擇 `DATABASE_URL`

### 第五步：確認部署

1. **檢查 Deployments 標籤** - 確認部署成功
2. **點擊生成的 URL** - 訪問您的 API
3. **檢查日誌** - 確認看到 "✅ 資料庫遷移完成"

## 🔧 預期的成功日誌

```
🔍 環境變數檢查:
   DATABASE_URL 存在: True
   DATABASE_URL 長度: [數字]
   最終連接字串長度: [數字]
🗄️ 使用 PostgreSQL 資料庫儲存
🔄 轉換 Railway PostgreSQL 連接格式...
🔄 開始資料庫遷移...
✅ 資料庫遷移完成
```

## 🧪 測試 API

部署完成後，您的 API 將在以下地址可用：
- **主頁**: https://你的應用名稱.up.railway.app/
- **Swagger 文檔**: https://你的應用名稱.up.railway.app/swagger
- **健康檢查**: https://你的應用名稱.up.railway.app/health
- **診斷**: https://你的應用名稱.up.railway.app/debug/env

## 📊 功能測試

使用 `test-railway-postgresql.http` 檔案測試以下功能：

1. **材料管理**:
   - GET `/api/materials` - 獲取所有材料
   - POST `/api/materials` - 新增材料
   - PUT `/api/materials/{id}` - 更新材料
   - DELETE `/api/materials/{id}` - 刪除材料

2. **產品管理**:
   - GET `/api/products` - 獲取所有產品
   - POST `/api/products` - 新增產品

3. **配方管理**:
   - GET `/api/recipes` - 獲取所有配方
   - POST `/api/recipes` - 新增配方

## ⚠️ 常見問題排解

### 問題：看到 "使用記憶體儲存" 而不是 PostgreSQL
**解決方案**: 檢查環境變數設定
1. 確認在 Web 服務 (不是資料庫服務) 中設定了 `DATABASE_URL`
2. 使用 `/debug/env` 端點檢查環境變數狀態

### 問題：資料庫連接失敗
**解決方案**: 
1. 確認 PostgreSQL 服務正在運行
2. 檢查 Variables 中的 Reference 是否正確連結

### 問題：應用程式無法啟動
**解決方案**:
1. 檢查 Deployments 日誌中的錯誤訊息
2. 確認 railway.toml 配置正確

## 🎯 成功指標

- ✅ 應用程式成功啟動
- ✅ 看到 "✅ 資料庫遷移完成" 訊息
- ✅ `/health` 端點返回 200 狀態碼
- ✅ Swagger 文檔正常顯示
- ✅ 能夠新增/查詢材料、產品、配方資料
- ✅ 重啟後資料不會消失（資料持久化）

## 📚 相關文件

- [Railway 官方文檔](https://docs.railway.app/)
- [.NET on Railway](https://docs.railway.app/guides/dotnet)
- [PostgreSQL on Railway](https://docs.railway.app/databases/postgresql)
