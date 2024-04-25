# Проект Кино

Настоящият проект представлява уеб приложение, в което потребителите могат да си резервират места и да си заплащат билетите.

## Инсталация

- Потребителят трябва да има инсталирани .NET версия 6 и Visual Studio 2022;
- Трябва да се изтегли кодът на проекта – като архив или с командата:
```bash
git clone https://github.com/KrisAngelov/CinemaProject.git
```
- Да се конфигурира връзката с базата данни в DataLayer в клас CinemaDbContext, да се създаде миграция и да се направи базата данни в конзолата с командите:
```bash
add-migration name_of_migration
update-database
```
- Да се конфигурира връзката с базата данни в MVC в appsettings.json:
```csharp
“ConnectionStrings”:{“CinemaDbContextConnection”: ”your_connection_string”
```
- Да се конфигурира връзката с базата данни в SeedingLayer:
```csharp
builder.UseSqlServer("your_connection_string");
```
- Да се стартира SeedingLayer преди тестване на проекта;
- Да се стартира през опцията "MVC", а не през "IIS Express" или "WSL".
