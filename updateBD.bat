cd HealthcareApp
dotnet ef database drop
rmdir /s Migrations
dotnet ef migrations add InitialCreate 
dotnet ef database update