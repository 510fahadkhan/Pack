# 🥫 Pantry Expiry Tracker — Web Edition

A browser-based pantry expiry tracker with a C# ASP.NET Core backend and a clean HTML/JS frontend.

---

## 📁 Project Structure

```
PantryWeb/
└── PantryApi/
    ├── Controllers/
    │   └── PantryController.cs   ← REST API endpoints
    ├── Models/
    │   └── PantryItem.cs         ← Data model
    ├── Services/
    │   └── FileHandler.cs        ← JSON file persistence
    ├── wwwroot/
    │   └── index.html            ← Browser UI (served by ASP.NET)
    ├── Program.cs                ← App entry point
    └── PantryApi.csproj          ← .NET 8 project file
```

---

## 🚀 How to Run

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Steps

```bash
# 1. Navigate to the API project
cd PantryWeb/PantryApi

# 2. Run the server
dotnet run

# 3. Open your browser and go to:
http://localhost:5000
```

The UI will load automatically in your browser. The backend API is available at:
- `GET    /api/pantry`             → All items
- `POST   /api/pantry`             → Add item
- `DELETE /api/pantry/{name}`      → Remove item
- `GET    /api/pantry/expired`     → Expired items
- `GET    /api/pantry/expiring-soon` → Items expiring within 3 days

---

## 💾 Data Storage

Items are saved in `pantry.json` in the project directory (auto-created on first use).

---

## ✨ Features

- ✅ Add pantry items with name, quantity, and expiry date
- 📋 View all items in a sortable table
- 🔴 Highlights expired items
- ⚠️ Warns about items expiring within 3 days
- 🗑️ Remove items with one click
- 📊 Summary counts (Fresh / Expiring Soon / Expired)
- 💾 Data persists between sessions via JSON file
