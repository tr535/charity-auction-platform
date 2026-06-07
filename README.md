🎟️ Charity Auction Platform - Luxe 2026

Charity Auction Management System is a multi-layered web application designed to streamline benefit auction operations, manage donor profiles, and track ticket purchases efficiently. This project showcases modern development standards with an enterprise-grade backend architecture and a dynamic frontend environment.

📸 צילומי מסך / Screenshots
🏠 דף הבית וחוויית הרכישה
<p align="center"> <img src="home-page.jpg.png" alt="Home Page" width="85%"> </p> <br><br>
🛒 עגלת קניות מפורטת ואישור רכישה
<p align="center"> <img src="shopping-cart.jpg.png" alt="Active Shopping Cart" width="85%"> </p> <br><br>
⚙️ ממשק ניהול וביצוע הגרלות (Admin Panel)
<details> <summary><b>לחץ כאן לצפייה במסכי המנהל, הוספת מתנות והדאשבורד 📊</b></summary> <br><br> <p align="center"> <b>📊 דאשבורד מעקב הכנסות וניהול זוכים:</b> <br><br> <img src="admin-dashboard.png.png" alt="Admin Dashboard" width="90%"> </p>

<br><br>

<hr style="border: 1px dashed #eaeaea;"> <br><br> <p align="center"> <b>🎁 טופס הוספת תורם ומתנה חדשה למערכת:</b> <br><br> <img src="add-donor-modal.png.png" alt="Add Gift Form" width="90%"> </p>

<br><br>

</details>
🛠️ Tech Stack & Architecture
Backend
Framework: C# / .NET Core 8.0 / Web API
Architecture: RESTful Services, Clean/Multi-layered Architecture (Controllers, Services, Repositories)
Database & ORM: SQL Server via Entity Framework Core (Code-First)
Security & Authentication: JWT (JSON Web Tokens), Role-Based Authorization, Password Hashing via BCrypt
Frontend
Framework: Angular 17+ / TypeScript
State & Forms: Reactive Forms, Validation Handling, Component-Driven Architecture
State Management & Async: RxJS Observables & Operators
Styling: Tailwind CSS / Modern CSS Components
🚀 Getting Started
Prerequisites

לפני הרצת הפרויקט יש לוודא שמותקנים:

.NET SDK 8.0
SQL Server
Node.js (v18+ מומלץ)
Angular CLI
npm install -g @angular/cli
⚙️ Backend Setup
1. Clone the Repository
git clone https://github.com/your-username/charity-auction-platform.git
cd charity-auction-platform
2. Configure Database

עדכן את מחרוזת החיבור בקובץ:

appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=CharityAuctionDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
3. Apply Migrations
dotnet ef database update
4. Run the API
dotnet restore
dotnet build
dotnet run

ברירת המחדל:

https://localhost:5001

או

http://localhost:5000
💻 Frontend Setup
1. Navigate to Client Folder
cd client
2. Install Dependencies
npm install
3. Run Angular Application
ng serve

האפליקציה תהיה זמינה בכתובת:

http://localhost:4200
🔗 API Configuration

יש לוודא שקובץ הסביבה של Angular מצביע לכתובת ה־API:

export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api'
};
🔐 פרטי התחברות לבדיקה (Testing Credentials)

כדי לבדוק את פיצ'רי הניהול המלאים של המערכת, ניתן להתחבר עם משתמש המנהל המובנה:

שדה	ערך
אימייל	m@m.com
סיסמה	admin123
📂 Project Structure
Backend
│
├── Controllers
├── Services
├── Repositories
├── Models
├── DTOs
├── Middleware
└── Data

Frontend
│
├── src
│   ├── app
│   ├── components
│   ├── services
│   ├── models
│   └── shared
✨ Main Features
🎟️ רכישת כרטיסי הגרלה
🎁 ניהול תורמים ומתנות
👥 מערכת משתמשים והרשאות
🔐 JWT Authentication
📊 Dashboard לניהול ומעקב
🏆 בחירת זוכים אוטומטית
📱 Responsive Design
⚡ Angular + .NET 8 Performance
📄 License

This project was developed for educational and portfolio purposes.
