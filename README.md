# Recipe Management API

一個使用 .NET 8.0 開發的產品配方管理系統 API，支援材料管理、產品管理、配方設計與成本計算。

## 🚀 功能特色

- **材料管理**: 完整的材料資訊管理，包含庫存、成本、供應商資訊
- **產品管理**: 產品資訊與分類管理
- **配方管理**: 靈活的配方設計與版本控制
- **成本計算**: 自動計算配方成本與分析
- **RESTful API**: 完整的 CRUD 操作端點
- **Swagger 文檔**: 自動生成的 API 文檔
- **資料驗證**: 完整的輸入驗證與錯誤處理

## 🛠️ 技術架構

- **.NET 8.0**: 最新的 .NET 框架
- **ASP.NET Core Web API**: RESTful API 開發
- **Entity Framework Core**: ORM 資料庫操作
- **InMemory Database**: 開發環境資料庫
- **Swagger/OpenAPI**: API 文檔與測試
- **CsvHelper**: CSV 檔案處理支援

## 📁 專案結構

```
RecipeManagementApi/
├── Controllers/              # API 控制器
│   ├── MaterialsController.cs
│   ├── ProductsController.cs
│   ├── RecipesController.cs
│   └── RecipeItemsController.cs
├── Models/                   # 資料模型
│   ├── Material.cs
│   ├── Product.cs
│   ├── Recipe.cs
│   ├── RecipeItem.cs
│   └── DTOs.cs
├── Data/                     # 資料存取層
│   └── RecipeDbContext.cs
├── Services/                 # 業務邏輯層
│   └── RecipeService.cs
├── Properties/
│   └── launchSettings.json
├── Program.cs               # 應用程式啟動
├── RecipeManagementApi.csproj
└── RecipeManagementApi.http  # API 測試檔案
```

## 🔧 安裝與執行

### 前置需求

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 或 Visual Studio Code

### 執行步驟

1. **複製專案**
   ```bash
   git clone https://github.com/your-username/RecipeManagementApi.git
   cd RecipeManagementApi
   ```

2. **還原套件**
   ```bash
   dotnet restore
   ```

3. **建置專案**
   ```bash
   dotnet build
   ```

4. **執行專案**
   ```bash
   dotnet run
   ```

5. **開啟 Swagger UI**
   - 瀏覽器訪問: `https://localhost:7120` 或 `http://localhost:5120`

## 📖 API 文檔

啟動專案後，可以透過以下方式查看 API 文檔：

- **Swagger UI**: `https://localhost:7120`
- **OpenAPI JSON**: `https://localhost:7120/swagger/v1/swagger.json`

## 🧪 API 測試

專案包含完整的 API 測試檔案 `RecipeManagementApi.http`，可以使用 Visual Studio Code 的 REST Client 擴充功能或其他 HTTP 客戶端進行測試。

### 主要 API 端點

#### 材料管理
- `GET /api/materials` - 取得材料清單
- `GET /api/materials/{id}` - 取得特定材料
- `POST /api/materials` - 建立新材料
- `PUT /api/materials/{id}` - 更新材料
- `DELETE /api/materials/{id}` - 刪除材料

#### 產品管理
- `GET /api/products` - 取得產品清單
- `GET /api/products/{id}` - 取得特定產品
- `GET /api/products/{id}/recipes` - 取得產品的配方
- `POST /api/products` - 建立新產品
- `PUT /api/products/{id}` - 更新產品
- `DELETE /api/products/{id}` - 刪除產品

#### 配方管理
- `GET /api/recipes` - 取得配方清單
- `GET /api/recipes/{id}` - 取得特定配方
- `GET /api/recipes/{id}/items` - 取得配方項目
- `GET /api/recipes/{id}/cost` - 計算配方成本
- `POST /api/recipes` - 建立新配方
- `POST /api/recipes/{id}/items` - 新增配方項目
- `POST /api/recipes/{id}/set-primary` - 設為主要配方
- `PUT /api/recipes/{id}` - 更新配方
- `DELETE /api/recipes/{id}` - 刪除配方

#### 配方項目管理
- `GET /api/recipe-items/{id}` - 取得配方項目詳情
- `PUT /api/recipe-items/{id}` - 更新配方項目
- `DELETE /api/recipe-items/{id}` - 刪除配方項目

## 📊 資料模型

### 材料 (Material)
- 基本資訊: 名稱、描述、分類
- 成本資訊: 單位成本、計量單位
- 庫存資訊: 庫存量、最低庫存警戒
- 供應商資訊

### 產品 (Product)
- 基本資訊: 名稱、編號、描述、分類
- 生產資訊: 標準產量、製作時間
- 價格資訊: 標準售價

### 配方 (Recipe)
- 基本資訊: 名稱、版本、描述
- 生產資訊: 產量、製作步驟
- 狀態管理: 草稿、測試、啟用、停用
- 審核資訊: 創建者、審核者

### 配方項目 (RecipeItem)
- 材料關聯: 材料 ID、數量、單位
- 類型分類: 主要材料、調味料、裝飾等
- 計算資訊: 單位換算、成本計算

## 🔮 功能擴展

未來可以考慮的功能擴展：

- [ ] 使用者認證與授權
- [ ] 實體資料庫支援 (SQL Server, PostgreSQL)
- [ ] 批量匯入/匯出功能
- [ ] 庫存預警通知
- [ ] 成本分析報表
- [ ] 配方營養成分計算
- [ ] 多語言支援
- [ ] 單元測試與整合測試
- [ ] Docker 容器化
- [ ] CI/CD 管道

## 🤝 參與貢獻

歡迎提交 Issue 和 Pull Request！

1. Fork 專案
2. 建立功能分支 (`git checkout -b feature/amazing-feature`)
3. 提交變更 (`git commit -m 'Add some amazing feature'`)
4. 推送到分支 (`git push origin feature/amazing-feature`)
5. 開啟 Pull Request

## 📄 授權條款

本專案採用 MIT 授權條款 - 詳見 [LICENSE](LICENSE) 檔案

## 📞 聯絡資訊

如有任何問題或建議，歡迎透過以下方式聯繫：

- 建立 [Issue](https://github.com/your-username/RecipeManagementApi/issues)
- 發送 Email: your-email@example.com

---

⭐ 如果這個專案對您有幫助，請給個 Star 支持一下！
