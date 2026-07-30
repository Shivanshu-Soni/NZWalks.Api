Navigate to the Project Directory where your .csproj file is located.

1. Add a Migration
   To create a new migration, use the following command:

## dotnet ef migrations add MigrationName

Replace MigrationName with a descriptive name for your migration (e.g., InitialCreate).

2. Update the Database
   To apply the migration and update the database, use:

## dotnet ef database update

Notes:
Ensure that Entity Framework Core tools are installed in your project. If not, add them with:

## dotnet add package Microsoft.EntityFrameworkCore.Design

If you get an error about missing tools, install the EF Core CLI tools globally:

## dotnet tool install --global dotnet-ef

### Using Package Manager Console (Visual Studio)

1. Add a Migration

```powershell
Add-Migration MigrationName
```

Replace `MigrationName` with a descriptive name.

2. Update the Database

```powershell
Update-Database
```

---

### Reverting Migrations

To remove the last migration (before it has been applied to the database):

**CLI:**

```bash
dotnet ef migrations remove
```

**Package Manager Console:**

```powershell
Remove-Migration
```

---

### Reverting Database to a Previous Migration

**CLI:**

```bash
dotnet ef database update PreviousMigrationName
```

**Package Manager Console:**

```powershell
Update-Database PreviousMigrationName
```

Replace `PreviousMigrationName` with the name of the migration you want to revert to.

---

### Dropping the Database

**CLI:**

```bash
dotnet ef database drop
```

**Package Manager Console:**

```powershell
Drop-Database
```
