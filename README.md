Here is a clean, professional, and GitHub-ready `README.md` formatted based on your specifications. I have added syntax highlighting, code blocks, and proper structural hierarchy.

***

# 🖥️ PCBuddy Backend

A robust **Hybrid ASP.NET Core 8 Application** that serves as the backbone for the PCBuddy ecosystem. This project provides a RESTful API for the mobile Flutter frontend and a secure MVC Admin Panel for content management.

## 🚀 Features

### 📱 API (Mobile Backend)
*   **Secure Authentication:** Utilizes **JWT (JSON Web Token)** for stateless user authentication.
*   **Delta Sync:** An optimized synchronization system that sends only modified or new data to the frontend, supporting an **Offline-first approach**.
*   **AI Integration:** Powered by **Google Gemini 1.5 Flash** for:
    *   Intelligent PC Part recommendations.
    *   Laptop assessments and performance estimates.
    *   Compatibility checking and FPS predictions.
*   **PC Builder Logic:** Handles complex compatibility checks and price calculations for custom user builds.
*   **Profile Management:** Supports user profile updates and local image hosting.

### 🛠️ Admin Panel (MVC)
*   **Secure Dashboard:** Protected by Cookie-based authentication and Role-based Authorization (Admin access only).
*   **Hardware Management:** Full **CRUD** operations for CPUs, GPUs, Memory, Storage, Motherboards, PSUs, and Cases.
*   **Smart Parsing:** Automatically handles complex component naming conventions (e.g., combining GPU Chipsets with Model names).
*   **Game Management:** Manages the database of games and system requirements used by the AI/Sync service.
*   **User Management:** View user statistics, ban/unban users, and manage administrative permissions.
*   **Pagination:** efficiently handles large datasets (e.g., 80,000+ games).
*   **Soft Deletes:** Data is marked as deleted (`IsDeleted`) rather than removed, preserving data integrity for the Delta Sync system.

---

## 🛠️ Tech Stack

| Category | Technology |
| :--- | :--- |
| **Framework** | .NET 8 (ASP.NET Core Web API + MVC) |
| **Language** | C# |
| **Database** | SQL Server (via Entity Framework Core) |
| **AI Model** | Google Gemini Flash |
| **Auth** | Dual-Scheme (JWT for API, Cookies for MVC) |
| **Utilities** | FuzzySharp (Matching), DotNetEnv (Env Variables) |

---

## ⚙️ Setup & Configuration

### 1. Prerequisites
*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   SQL Server (LocalDB or Full Instance)

### 2. Environment Variables
Create a `.env` file in the root directory of the project. **This is required.**

```properties
# Database Connection (Optional if set in appsettings.json)
DefaultConnection="Server=(localdb)\\mssqllocaldb;Database=PCBuddyDB;Trusted_Connection=True;MultipleActiveResultSets=true"

# JWT Settings (Required for API Auth)
JWT_KEY=YourSuperSecretKeyMustBeAtLeast32CharsLong!
JWT_ISSUER=PCBuddyBackend
JWT_AUDIENCE=PCBuddyUsers

# Google Gemini AI (Required for AI Features)
GEMINI_API_KEY=your_google_gemini_api_key_here
```

### 3. Database Setup
Run the Entity Framework migrations to create the database schema.

```bash
dotnet ef database update
```

### 4. Seeding Data
The project includes a seeder to populate hardware and game data. Run this command once:

```bash
dotnet run seed
```

### 5. Running the Application
Start the server:

```bash
dotnet run
```

The application typically starts on:
*   **HTTPS:** `https://localhost:7001`
*   **HTTP:** `http://localhost:5000`

---

## 📂 Project Structure

```text
├── Controllers/       # API Controllers (/api/...) and Admin MVC Controllers (/Admin/...)
├── Services/          # Business logic (ComputerService, AIService, UserService, SyncService)
├── Models/            # Database entities (EF Core)
├── DTOs/              # Data Transfer Objects for API requests/responses
├── Views/             # Razor views for the Admin Panel
└── wwwroot/           # Static files (e.g., Profile pictures)
```

---

## 🧪 Testing

### API Testing (Postman)
A script/collection is included in the repository to test API endpoints.

1.  **Auth:** Login via `/api/auth/login` to obtain a **Bearer Token**.
2.  **Sync:** Test `/api/sync/reference-data` to verify the Delta Sync logic.

### Admin Panel
1.  Navigate to `https://localhost:7001/` (or your configured port).
2.  Login with an Admin account.
    *   *Note:* If you seeded users, check the database for admin credentials or manually set a user's `Role` to `1` (Admin) in the `Users` table.

---

## 📸 Image Uploads

Profile pictures are stored locally in the `wwwroot/uploads/profiles` directory.

> **Note:** If you encounter errors uploading images, ensure the `wwwroot` folder exists in the root directory. The application handles folder creation, but file system permissions must be valid.
