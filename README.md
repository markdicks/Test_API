# 🧪 Test API (.NET 8 + EF Core)

This is a clean, modular .NET 8 API project using:

- Entity Framework Core 9
- Repository & Service Pattern
- SQL Server with customizable schema
- Layered architecture (Data, Repo, Service, Web)

---

## 🧱 Architecture

```
OA.Web         => API entry point (ASP.NET Core)
OA.Service     => Business logic / Service Layer
OA.Repo        => Data access / Repositories + DbContext
OA.Data        => Models / Entities / BaseEntity
```

- Uses `ApplicationContext` for centralized EF Core configuration
- Separation of concerns with interfaces and dependency injection

---

## 🧪 Getting Started

### 1. Clone the repo

```bash
git clone https://github.com/markdicks/Test_API.git
cd Test_API
```

### 2. Setup your local database connection

Copy `appsettings.json` to a local override version:

```bash
cp appsettings.json appsettings.Development.json
```

Then edit `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TestDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> 🔐 **Never commit `appsettings.Development.json`** – it contains sensitive info.

---

## 🛠️ EF Core Migrations

> Migrations are kept under version control for shared schema management.

To add a new migration (using Package Manager Console):

```bash
Add-Migration <name_of_migration>
```

To apply migrations:

```bash
Update-Database
```

---

## 🧪 Run the API

```bash
cd OA.Web
dotnet run
```

The API will start on:

```
https://localhost:5001
```

---

## 🧼 .gitignore Highlights

```gitignore
# Local secrets & sensitive configs
appsettings.Development.json

# Migrations (Comment out if you want to keep them)
OA.Repo/Migrations/*
```

---

## 🧠 Notes

- Follows SOLID principles and clean architecture practices
- Migrations use schema prefixing (`Test_`) for namespace isolation
- DI setup via `builder.Services.AddScoped` and `AddTransient` based on service lifetimes
- OA stands for "Onion Architecture"

---

## 📦 Packages

- `Microsoft.EntityFrameworkCore` (9.0.4)
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`

---

## 📌 License

MIT © [Mark Dicks](https://github.com/markdicks)

## Status
![Alt](https://repobeats.axiom.co/api/embed/ce19d8d6e335d8b0074dfc2db32053fa1b245a34.svg "Repobeats analytics image")
