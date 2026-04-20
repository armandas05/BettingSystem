# iBet – Betting Platform

A modern betting system built with ASP.NET Core and JavaScript.

---

## Features

### Games

* Blackjack (full logic, dealer AI, animations)
* Dice (probability-based betting system)

### User System

* Authentication & registration
* Balance management
* Simple Deposit system

### Admin Dashboard

* Analytics (charts, KPIs)
* User management
* Payments tracking
* Game history

---

## Tech Stack

* ASP.NET Core (C#)
* Entity Framework Core
* JavaScript (Vanilla)
* Chart.js
* SQL Server

---

## Preview


---

## ⚙️ Setup

### 1. Clone repo

```
git clone https://github.com/yourusername/BettingSystem.git
cd BettingSystem
```

### 2. Configure database

Update connection string in:

```
appsettings.json
```

### 3. Run migrations

```
dotnet ef database update
```

### 4. Run project

```
dotnet run
```

---

## 🔑 Demo Accounts

### 👑 Admin

* Email: [admin@test.com](mailto:admin@test.com)
* Password: 123456

### 👤 User

* Email: [user@test.com](mailto:user@test.com)
* Password: 123456

---

## 📌 Future Improvements

* RabbitMQ integration
* Real-time updates (SignalR)
* More games (Roulette, High/Low)
* Bug fixes

---

## Notes

Built as a portfolio project.
