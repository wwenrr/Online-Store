## How to create a migration
1. Open `Package Manager Console`
2. Select default project: `Training.DataAccess`
3. Run: `Add-Migration <MigraitonName>`
4. Add `// <auto-generated>` on top of the file to suppress the "using directive" warning

## How to update database
1. Set `Training.Api` as default startup project.
2. Configure the correct connection string in `appsettings.json` in `Training.Api` project

### Method 1:
3. Open `Package Manager Console`:
4. Select default project: `Training.DataAccess`
5. Run: `Update-Database`

### Method 2:
3. Set environment `AutoMigration=true` in `appsettings.json` or `appsettings.Development.json` in `Training.Api` project
4. Run the app
