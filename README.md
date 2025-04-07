# Registration Form API

A clean, production-ready ASP.NET Core 8 Web API for user registration using PostgreSQL stored procedures.


# Features

1. Clean Architecture: Follows a three-tier architecture (Presentation, Business, Data Access)
2. PostgreSQL Integration: Uses Npgsql to communicate with PostgreSQL via stored procedures
3. Secure Password Storage: Implements SHA256 password hashing
4. Full CRUD Operations: Complete user management functionality
5. Swagger Documentation: API testing and documentation with Swagger UI
6. Proper Error Handling: Comprehensive error handling at all layers


# Tech Stack

ASP.NET Core 8

PostgreSQL (via Npgsql)

SHA256 password hashing

Swagger/OpenAPI


# Project Structure

RegistrationFormAPI/

├── Controllers/

│   └── UsersController.cs

├── BusinessLayer/

│   ├── Interfaces/

│   │   └── IUserService.cs

│   └── Services/

│   |   └── UserService.cs

├── DataAccessLayer/

│   ├── Interfaces/

│   │   └── IUserRepository.cs

│   └── Repositories/

│   |   └── UserRepository.cs

├── Models/

│   └── User.cs

├── DTOs/

│   └── UserDto.cs

├── Utils/

│   └── PasswordHasher.cs

├── Database/

│   └── stored_procedures.sql

├── appsettings.json

├── Program.cs



# Setup Instructions

Prerequisites : 

.NET 8 SDK

PostgreSQL (version 12 or higher)

# Database Setup

Connect to your PostgreSQL instance:

`psql -U postgres`

Create a new database:

`sql CREATE DATABASE registrationdb;`

Connect to the new database:

`sql \c registrationdb`

Run the SQL script from the Database folder:

`psql -U postgres -d registrationdb -f ./Database/stored_procedures.sql`


# API Setup

Clone the repository:

<pre lang="markdown">git clone [repository-url] 
cd RegistrationFormAPI</pre>

Update the connection string in appsettings.json and appsettings.Development.json with your PostgreSQL credentials.
<pre lang="markdown"> {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=registrationdb;Username=postgres;Password=your_password;"
  }
}  </pre>

Restore NuGet packages:

`dotnet restore`

Build the project:

`dotnet build`

Run the application:

`dotnet run`


# API Endpoints

1. POST /api/users - Register a new user
2. GET /api/users - Get all users
3. GET /api/users/{id} - Get user by ID
4. PUT /api/users/{id} - Update user
5. DELETE /api/users/{id} - Delete user

# Response Format

All API responses follow a standard format:

jsonCopy{
  "Success": true,
  "Message": "Operation successful",
  "Data": { ... }
}


# NOTE INSTALL NUGET PACKAGES WITH SAME VERSIONS MENTIONED :

# Install Npgsql for PostgreSQL connectivity

dotnet add package Npgsql --version 8.0.2

# Install Swagger packages
dotnet add package Swashbuckle.AspNetCore --version 6.5.0

dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.2
