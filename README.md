# Project

Sample .NET Core REST API project

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
2. **Run Program**
    - open project with vs code
    - go to API directory
        cd ./DOTProject.API
    - build application to ensure there are not errors
        dotnet build
    - if there ara no errors, run API
        dotnet run
    - open swagger to show endpoints list
        http://localhost:5258/swagger/index.html
3. **EF Migration or execute script backup database**
    - open project
    - execute migration
        ```bash
        dotnet ef migrations add InitialCreate --project .\DOTProject.Infrastructure\DOTProject.Infrastructure.csproj --startup-project .\DOTProject.API\DOTProject.API.csproj

        dotnet ef database update --project .\DOTProject.Infrastructure\DOTProject.Infrastructure.csproj --startup-project .\DOTProject.API\DOTProject.API.csproj

    ## add seed
        ```bash
        dotnet ef migrations add SeedData --project .\DOTProject.Infrastructure\DOTProject.Infrastructure.csproj --startup-project .\DOTProject.API\DOTProject.API.csproj

        dotnet ef database update --project .\DOTProject.Infrastructure\DOTProject.Infrastructure.csproj --startup-project .\DOTProject.API\DOTProject.API.csproj
    - or execute script backup file attached (backupdb.sql)
4. **API Endpoint**
    - Product Authenticate
        POST /api/Authenticate/login: Generate JWT Token
    - Product Endpoints
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
5. **How To use API**
    - Open swagger http://localhost:5258/swagger/index.html to show endpoint list
    - Call /api/Authenticate/login for get token for authentication with user login below and 
        username : admin
        password : Admin#1234

        JSON Request : { "username": "admin","password": "Admin#1234"}
        note : you can call by postman or swagger
    - Call other endpoint with postman (or similar) and add authorization Bearer Token with the token we got earlier
        you can use the attached postman file (DOT Project.postman_collection.json)
    - link demo https://www.loom.com/share/44dc03648d134c1892d5b208520f0301?sid=e7bae2d3-b863-4ef1-b42d-4f6a76c5bd49
6. **Interview Pengalaman kerja**
    link interview https://www.loom.com/share/d580637de41246439d853322a7ac47ed