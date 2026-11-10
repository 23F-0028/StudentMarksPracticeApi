
# Student Marks Management API

A RESTful Web API practice project developed using ASP.NET Core Web API, C#, Entity Framework Core, and PostgreSQL. The system manages students, courses, enrollments, marks, authentication, and role-based access control.

## Project Overview

The Student Marks Management API provides endpoints for managing student records, course information, enrollments, and academic marks. It also supports JWT-based authentication and authorization for Admin and Student users.

## Technologies Used

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Npgsql
- JWT Authentication
- RESTful API
- PowerShell
- Git and GitHub

## Main Features

### 1. Student Management
- Create student records.
- Retrieve student information.
- Update student records.
- Delete student records.
- Restrict students to viewing their own records.

### 2. Course Management
- Create courses.
- Retrieve course information.
- Update course details.
- Delete courses.

### 3. Enrollment Management
- Enroll students in courses.
- Retrieve enrollment records.
- Update enrollment marks.
- Delete enrollments.
- Prevent duplicate enrollments.
- Apply role-based access restrictions.

### 4. Marks Management
- Store student marks.
- Validate marks within the range of 0 to 100.
- Calculate course statistics.
- Calculate student weighted averages.
- Generate student transcripts.

### 5. Authentication and Authorization
- User registration and login.
- Password hashing.
- JWT token generation and validation.
- Admin and Student roles.
- Protected API endpoints.
- Student data ownership restrictions.
- Admin access to management operations.

## Project Structure

```text
MarksPracticeApi/
│
├── Controllers/
│   ├── AdminController.cs
│   ├── AuthController.cs
│   ├── CoursesController.cs
│   ├── EnrollmentsController.cs
│   └── StudentsController.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── DTOs/
│   ├── Auth/
│   ├── Courses/
│   ├── Enrollments/
│   └── Students/
│
├── Migrations/
│
├── Models/
│   ├── Student.cs
│   ├── Course.cs
│   ├── Enrollment.cs
│   └── User.cs
│
├── Services/
│   └── PasswordService.cs
│
├── Properties/
│
├── Program.cs
├── appsettings.json
├── MarksPracticeApi.csproj
└── README.md
```

## Prerequisites

Install the following software before running the project:

- .NET 10 SDK
- PostgreSQL
- Visual Studio Code or Visual Studio
- Git

## Database Configuration

The project uses PostgreSQL as its database and Entity Framework Core for database operations.

1. Install and start PostgreSQL.
2. Create a database named `marks_practice_db`.
3. Configure the database connection using .NET User Secrets.
4. Configure JWT settings using .NET User Secrets.

Do not publish database passwords, JWT signing keys, or other sensitive configuration values to GitHub.

## Running the Project

Clone the repository:

```powershell
git clone https://github.com/23F-0028/StudentMarksPracticeApi.git
```

Navigate to the project directory:

```powershell
cd StudentMarksPracticeApi
```

Restore dependencies:

```powershell
dotnet restore
```

Apply database migrations:

```powershell
dotnet ef database update
```

Build the project:

```powershell
dotnet build
```

Run the API:

```powershell
dotnet run
```

The local API address used during development is:

```text
http://localhost:5219
```

## Database Migrations

Entity Framework Core migrations are used to create and update the database schema.

The project includes migrations for:

1. Initial database creation.
2. Course management.
3. Enrollment management.
4. User authentication.
5. Sample data seeding.
6. Linking students with user accounts.

To view migrations:

```powershell
dotnet ef migrations list
```

To update the database:

```powershell
dotnet ef database update
```

## API Modules

| Module | Description |
|---|---|
| Authentication | User registration and login |
| Admin | Administrative operations |
| Students | Student record management |
| Courses | Course management |
| Enrollments | Enrollment and marks management |

## Authorization Rules

The API implements role-based authorization.

### Admin
- Access administrative operations.
- View student and enrollment records.
- Manage enrollment records.
- Perform authorized management operations.

### Student
- Access protected student functionality.
- View their own student record.
- View their own enrollment information.
- Access permitted course information.
- Cannot perform restricted enrollment write operations.

## Testing

The project has been manually tested using API requests.

Tested functionality includes:

- Student and course operations.
- Enrollment creation, updating, and deletion.
- Marks validation.
- Course statistics.
- Student transcripts.
- User authentication.
- JWT authorization.
- Student data ownership.
- Admin and Student access restrictions.

## Security

- Passwords are stored using password hashing.
- JWT authentication protects secured endpoints.
- Role-based authorization restricts access.
- Student ownership checks protect student-specific records.
- Local secrets are stored outside the source code.
- Sensitive configuration files are excluded through `.gitignore`.

## GitHub Repository

Source code:

https://github.com/23F-0028/StudentMarksPracticeApi

## Project Status

This is an academic practice project developed to improve understanding of ASP.NET Core Web API development, database integration, authentication, authorization, and REST API design.

Some additional documentation, automated tests, and deployment improvements may be added in future updates.

## Author

**Muhammad Jawad**

GitHub: https://github.com/23F-0028
