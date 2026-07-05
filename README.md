# 🎓 Student Management System

A Student Management System built using **ASP.NET Core 8 Web API** and **ASP.NET Core MVC** with **JWT Authentication**, **SQL Server**, **Entity Framework Core**, and **Excel Export**.

---

## 🚀 Features

- User Registration
- User Login
- JWT Authentication
- Role-Based Authorization
- Student CRUD Operations
- Search & Filter
- Pagination
- Export Students to Excel
- Exception Handling Middleware
- Entity Framework Core
- SQL Server Database

---

## 🛠️ Technologies Used

- ASP.NET Core 8 MVC
- ASP.NET Core 8 Web API
- C#
- SQL Server
- Entity Framework Core
- JWT Authentication
- ClosedXML (Excel Export)
- Bootstrap 5

---

## 📁 Project Structure

```
StudentManagementAPI
│
├── Controllers
├── Models
├── DTOs
├── Services
├── Middleware
├── Data
└── Program.cs

StudentManagementUI
│
├── Controllers
├── Models
├── Views
├── wwwroot
└── Program.cs
```

---

## 🔐 Authentication

- User Registration
- User Login
- JWT Token Generation
- Role-Based Authorization
- Session-based Token Storage in MVC

---

## 📚 Student Features

- Add Student
- Update Student
- Delete Student
- View Student Details
- Search Students
- Pagination
- Export Students to Excel

---

## 📡 API Endpoints

### Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/Auth/Register` | Register User |
| POST | `/api/Auth/Login` | Login User |

### Student

| Method | Endpoint |
|--------|----------|
| GET | `/api/Student` |
| GET | `/api/Student/{id}` |
| POST | `/api/Student` |
| PUT | `/api/Student/{id}` |
| DELETE | `/api/Student/{id}` |
| GET | `/api/Student/export` |

---

## ⚙️ Installation

### Clone Repository

```bash
git clone https://github.com/nishant-21004/StudentAPI1.git
```

### Open Solution

Open the solution in **Visual Studio 2022**.

### Configure Database

Update the SQL Server connection string in:

```
appsettings.json
```

Run Entity Framework migrations:

```bash
Update-Database
```

### Run API

```
StudentManagementAPI
```

### Run MVC

```
StudentManagementUI
```

---

## 🔑 Default Flow

1. Register User
2. Login
3. JWT Token Generated
4. Token Stored in Session
5. Access Student Module
6. Perform CRUD Operations
7. Export Student Data to Excel

---

## 📸 Screenshots

- Login Page
- Register Page
- Student List
- Add Student
- Edit Student
- Delete Student
- Excel Export

(Add screenshots here.)

---

## 👨‍💻 Author

**Nishant Singh**

GitHub: https://github.com/nishant-21004

---

## 📄 License

This project is for learning and educational purposes.
