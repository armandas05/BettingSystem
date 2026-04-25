# iBet – Betting Platform

WebAPI project built with ASP.NET Core and JavaScript.

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

### Asynchronous Processing

* RabbitMQ used for background processing
* Separate queues for game events and transactions
* Background consumers handle database operations

---

## Tech Stack

* ASP.NET Core (C#)
* Entity Framework Core
* JavaScript (Vanilla)
* SQL Server
* RabbitMQ

---

## Preview


---

## ⚙️ Setup

### 1. Clone repo

```
git clone https://github.com/armandas05/BettingSystem.git
cd BettingSystem
```

### 2. Configure database

Update connection string in:

```
appsettings.json
```

### 3. Run project

```
dotnet run
```

### 4. Run RabbitMQ (Docker) (OPTIONAL)
```
docker run -d --hostname rabbit --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

### 5. Open RabbitMQ UI
```
http://localhost:15672 
Login: guest / guest
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

* Real-time updates (SignalR)
* More games (Roulette, High/Low)
* Bug fixes

---

## Notes

Built as a portfolio project.
