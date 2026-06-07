# 🎟️ Charity Auction Platform - Luxe 2026

Charity Auction Management System is a multi-layered web application designed to streamline benefit auction operations, manage donor profiles, and track ticket purchases efficiently. This project showcases modern development standards with an enterprise-grade backend architecture and a dynamic frontend environment.

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
    <b>👤 טופס הוספת תורם חדש למערכת:</b>
    <br><br>
    <img src="add-donor-modal.png.png" alt="Add Donor Form" width="90%">
  </p>
  <br><br>
</details>

---

## 🛠️ Tech Stack & Architecture

### Backend:
* **Framework:** C# / .NET Core 8.0 / Web API
* **Architecture:** RESTful Services, Clean/Multi-layered Architecture (Controllers, Services, Repositories)
* **Database & ORM:** SQL Server via Entity Framework Core (Code-First)
* **Security & Authentication:** JWT (JSON Web Tokens), Role-Based Authorization, Password Hashing via BCrypt

### Frontend:
* **Framework:** Angular 17+ / TypeScript
* **State & Forms:** Reactive Forms, Validation handling, Component-Driven Architecture
* **State Management & Async:** RxJS Observables & Operators
* **Styling:** Tailwind CSS / Modern CSS Components

---

## 🚀 הוראות הרצה / Installation & Setup

### דרישות קדם (Prerequisites)
* .NET 8.0 SDK
* Node.js (Version 18+) & Angular CLI
* SQL Server

### 🖥️ הרצת ה-Backend (Server)
נווט אל תיקיית השרת, עדכן את מחרוזת החיבור (Connection String) בקובץ `appsettings.json`, והרצ את הפקודות הבאות ברצף:
```bash
cd project-web-api
dotnet ef database update
dotnet run


🌐 הרצת ה-Frontend (Client)
נווט אל תיקיית הלקוח והרצ את פקודות ההתקנה וההפעלה ברצף:

Bash
cd angular
npm install
ng serve
לאחר מכן פתח את הדפדפן בכתובת: http://localhost:4200

🔐 פרטי התחברות לבדיקה (Testing Credentials)
כדי לבדוק את פיצ'רי הניהול המלאים של המערכת, ניתן להתחבר עם משתמש המנהל המובנה:

אימייל: m@m.com

סיסמה: admin123
