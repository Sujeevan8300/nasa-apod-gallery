# 🚀 NASA APOD Gallery

An ASP.NET Core MVC web application that fetches data from NASA's **Astronomy Picture of the Day (APOD)** API, stores it in a SQL Server database using raw ADO.NET, and displays the images in a responsive gallery.

---

## 📋 Table of Contents

- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Project Structure](#-project-structure)
- [How It Works](#-how-it-works)
- [Prerequisites](#-prerequisites)
- [Database Setup](#-database-setup)
- [Configuration](#-configuration)
- [Running the Project](#-running-the-project)
- [Using the Application](#-using-the-application)

---

## ✨ Features

- Fetches APOD data for a **date range** (last 7 days) from the NASA Open API
- Stores fetched data in **SQL Server** using raw ADO.NET (`SqlConnection`, `SqlCommand`, `SqlParameter`)
- **Prevents duplicate entries** — the same picture date is never stored twice
- Displays all saved pictures in a clean, responsive **CSS Grid gallery**
- Handles both `image` and `video` media types gracefully
- Fast page loads — the Home Page reads only from the local database; NASA is only called when you click **"Sync"**

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| Language | C# |
| Database | SQL Server / SQL Express |
| Data Access | ADO.NET (`Microsoft.Data.SqlClient`) |
| External API | [NASA APOD API](https://api.nasa.gov/) |
| JSON Parsing | `System.Text.Json` |
| Front-End | Razor Views (`.cshtml`), Bootstrap 5, CSS Grid |
| Config | `appsettings.json` + `.env` via `DotNetEnv` |

---

## 📁 Project Structure

```
NasaApodGallery/
├── Controllers/
│   └── HomeController.cs        # Handles Index (gallery) and Sync (fetch from NASA) actions
├── DTOs/
│   └── ApodDto.cs               # Maps NASA API JSON response fields to C# properties
├── Models/
│   └── Apod.cs                  # Domain model representing a database record
├── Services/
│   ├── INasaApodService.cs      # Interface for the NASA API service
│   ├── NasaApodService.cs       # Calls NASA APOD API and deserializes JSON
│   ├── IApodRepository.cs       # Interface for database access
│   └── ApodRepository.cs        # Raw ADO.NET: INSERT and SELECT from SQL Server
├── Views/
│   ├── Home/
│   │   └── Index.cshtml         # Gallery page with Sync button
│   └── Shared/
│       └── _Layout.cshtml       # Shared HTML layout (navbar, scripts)
├── Database/
│   └── schema.sql               # SQL script to create the database and table
├── wwwroot/
│   └── css/site.css             # Custom gallery styles (CSS Grid, hover effects)
├── appsettings.json             # App configuration (connection string, API key placeholders)
├── .env                         # Local secret values (NOT committed to Git)
└── Program.cs                   # DI registration and middleware pipeline
```

---

## ⚙️ How It Works

### Visiting the Home Page (GET /Home/Index)

```
Browser  →  HomeController.Index()  →  ApodRepository.GetAllAsync()
                                             ↓
                                     SQL Server: SELECT * FROM Apod
                                             ↓
                                     List<Apod> passed to View
                                             ↓
                              Index.cshtml renders finished HTML
                                             ↓
                              Browser displays gallery (no API calls!)
```

### When "Sync" is clicked (POST /Home/Sync)

```
Browser clicks Sync button
        ↓
HomeController.Sync()
        ↓
NasaApodService.GetApodRangeAsync()  →  NASA APOD API (Internet)
        ↓
Deserializes JSON into List<ApodDto>
        ↓
ApodRepository.InsertIfNotExistsAsync() for each item
        ↓
SQL Server: IF NOT EXISTS INSERT INTO Apod ...
        ↓
RedirectToAction("Index")  →  Browser reloads gallery with new pictures
```

---

## 📦 Prerequisites

Make sure you have the following installed:

- [.NET 10 SDK](https://dotnet.microsoft.com/download) or later
- **SQL Server** — any one of the following:
  - SQL Server Express (Windows)
  - Docker with `mcr.microsoft.com/mssql/server` (Mac/Linux)
  - Azure SQL Edge (Mac Apple Silicon M1/M2/M3)

---

## 🗄️ Database Setup

1. Open **Azure Data Studio** or **SSMS** and connect to your SQL Server instance.
2. Open the file `Database/schema.sql` from this repository.
3. Run the script. It will create:
   - A database named `NasaApodDb`
   - A table named `Apod`

### Table Schema

| Column | Type | Description |
|---|---|---|
| `Id` | `INT IDENTITY` PK | Primary key, auto-incremented |
| `Date` | `DATE NOT NULL` | Date of the APOD entry (prevents duplicates) |
| `Title` | `NVARCHAR(255)` | Title of the picture |
| `Explanation` | `NVARCHAR(MAX)` | Long description from NASA |
| `Url` | `NVARCHAR(MAX)` | URL of the image or video |
| `MediaType` | `VARCHAR(50)` | Either `"image"` or `"video"` |
| `ServiceVersion` | `VARCHAR(50)` | NASA API version (e.g., `"v1"`) |
| `SavedAt` | `DATETIME NOT NULL` | Timestamp of when the record was saved |

---

## 🔑 Configuration

This project uses a `.env` file to keep secrets out of source code.

### Step 1 — Get a NASA API Key

1. Visit [https://api.nasa.gov/](https://api.nasa.gov/)
2. Fill in the form and click **"Signup"**
3. Your API key will be emailed to you instantly

> You can use `DEMO_KEY` for quick testing (limited to 30 requests/hour).


### Step 2 — Insert credential's into `appsettings.json` for Local Deployement


```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=NasaApodDb;User Id=${DB_USER};Password=${DB_PASS};TrustServerCertificate=True;Encrypt=False;"
  },
  "Nasa": {
    "ApiKey": "${NASA_API_KEY}"
  }
}
```

---

## ▶️ Running the Project

1. Complete the **Database Setup** steps above.
2. Create and fill in the **`.env` file** as described in Configuration.
3. Open a terminal in the project root and run:

```bash
dotnet run
```

4. Open your browser and navigate to:
   - `http://localhost:5224`

---

## 🖼️ Using the Application

| Action | What happens |
|---|---|
| **Visit the Home Page** | Displays all pictures already saved in your local database |
| **Click "Sync Latest from NASA"** | Fetches the last 7 days from NASA, saves new entries, reloads the gallery |
| **Gallery cards** | Show the image (or a video link for non-image entries), title, and date |
