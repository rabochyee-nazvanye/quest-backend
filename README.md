# Questspace - backend API

Questspace is a platform to organize and participate in various quests. With our state-of-the-art platform, quest organizers can create and organize a quest from scratch in minutes, not days.

The platform is easily scalable and extensible, it supports different types of tasks and quests. Try it out at https://questspace.live!

This is a .NET Core REST API application implemented with basic CQRS approach and Domain Driven Design.

### Technologies
* ASP.NET Core 3.1
* ASP.NET Core Identity 
* JWT for API authentication
* Entity Framework Core 3.1
* MediatR
* Sentry
* NUnit
* PostgreSQL

### Solution structure
Solution is divided into several projects:

* Quest.Domain - Domain layer, contains the main business logic entities
* Quest.DAL - Data Access Layer, contains the DbContext (through which the interaction with the database goes) and stores the migration history. The repository is the Entity Framework ORM, which already implements the Unit Of Work pattern. Depends on the Quest.Domain layer
* Quest.Application - Application layer, contains Commands, Queries requests and their handlers, each of which contains the implementation of some kind of interaction with the application. Depends on Quest.DAL and Quest.Domain
* Quest.API is a presentation layer, responsible for interacting with users through the REST API. Depends on layers Domain, DAL, Application. Also this layer has the SignalR hub used as message bus with the telegram bot. Telegram bot receives requests for manual verification of tasks by moderators.

### Setup
Follow these steps to get your development environment set up:

  1. Clone the repository
     ```
     git clone https://github.com/rabochyee-nazvanye/quest-backend/
     ```
  2. Make sure that you have the PostgreSQL database cluster up and running somewhere 
  3. Create the appsettings.json file in the `Quest.API` directory with the following contents (replace the corresponding credentials with yours)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=SERVER_IP_OR_FQDN;Database=DATABASE_NAME;User Id=DATABASE_USER; Password=DATABASE_PASSWORD; Maximum Pool Size=1024"
  },
  "CorsOrigins": [
    "http://place.your.safe.origins.here (used only in production mode)",
    "https://and.here"
  ],
  "Jwt": {
    "Key": "YOUR_JWT_SECRET_KEY",
    "Issuer": "YOUR_JWT_KEY_ISSUER"
  },
  "Sentry": {
    "Dsn": "YOUR_SENTRY_DSN"
  }
}
```
  4. At the root directory, restore required packages by running:
     ```
     dotnet restore
     ```
  5. Update the database (if empty)
     ```
     dotnet ef -s Quest.API -p Quest.DAL database update
     ```
  6. Build the solution by running:
     ```
     dotnet build
     ```
  7. Next, within the `Quest.API` directory, launch the backend by running:
     ```
     dotnet run
     ```
  8. Visit [http://localhost:5000/swagger](http://localhost:5000/swagger) in your browser to get info about the API capabilites.

Note: if you want to deploy a complete Questspace installation, you should also run [questspace-frontend](https://github.com/rabochyee-nazvanye/quest-frontend) to interact with this API.
