# SampleRestApi Project

This is a sample .NET Core REST API project demonstrating the use of Onion Architecture, Entity Framework Core with Code-First and Migrations, CRUD operations, relationship management with eager and lazy loading, data validation, error handling, caching, and implementation of OOP/SOLID principles.

## Table of Contents
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)
- [Setup Instructions](#setup-instructions)
- [Running the Project](#running-the-project)
- [API Endpoints](#api-endpoints)
- [Error Handling](#error-handling)
- [Caching](#caching)
- [Validation](#validation)
- [OOP/SOLID Principles](#oops-solid-principles)

## Project Structure

The project follows the Onion Architecture and is divided into several layers:
- **Core (Domain)**: Contains entities and interfaces.
- **Infrastructure**: Contains the database context, configurations, and migrations.
- **Application**: Contains business logic and services.
- **Presentation**: Contains controllers and API endpoints.

## Technologies Used

- .NET Core
- Entity Framework Core
- SQL Server
- FluentValidation
- MemoryCache
- ASP.NET Core MVC

## Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone https://github.com/solafidedev/DOTProject.git
   cd DOTProject

2. **EF Migration**
    dotnet ef migrations add InitialCreate --project .\DOTProject.Infrastructure\DOTProject.Infrastructure.csproj --startup-project .\DOTProject.API\DOTProject.API.csproj

    dotnet ef database update --project .\DOTProject.Infrastructure\DOTProject.Infrastructure.csproj --startup-project .\DOTProject.API\DOTProject.API.csproj

    ## add seed
    dotnet ef migrations add SeedData --project .\DOTProject.Infrastructure\DOTProject.Infrastructure.csproj --startup-project .\DOTProject.API\DOTProject.API.csproj

    dotnet ef database update --project .\DOTProject.Infrastructure\DOTProject.Infrastructure.csproj --startup-project .\DOTProject.API\DOTProject.API.csproj

3. **API Endpoint**
    - API Endpoints
        Product Endpoints
        GET /api/products: Get all products.
        GET /api/products/{id}: Get a product by ID.
        POST /api/products: Create a new product.
        PUT /api/products/{id}: Update an existing product.
        DELETE /api/products/{id}: Delete a product.
    - Category Endpoints
        GET /api/categories: Get all categories.
        GET /api/categories/{id}: Get a category by ID.
        POST /api/categories: Create a new category.
        PUT /api/categories/{id}: Update an existing category.
        DELETE /api/categories/{id}: Delete a category.