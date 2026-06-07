# 🎟️ Charity Auction Platform - Luxe 2026

Charity Auction Management System is a multi-layered web application designed to streamline benefit auction operations, manage donor profiles, and track ticket purchases efficiently. This project showcases modern development standards with an enterprise-grade backend architecture and a dynamic frontend environment.

---


---

## 🛠️ Tech Stack & Architecture

### Backend:
* **Framework:** C# / .NET 8 Web API
* **Architecture:** Layered Architecture (Controllers, BL, DAL)
* **Database & ORM:** SQL Server + Entity Framework Core
* **Security:** JWT Authentication, Role-Based Authorization, BCrypt Password Hashing

### Frontend:
* **Framework:** Angular 17+ / TypeScript
* **State & Forms:** Reactive Forms, Validation handling, Component-Driven Architecture
* **State Management & Async:** RxJS Observables & Operators
* **Styling:** Tailwind CSS / Modern CSS Components

---
---

## 🌟 Highlights

- ⚡ Angular 17 + TypeScript
- 🚀 .NET 8 Web API
- 🗄️ SQL Server & Entity Framework Core
- 🔐 JWT Authentication & Role-Based Authorization
- 🏗️ Clean Multi-Layer Architecture
- 📊 Administrative Dashboard
- 🎟️ Ticket Purchase & Auction Management

---

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/tr535/charity-auction-platform.git
cd charity-auction-platform
```

### 2. Frontend Setup

```bash
cd angular
npm install
ng serve
```

The Angular application will be available at:

```text
http://localhost:4200
```

### 3. Backend Setup

- Open the `.sln` solution file in Visual Studio.
- Update the SQL Server connection string in `appsettings.json`.
- Apply database migrations.
- Run the API project.

API default URL:

```text
https://localhost:5001
```

---

## ✨ Main Features

- 🎟️ Purchase raffle tickets online
- 🛒 Shopping cart and checkout process
- 🎁 Donor and gift management
- 👥 User registration and authentication
- 🔐 JWT-secured API
- 🏆 Automatic winner selection
- 📊 Revenue and winners dashboard
- 📱 Fully responsive design
- ⚙️ Admin management panel

---


## 📸 צילומי מסך / Screenshots

### 🏠 דף הבית וחוויית הרכישה
<p align="center">
  <img src="home-page.jpg.png" alt="Home Page" width="85%">
</p>
<br><br>

### 🛒 עגלת קניות מפורטת ואישור רכישה
<p align="center">
  <img src="shopping-cart.jpg.png" alt="Active Shopping Cart" width="85%">
</p>
<br><br>

---

### ⚙️ ממשק ניהול וביצוע הגרלות (Admin Panel)
<details>
  <summary><b>לחץ כאן לצפייה במסכי המנהל, הוספת מתנות והדאשבורד 📊</b></summary>
  <br><br>
  
  <p align="center">
    <b>📊 דאשבורד מעקב הכנסות וניהול זוכים:</b>
    <br><br>
    <img src="admin-dashboard.png.png" alt="Admin Dashboard" width="90%">
  </p>
  
  <br><br>
  <hr style="border: 1px dashed #eaeaea;">
  <br><br>
  
  <p align="center">
    <b>🎁 טופס הוספת תורם ומתנה חדשה למערכת:</b>
    <br><br>
    <img src="add-donor-modal.png.png" alt="Add Gift Form" width="90%">
  </p>
  <br><br>
</details>


## 🏗️ Architecture

The application follows a clean multi-layered architecture:

```text
Angular Frontend
       │
       ▼
.NET 8 Web API
       │
       ▼
BL (Business Logic)
       │
       ▼
DAL (Data Access Layer)
       │
       ▼
SQL Server Database
```

This architecture promotes maintainability, scalability, and separation of concerns.

---

## 📂 Project Structure

```text
Frontend (Angular)
├── Components
├── Services
├── Guards
├── Interceptors
└── Models

Backend (.NET 8)
├── Controllers
├── BL (Business Logic)
├── DAL (Data Access Layer)
├── Models
├── Middlewares
└── Migrations
```


---




## 🔐 פרטי התחברות לבדיקה (Testing Credentials)
כדי לבדוק את פיצ'רי הניהול המלאים של המערכת, ניתן להתחבר עם משתמש המנהל המובנה:
* **אימייל:** `m@m.com`
* **סיסמה:** `admin123`
