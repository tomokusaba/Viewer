# デジタルサイネージアプリ (Digital Signage Application)

.NET Aspire を使用したデジタルサイネージアプリケーションです。

## 技術スタック

- **Frontend**: Blazor WebAssembly
- **Backend**: ASP.NET Core Web API
- **Database**: SQL Server
- **Framework**: .NET 10
- **Orchestration**: .NET Aspire

## プロジェクト構成

```
src/
├── DigitalSignage.AppHost/          # Aspire オーケストレーター
├── DigitalSignage.ServiceDefaults/  # 共通サービス設定
├── DigitalSignage.Server/           # Web API サーバー
├── DigitalSignage.Client/           # Blazor WASM クライアント
└── DigitalSignage.Shared/           # 共有モデル・DTO
```

## 機能

- ✅ データベース上のコンテンツを表示
- ✅ 管理コンソール
- ✅ コンテンツ登録機能（画像・動画・マークダウン）
- ✅ テンプレートによる装飾（選択可能）
- ✅ コンテンツのタグ付け機能
- 🔲 カメラ連携（AI タグ分類）
- ✅ タグに基づく優先コンテンツ表示

## 必要条件

- .NET 10 SDK
- Docker (SQL Server コンテナ用)

## 起動方法

```bash
# Aspire AppHost から起動
cd src/DigitalSignage.AppHost
dotnet run
```

Aspire Dashboard が起動し、以下のサービスが自動的に構成されます：
- SQL Server コンテナ
- Web API サーバー（Blazor WASM クライアント含む）

## 開発

### ビルド

```bash
dotnet build
```

### テスト

```bash
dotnet test
```

## API エンドポイント

### コンテンツ管理
- `GET /api/contents` - 全コンテンツ取得
- `GET /api/contents/active` - アクティブなコンテンツ取得
- `GET /api/contents/by-tags?tags=1,2,3` - タグでフィルタリング
- `POST /api/contents` - コンテンツ作成
- `PUT /api/contents/{id}` - コンテンツ更新
- `DELETE /api/contents/{id}` - コンテンツ削除

### タグ管理
- `GET /api/tags` - 全タグ取得
- `POST /api/tags` - タグ作成
- `PUT /api/tags/{id}` - タグ更新
- `DELETE /api/tags/{id}` - タグ削除

### テンプレート管理
- `GET /api/templates` - 全テンプレート取得
- `GET /api/templates/active` - アクティブなテンプレート取得
- `POST /api/templates` - テンプレート作成
- `PUT /api/templates/{id}` - テンプレート更新
- `DELETE /api/templates/{id}` - テンプレート削除

## ライセンス

MIT