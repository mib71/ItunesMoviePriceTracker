# 🎬 ItunesMoviePriceTracker

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![Blazor](https://img.shields.io/badge/Blazor-Server-5C2D91?logo=blazor)
![MSSQL](https://img.shields.io/badge/MSSQL-Database-CC2927?logo=microsoftsqlserver)
![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20IIS-0078D4?logo=windows)
![License](https://img.shields.io/badge/License-MIT-green)

A Blazor Server application for managing your iTunes movie wishlist with price monitoring. Track HD and 4K price history, set watch prices and receive email notifications when prices drop, all from a local IIS-hosted web app.

---

## Screenshots

| Movie List | Add Movie | Movie Detail |
|---|---|---|
| ![Movie List](docs/screenshots/movie-list.png) | ![Add Movie](docs/screenshots/add-movie.png) | ![Movie Detail](docs/screenshots/movie-detail.png) |

---

## Features

- 📋 Browse all tracked iTunes movies in a sortable, filterable data grid
- 📈 Price history per movie with trend indicators (up / down / all-time low)
- 🎯 Set a personal watch price per movie
- 🔄 Scheduled price checks via Windows Task Scheduler
- 🌍 Choose your iTunes store on first start — 18 countries supported
- 🖱️ Manual price update trigger from the UI
- 🌙 Dark / light mode with system theme detection and localStorage persistence
- 🔍 Filter by title and director

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (local or remote)
- [IIS](https://learn.microsoft.com/en-us/iis/get-started/introduction-to-iis/iis-web-server-overview) with ASP.NET Core Hosting Bundle installed
- [ASP.NET Core Hosting Bundle for .NET 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/your-username/ItunesMoviePriceTracker.git
cd ItunesMoviePriceTracker
```

### 2. Configure the database

Update the connection string in both `appsettings.json` files (see [Configuration](#configuration) below).

### 3. Run the Web app locally

```bash
dotnet run --project src/ItunesMoviePriceTracker.Web
```

Or open `ItunesMoviePriceTracker.sln` in Visual Studio and press F5.

The database and all migrations are applied automatically on startup. If the database does not exist it will be created.

On first start you are taken to a setup page where you choose which iTunes store to track prices in. The choice is stored in the database and cannot be changed later. Existing databases from earlier versions are automatically set to the Swedish store and skip this step.

### 4. Set up the Update Service (optional)

To run price checks on a schedule, register the UpdateService with Windows Task Scheduler:

```bash
dotnet publish src/ItunesMoviePriceTracker.UpdateService -c Release -o publish/UpdateService
```

Then create a new task in Windows Task Scheduler pointing to:
```
publish/UpdateService/ItunesMoviePriceTracker.UpdateService.exe
```

Configure the trigger to run at your preferred times. Price checks can also be triggered manually from the Web UI via the Refresh button.

Until a store has been selected in the web app, the UpdateService logs a warning and exits with code `1`, which shows as `0x1` in the task's Last Run Result.

---

## Configuration

### `appsettings.json` (Web & UpdateService)

Both `ItunesMoviePriceTracker.Web` and `ItunesMoviePriceTracker.UpdateService` require a local `appsettings.json` that is **not committed to the repository**. Use `appsettings.example.json` as a template — copy it and rename it to `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ItunesMovies;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "PriceCheck": {
    "ThrottleHours": 24
  },
  "UiPolling": {
    "IntervalMinutes": 30
  },
  "DataProtection": {
    "KeyPath": "YOUR_KEY_PATH"
  },
  "Notifications": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUser": "YOUR_EMAIL",
    "SmtpPassword": "YOUR_APP_PASSWORD",
    "ToEmail": "YOUR_EMAIL"
  },
  "AllowedHosts": "*"
}
```

Replace `YOUR_SERVER` with your SQL Server instance name, e.g. `localhost` or `.\SQLEXPRESS`.

`ThrottleHours` controls how many hours must pass before a movie is eligible for a new price check. Defaults to `24` if not set.

`UiPolling:IntervalMinutes` sets how often the UI polls the database for updates in the background. Defaults to `30` minutes if not set.

`DataProtection:KeyPath` sets the path where ASP.NET Core Data Protection keys are persisted. Example: `C:\\inetpub\\ItunesMoviePriceTracker\\keys`. The IIS app pool identity needs write access to this folder.

`Notifications` — SMTP settings for email notifications when a price drops below `WatchPrice`.


---

## IIS Setup

### 1. Install ASP.NET Core Hosting Bundle for .NET 10

Download and install from [dot.net/download](https://dotnet.microsoft.com/download/dotnet/10.0) then run:

```bash
iisreset
```

### 2. Create site folder and required subfolders

```bash
mkdir C:\inetpub\ItunesMoviePriceTracker
mkdir C:\inetpub\ItunesMoviePriceTracker\logs
mkdir C:\inetpub\ItunesMoviePriceTracker\keys
```

### 3. Grant IIS permissions

```bash
icacls "C:\inetpub\ItunesMoviePriceTracker" /grant "IIS_IUSRS:(OI)(CI)RX"
icacls "C:\inetpub\ItunesMoviePriceTracker\logs" /grant "IIS_IUSRS:(OI)(CI)F"
icacls "C:\inetpub\ItunesMoviePriceTracker\keys" /grant "IIS_IUSRS:(OI)(CI)F"
icacls "C:\inetpub\ItunesMoviePriceTracker\keys" /grant "IIS APPPOOL\ItunesMoviePriceTracker:(OI)(CI)F"
```

> Note: The last command uses the exact application pool identity. Replace `ItunesMoviePriceTracker` with your site name if different.

### 4. Grant SQL Server permissions

Run in SSMS:

```sql
USE master;
CREATE LOGIN [IIS APPPOOL\ItunesMoviePriceTracker] FROM WINDOWS;

USE ItunesMovies;
CREATE USER [IIS APPPOOL\ItunesMoviePriceTracker] FOR LOGIN [IIS APPPOOL\ItunesMoviePriceTracker];
ALTER ROLE db_datareader ADD MEMBER [IIS APPPOOL\ItunesMoviePriceTracker];
ALTER ROLE db_datawriter ADD MEMBER [IIS APPPOOL\ItunesMoviePriceTracker];
ALTER ROLE db_ddladmin ADD MEMBER [IIS APPPOOL\ItunesMoviePriceTracker];
```

### 5. Create IIS site

In IIS Manager → **Sites** → **Add Website**:
- **Site name:** `ItunesMoviePriceTracker`
- **Physical path:** `C:\inetpub\ItunesMoviePriceTracker`
- **Port:** `8080`

### 6. Configure Application Pool

In IIS Manager → **Application Pools** → `ItunesMoviePriceTracker` → **Advanced Settings**:
- **.NET CLR Version:** `No Managed Code`
- **Enable 32-Bit Applications:** `False`

### 7. Publish and deploy

Run as administrator:

```bash
iisreset /stop
dotnet publish src/ItunesMoviePriceTracker.Web -c Release -o C:\inetpub\ItunesMoviePriceTracker
iisreset /start
```

---

## Architecture

### Solution Structure

```
ItunesMoviePriceTracker/
├── ItunesMoviePriceTracker.sln
└── src/
    ├── ItunesMoviePriceTracker.Shared           # DTOs only
    ├── ItunesMoviePriceTracker.Repository       # EF Core, MSSQL, internal entities
    ├── ItunesMoviePriceTracker.Services         # Business logic, iTunes API, DI registration
    ├── ItunesMoviePriceTracker.Web              # Blazor Server (.NET 10), IIS hosted
    └── ItunesMoviePriceTracker.UpdateService    # Console App, Windows Task Scheduler
```

### Dependency Graph

```
Shared      ←  Services  ←  Web
                          ←  UpdateService
Repository  ←  Services
```

- `Shared` has zero dependencies — DTOs only
- `Repository` has zero dependencies on other projects — only EF Core NuGet packages
- `Services` references `Shared` and `Repository`, contains service interfaces, implementations and DI registration via extension methods
- `Web` and `UpdateService` reference `Shared` and `Services` only — `Repository` is wired up via DI in `Program.cs`

### Data Flow

```
Web → (DTOs in Shared) → Service → Repository → MSSQL
                              ↑
                 EF entity mapped to DTO in Service layer
```

---

## Coding Conventions

| Convention | Choice |
|---|---|
| Constructors | Primary constructors (C# 12) |
| Async | Async/await throughout |
| Nullable | Nullable reference types enabled |
| Naming | PascalCase for classes/methods, camelCase for locals |

---

## Architecture & Key Decisions

| Area | Choice | Reason |
|---|---|---|
| Framework | .NET 10 / Blazor Server | Modern, interactive server rendering |
| Database | MSSQL (existing) | Preserves years of accumulated price data |
| ORM | EF Core Code First | Migrations handle schema changes cleanly |
| Hosting | IIS (local) | Consistent with existing setup |
| Price updates | Windows Task Scheduler + manual UI trigger | Simple, reliable, no external dependencies |
| iTunes data | iTunes Search API (Apple) | Proven, no scraping needed |
| Store country | Stored in database, not configuration | Web and UpdateService cannot drift apart; price history is tagged per store |
| HTTP | IHttpClientFactory | Best practice for HttpClient lifetime management |
| Theme | System detection + localStorage | Respects user preference, persists across navigation |
| Logging | Serilog with file sink | Daily rolling log files, EF Core noise filtered out |

### Layered Architecture

- `Shared` exposes only DTOs — no interfaces, no EF entities leak into Web
- `Repository` is fully internal — EF entities never leave the Repository layer
- `Services` owns all mapping between EF entities and DTOs
- `Services` owns DI registration via extension methods — `Web` and `UpdateService` call `builder.Services.AddMovieServices()` in `Program.cs` without knowing anything about `Repository`

### Price Trend

Trend is intentionally calculated in the UI layer (Blazor component), not in services. It is a display concern — no enum or helper needed. The `Trend` property is set on the DTO after data is loaded.

### iTunes API Behavior

- Store country is read from `dbo.StoreSettings` via `IStoreSettingsProvider` and cached for the lifetime of the process
- Supported stores are defined in `StorefrontCatalog` in the Services project
- Every price record is tagged with the country code of the store it was fetched from
- If no store has been selected, all pages redirect to `/setup` and price checks are skipped with a warning
- 3-second delay between API calls — respects Apple rate limits
- Movies only checked if `LastChecked` is older than `ThrottleHours` (configurable, default 24h)

### Logging

Serilog is used for structured logging with a daily rolling file sink. EF Core database command logging is suppressed at `Warning` level to avoid noise. Log files are stored in the `logs/` folder of each deployed application and rotated daily with a 30-day retention policy.

On startup, `ApplyMigrationsAsync()` is called via `Program.cs`. This creates the database if it does not exist and applies any pending migrations automatically. No manual `dotnet ef database update` needed.

### Theme

Theme is managed entirely in JavaScript — Blazor never touches it. On page load, the saved `localStorage` value is applied. If no preference is saved, the system theme is used. The toggle button in the header persists the choice to `localStorage`.

---

## Database Schema

**dbo.Movies**

| Column | Type | Notes |
|---|---|---|
| TrackId | bigint PK | iTunes Track ID |
| TrackName | nvarchar(500) | Movie title |
| ReleaseDate | datetime2(7) | |
| ArtistName | nvarchar(255) | Director |
| LongDescription | nvarchar(2000) | |
| ArtworkUrl60 | nvarchar(500) | Thumbnail |
| ArtworkUrl400 | nvarchar(500) | Full artwork |
| TrackHdPrice | decimal(18,2) | Latest known price |
| LastChecked | datetime2(7) | Throttle control |
| WatchPrice | decimal(18,2) | Target watch price (nullable) |

**dbo.Prices**

| Column | Type | Notes |
|---|---|---|
| Id | int PK | |
| Date | datetime2(7) | UTC |
| Price | decimal(18,2) | |
| CountryCode | nvarchar(2) | iTunes store the price was fetched from, e.g. `se` |
| MovieTrackId | bigint FK | → dbo.Movies.TrackId |

**dbo.StoreSettings**

Holds the iTunes store selected for this installation. At most one row, enforced by the check constraint `CK_StoreSettings_SingleRow`.

| Column | Type | Notes |
|---|---|---|
| Id | int PK | Always `1` |
| CountryCode | nvarchar(2) | iTunes store country, e.g. `se` |
| CurrencyCode | nvarchar(3) | ISO 4217, e.g. `SEK` |
| Culture | nvarchar(10) | Culture name for the store, e.g. `sv-SE` |
| SelectedAt | datetime2(7) | UTC |

---

## Future Considerations

- Multi-language support (UI layer, resource strings)
- Price formatting per store currency (prices are currently shown without a currency symbol)
- Changing the iTunes store after initial setup (price history is already tagged per store)
- Desktop app version using Blazor + WebView2 — wraps the existing app in a Windows `.exe` installer, eliminating the need for IIS and SQL Server setup for end users

---

## License

This project is licensed under the [MIT License](LICENSE).