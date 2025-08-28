# CheckToAim

ASP.NET MVC 5 (on .NET Framework 4.7.2) web app for creating and sharing daily "aim" checklists. It demonstrates EF6 code‑first, forms authentication, role‑based authorization, search/filtering/sorting, pagination, and Razor views.

## Features
- EF6 Code First with seed data (themes, roles, sample users, sample lists)
- User registration and login (Forms Authentication)
- Roles: Admin/User with separate areas/actions
- Create, edit, delete checklists; search, filter by theme, sort, paginate
- Bootstrap/jQuery UI styling and validation

## Tech Stack
- ASP.NET MVC 5, Razor views
- .NET Framework 4.7.2
- Entity Framework 6.x
- SQL Server (LocalDB by default)
- Bootstrap, jQuery, jQuery Validation

## Prerequisites (Windows)
- .NET Framework 4.7.2 Developer Pack (Targeting Pack) or Visual Studio Build Tools 2019/2022 with ".NET Framework 4.x development tools"
- IIS Express
- SQL Server Express LocalDB (MSSQLLocalDB) or a SQL Server instance
- NuGet CLI (`nuget.exe`) on PATH (or use Visual Studio to restore packages)

## Quick Start (VS Code or Terminal)
1) Clone the repo and open the folder in VS Code.

2) Connection string (default works with LocalDB):
   - `CheckToAim/Web.config` contains:
     ```xml
     <connectionStrings>
       <add name="CheckToAimDb"
            connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CheckToAimDb;Integrated Security=True;MultipleActiveResultSets=True"
            providerName="System.Data.SqlClient" />
     </connectionStrings>
     ```
   - `Models/CheckListDBContext.cs` uses `base("name=CheckToAimDb")`.
   - To use a full SQL Server, change the connection string accordingly.

3) Restore NuGet packages:
   ```powershell
   nuget restore .\CheckToAim.sln
   ```

4) Build the project (PowerShell):
   ```powershell
   & "C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe" .\CheckToAim\CheckToAim.csproj /p:Configuration=Debug
   ```
   If `msbuild` is already on PATH, you can simply run:
   ```powershell
   msbuild .\CheckToAim\CheckToAim.csproj /p:Configuration=Debug
   ```

5) Run with IIS Express (PowerShell):
   ```powershell
   & "$env:ProgramFiles\IIS Express\iisexpress.exe" /path:"$PWD\CheckToAim" /port:5000
   # or if installed under Program Files (x86):
   & "$env:ProgramFiles(x86)\IIS Express\iisexpress.exe" /path:"$PWD\CheckToAim" /port:5000
   ```
   Then open http://localhost:5000 in your browser. The default route points to `Home/Home`.

## Running in Visual Studio
1) Open `CheckToAim.sln` in Visual Studio.
2) Ensure the connection string in `Web.config` is correct for your environment.
3) Press F5 to build and run (IIS Express).

## Running in VS Code
- Install the "C#" extension by Microsoft.
- Use the Quick Start steps above to restore, build, and start IIS Express from the integrated terminal.
- Optionally add VS Code tasks to automate restore/build/run.

## Database and Seed Data
- The EF initializer (`DropCreateDatabaseIfModelChanges`) seeds:
  - Themes: Art, Adventure, Travel, Reading, Sport, Hobby, Other
  - Sort options
  - Roles: Admin, User
  - Users (login with email + password):
    - Admin: Email `test@test.com`, Password `12345678`
    - User:  Email `ed7777ed77@gmail.com`, Password `12345678`
- On first run, the database `CheckToAimDb` is created automatically if it does not exist.

## Troubleshooting
- MSB3644 (.NETFramework v4.7.2 not found): Install the 4.7.2 Developer Pack (Targeting Pack).
- Missing `Microsoft.WebApplication.targets`: Install Visual Studio Build Tools with the Web workload, or add NuGet `MSBuild.Microsoft.VisualStudio.Web.targets`.
- LocalDB not found: Install SQL Server Express LocalDB (`sqllocaldb info` should list `MSSQLLocalDB`). Update the connection string if using another SQL instance.
- Port already in use: change `/port:5000` to another free port.

## Project Structure (high‑level)
- `Controllers/` — MVC controllers (e.g., `HomeController`, `LoginController`)
- `Models/` — EF models, `CheckListDBContext`
- `Views/` — Razor views and partials
- `App_Start/` — Route/Bundle/Filter config
- `Global.asax` — application startup
- `Web.config` — app and EF configuration

---
This is a learning project; contributions and improvements are welcome.
