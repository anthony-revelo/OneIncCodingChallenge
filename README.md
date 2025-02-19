# User Management API

This project is a User Management API built using ASP.NET Core. It provides a RESTful API for managing users, including creating, reading, updating, and deleting users.

## Technologies Used

- **ASP.NET Core 8**: Framework for building the API.
- **FluentValidation**: Library for validation of model objects.
- **SQL Server**: Database management system.
- **Swagger (Swashbuckle)**: Library for API documentation.
  > I want to point out that I had issues running Swagger for this solution, that's why I'm providing the postman collection for testing purposes
- **xUnit**: Testing framework.
- **Moq**: Library for creating mock objects in unit tests.
- **Postman**: Tool for API testing and documentation.

## Project Structure

The project is organized into the following layers:

- **UserManagement.Api**: Contains the API controllers and configuration.
- **UserManagement.Application**: Contains the service layer, business logic, and interfaces.
- **UserManagement.Domain**: Contains the domain entities and validation logic.
- **UserManagement.Infrastructure**: Contains the data access layer, repository implementations, and database client.
- **Test folder**: Inside this folder you can find the testing projects for each of the layers.  

  > I used the Clean Architecture approach to maintain a good order in the project structure and also to keep a separation of concerns, makin it easier to evolve the application in the future. Some of the good practices I implemented in this project are:
  > **Dependendcy injection :** \
  > - With this you can decouple the diferent components of the application, so they depend in abstractions (interfaces in this case) rather than objects. This enforces some of the main ideas of the SOLID Principles
  > - I made use of the IOptionsMonitor to inject the configuration values from the appsettings file, this allows the application to detect changes in those values and act accordingly dynamically updating the values without restarting the app.
  > - With the inclussion of fluentvalidation nuget, I inteded to separate the concern of model validations in a single layer, making it easier to maintain, and giving me the ability to use this for example to validate if any configuration is missing at the start of the application. you can easylly test this by removing the connectionstring value from the appsettings file  and see what happens when you try to start the app.
  > **Unit Testing :**
  > - This is crucial in the modern development world, to ensure the quality of your software and provide an easy way to detect issues during the development process.
  > >I want to point out that the unit tests for this project were generated using Copilot, this allowed me to generate all the boilerplate and focus on generating the actual functionality

## Configuration

The project uses `appsettings.json` for configuration. Ensure the file is included in the project and set to copy to the output directory.

Example `appsettings.json`:

<code>
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
</code>

## Running the Project
To run the project, follow these steps:
- Restore NuGet Packages: Run dotnet restore to restore all the required packages.
- Update Database Connection: Ensure the connection string in appsettings.json is correctly configured.
- Run the Database Initializer: Ensure the database and tables are created by running the database initializer.
- Run the API: Use dotnet run to start the API project.

## API Endpoints
The following endpoints are available:
- GET /api/users: Retrieves all users.
- GET /api/users/{id}: Retrieves a user by ID.
- POST /api/users: Adds a new user.
- PUT /api/users/{id}: Updates an existing user.
- DELETE /api/users/{id}: Deletes a user by ID.

## Testing
Unit tests are written using xUnit and Moq. To run the tests, use the following command:

bash
dotnet test

## Postman Collection
A Postman collection is available for testing the API endpoints. The collection uses a variable for the base URL to easily switch between different environments.
> The provided postman collection can be fount in the root directory of the solution, or you can copy it from this readme file

## Importing the Collection
- Open Postman.
- Click on the "Import" button in the top-left corner.
- Select the "Raw text" option.
- Paste the JSON content into the text area.
- Click "Continue" and then "Import".

Example Postman Collection JSON
<code>
{
  "info": {
    "name": "User Management API",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "variable": [
    {
      "key": "baseUrl",
      "value": "http://localhost:5033"
    }
  ],
  "item": [
    {
      "name": "Get All Users",
      "request": {
        "method": "GET",
        "header": [],
        "url": {
          "raw": "{{baseUrl}}/api/users",
          "host": ["{{baseUrl}}"],
          "path": ["api", "users"]
        }
      }
    },
    {
      "name": "Get User By ID",
      "request": {
        "method": "GET",
        "header": [],
        "url": {
          "raw": "{{baseUrl}}/api/users/:id",
          "host": ["{{baseUrl}}"],
          "path": ["api", "users", ":id"],
          "variable": [
            {
              "key": "id",
              "value": ""
            }
          ]
        }
      }
    },
    {
      "name": "Add User",
      "request": {
        "method": "POST",
        "header": [
          {
            "key": "Content-Type",
            "value": "application/json"
          }
        ],
        "body": {
          "mode": "raw",
          "raw": "{\n  \"firstName\": \"John\",\n  \"lastName\": \"Doe\",\n  \"email\": \"johndoe@example.com\",\n  \"dateOfBirth\": \"2000-01-01T00:00:00Z\",\n  \"phoneNumber\": \"1234567890\"\n}"
        },
        "url": {
          "raw": "{{baseUrl}}/api/users",
          "host": ["{{baseUrl}}"],
          "path": ["api", "users"]
        }
      }
    },
    {
      "name": "Update User",
      "request": {
        "method": "PUT",
        "header": [
          {
            "key": "Content-Type",
            "value": "application/json"
          }
        ],
        "body": {
          "mode": "raw",
          "raw": "{\n  \"id\": \"\",\n  \"firstName\": \"Jane\",\n  \"lastName\": \"Doe\",\n  \"email\": \"janedoe@example.com\",\n  \"dateOfBirth\": \"1995-01-01T00:00:00Z\",\n  \"phoneNumber\": \"0987654321\"\n}"
        },
        "url": {
          "raw": "{{baseUrl}}/api/users/:id",
          "host": ["{{baseUrl}}"],
          "path": ["api", "users", ":id"],
          "variable": [
            {
              "key": "id",
              "value": ""
            }
          ]
        }
      }
    },
    {
      "name": "Delete User",
      "request": {
        "method": "DELETE",
        "header": [],
        "url": {
          "raw": "{{baseUrl}}/api/users/:id",
          "host": ["{{baseUrl}}"],
          "path": ["api", "users", ":id"],
          "variable": [
            {
              "key": "id",
              "value": ""
            }
          ]
        }
      }
    }
  ]
}
</code>
