# CarBrand API

RESTful API for managing vehicle brands, built with **ASP.NET Core 8** and **PostgreSQL**.

This project demonstrates backend development using a layered architecture, Entity Framework Core, Docker, and cloud deployment.

## Technologies

- **C# / .NET 8** – Backend framework.
- **ASP.NET Core Web API** – REST API development.
- **Entity Framework Core** – ORM and database management.
- **PostgreSQL** – Relational database.
- **Npgsql** – PostgreSQL provider for EF Core.
- **Docker** – Containerization.
- **Swagger / OpenAPI** – API documentation.
- **Render** – API hosting.
- **Neon** – Cloud PostgreSQL database.

## Features

- Create vehicle brands.
- Retrieve all vehicle brands.
- Retrieve a vehicle brand by ID.
- Update existing vehicle brands.
- Delete vehicle brands.
- Paginated results.
- Database migrations with Entity Framework Core.

## Architecture

The solution is organized into separate projects:

- **CarBrand.Api:** HTTP endpoints, controllers, and application configuration.
- **CarBrand.Application:** Application services and business logic.
- **CarBrand.Domain:** Domain entities and contracts.
- **CarBrand.Infrastructure:** Database context, repositories, and persistence.

## API Endpoints

Base URL:

`https://carbrand-api.onrender.com`

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/CarBrand` | Get all brands |
| GET | `/api/CarBrand/{id}` | Get a brand by ID |
| POST | `/api/CarBrand` | Create a brand |
| PUT | `/api/CarBrand/{id}` | Update a brand |
| DELETE | `/api/CarBrand/{id}` | Delete a brand |

## API Documentation

Explore and test the endpoints using Swagger:

[Open Swagger Documentation](https://carbrand-api.onrender.com/swagger/index.html)

## Run Locally

### Prerequisites

- .NET 8 SDK
- Docker Desktop
- Git

### Clone the repository

```bash
git clone https://github.com/JSalaverry25/CarBrand-API.git
cd CarBrand-API
```

### Configure environment variables

Create a `.env` file in the root directory and configure the database password:

```env
DB_PASSWORD
