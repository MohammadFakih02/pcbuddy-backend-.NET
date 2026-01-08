🖥️ PCBuddy Backend

A robust Hybrid ASP.NET Core 8 Application that serves as the backbone for the PCBuddy ecosystem. It provides a RESTful API for the mobile Flutter frontend and a secure MVC Admin Panel for content management.
🚀 Features
📱 API (Mobile Backend)

    Secure Authentication: JWT (JSON Web Token) authentication for users.

    Delta Sync: Optimized synchronization system sending only modified/new data to the frontend (Offline-first approach).

    AI Integration: Powered by Google Gemini 1.5 Flash for:

        Intelligent PC Part recommendations.

        Laptop assessments and performance estimates.

        Compatibility checking and FPS predictions.

    PC Builder Logic: Complex compatibility checks and price calculation for custom builds.

    Profile Management: User profile updates and image uploads.

🛠️ Admin Panel (MVC)

    Secure Dashboard: Cookie-based authentication protected by Role-based Authorization (Admin only).

    Hardware Management: Full CRUD operations for CPUs, GPUs, Memory, Storage, Motherboards, PSUs, and Cases.

    Smart Parsing: Automatically handles complex component names (e.g., combining GPU Chipsets with Model names).

    Game Management: Manage the database of games and system requirements used by the AI/Sync service.

    User Management: View statistics, ban/unban users, and manage permissions.

    Pagination: Efficient handling of large datasets (e.g., 80,000+ games).

    Soft Deletes: Data is marked as deleted (IsDeleted) to preserve integrity for the Delta Sync system.

🛠️ Tech Stack

    Framework: .NET 8 (ASP.NET Core Web API + MVC)

    Language: C#

    Database: SQL Server (via Entity Framework Core)

    AI Model: Google gemini 2.5 flash lite

    Authentication: Dual-Scheme (JWT for API, Cookies for MVC)

    Utilities: FuzzySharp (Fuzzy Matching), DotNetEnv (Environment Variables)

⚙️ Setup & Configuration
1. Prerequisites

    .NET 8 SDK

    SQL Server (LocalDB or Full Instance)

2. Environment Variables

Create a .env file in the root directory of the project. This is required.
code Properties

    
# Database Connection (Optional if set in appsettings.json)
DefaultConnection="Server=(localdb)\\mssqllocaldb;Database=PCBuddyDB;Trusted_Connection=True;MultipleActiveResultSets=true"

# JWT Settings (Required for API Auth)
JWT_KEY=YourSuperSecretKeyMustBeAtLeast32CharsLong!
JWT_ISSUER=PCBuddyBackend
JWT_AUDIENCE=PCBuddyUsers

# Google Gemini AI (Required for AI Features)
GEMINI_API_KEY=your_google_gemini_api_key_here

  

3. Database Setup

Run the migrations to create the database schema.
code Bash

    
dotnet ef database update

  

4. Seeding Data

The project includes a seeder to populate hardware and game data. Run this once:
code Bash

    
dotnet run seed

  

5. Running the Application

Start the server:
code Bash

    
dotnet run

  

The application typically starts on https://localhost:7001 or http://localhost:5000.
📂 Project Structure

    Controllers/: Contains both API Controllers (/api/...) and Admin MVC Controllers (/Admin/...).

    Services/: Business logic layer (ComputerService, AIService, UserService, SyncService).

    Models/: Database entities (EF Core).

    DTOs/: Data Transfer Objects for API requests/responses.

    Views/: Razor views for the Admin Panel.

    wwwroot/: Stores uploaded static files (Profile pictures).

🧪 Testing
API Testing (Postman)

A script is included in the repository to generate a Postman collection for all API endpoints.

    Run the script (if applicable) or import the collection.

    Auth: Login via /api/auth/login to get a Bearer Token.

    Sync: Test /api/sync/reference-data to see the Delta Sync in action.

Admin Panel

    Navigate to https://localhost:7001/ (or your port).

    Login with an Admin account.

        Note: If you seeded users, check the database for the admin credentials or manually set a user's role to 1 (Admin) in the Users table.

📸 Image Uploads

Profile pictures are stored locally in the wwwroot/uploads/profiles directory.

    Note: If you encounter errors uploading images, ensure the wwwroot folder exists in the root directory. The application handles folder creation, but permissions must be valid.
