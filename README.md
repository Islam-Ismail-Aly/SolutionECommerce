# E-CommerceApp

## Overview
**E-CommerceApp** is a comprehensive online shopping platform designed to facilitate seamless transactions, manage product listings, and handle user authentication and authorization. This project leverages modern technologies and architectural patterns to ensure a robust, scalable, and maintainable codebase.

## Features
- **Product Management**
  - Add, update, delete, and view products with detailed descriptions and categories
  - 
- **User Management**
  - Registration and login with role-based access control (Admin and User roles)
  - 
- **Order Management**
  - Manage shopping cart, create orders, and track order status
  - 
- **Authentication and Authorization**
  - Implemented using Microsoft Identity and JSON Web Tokens (JWT) for secure API access
  - 
- **Data Mapping and Validation**
  - AutoMapper for object-to-object mapping
  - FluentValidation for clean and fluent validation rules
  - 
- **Design Patterns**
  - Repository Pattern for data access
  - Unit of Work for managing transactions
  - MVC (Model-View-Controller) pattern for organizing the application
  - 
- **Dashboard**
  - Display counts, charts, and other metrics for a quick overview.
  - 
- **Role Management**
  - Different roles for Admin and User with specific permissions

## Technologies Used
- **Backend**: .NET Core 8.0, MS Identity, JWT, AutoMapper, FluentValidation, FluentAPI
- **Database**: SQL Server
- **Architecture**: Layered Architecture, Code First Approach

## Project Structure

E-CommerceApp/
├── src/
│   ├── Application/
│   │   └── Marketoo.Application/
│   │       ├── DTOs
│   │       ├── Services
│   │       ├── Interfaces
│   │       ├── Utilities
│   │       ├── Repositories
│   │       ├── Mapper
│   │       ├── Validations
│   │       ├── Pagination
│   │       └── ...
│   ├── Core/
│   │   └── Marketoo.Core/
│   │       ├── Entities
│   │       ├── Interfaces
│   │       └── ...
│   ├── Infrastructure/
│   │   └── Marketoo.Infrastructure/
│   │       ├── Data
│   │       ├── Migrations
│   │       ├── Repositories
│   │       ├── UnitOfWork
│   │       └── ...
│   ├── Web/
│   │   └── Marketoo.ECommerceAPI/
│   │       ├── wwwroot
│   │       ├── Admin
│   │       ├── Exceptions
│   │       ├── Domain
│   │       └── ...
