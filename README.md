# 💼 Accounting Management System - Backend

This is the backend part of the **Accounting Management System** developed using **.NET 8**. It employs **Clean Architecture**, **Repository Pattern**, and **Mediator Pattern** to manage accounting entities such as currencies, Chart of Accounts, sub-ledgers, and financial periods.

🔗 **Frontend Demo**: [ERP Frontend System](https://erp-frontend-system.netlify.app)

🔗 **Frontend Repo**: [Accounting Management System](https://github.com/AhmedYounis20/aaa-dashboard)

## 🛠️ Technologies Used

![dotnet](https://img.shields.io/badge/.NET-8-512BD4?logo=.net&logoColor=white)  
![mssql](https://img.shields.io/badge/MSSQL-CC2927?logo=microsoft-sql-server&logoColor=white)  
![fluentvalidation](https://img.shields.io/badge/FluentValidation-71B72B?logo=fluent&logoColor=white)  
![mediatr](https://img.shields.io/badge/MediatR-00A9E0?logo=mediatr&logoColor=white)  
![swagger](https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=white)  

- **.NET 8** for backend development
- **Clean Architecture** and **Repository Pattern** for maintainable, scalable architecture
- **Mediator Pattern** for separating concerns in business logic
- **Entity Framework Core** for ORM with **SQL Server**
- **FluentValidation** for model validation
- **Swagger** for API documentation

## 📂 Features

- **Currency Management**: Endpoints to manage multiple currencies and exchange rates.
- **Chart of Accounts**: APIs to manage and retrieve hierarchical account data.
- **Subledger Management**: CRUD operations for banks, customers, suppliers, cash inboxes, and fixed assets.
- **General Ledger**: Settings management and entry management for different entry types.
- **Financial Periods**: Manage different financial periods for the accounting process.
- **Validation**: Cross-cutting validation for all API inputs using **FluentValidation**.
- **Separation of Concerns**: Clear separation between API, service, and model layers.

## 🔧 Setup Instructions

### **Prerequisites**

- **.NET 8 SDK**
- **SQL Server** for database storage

### **Steps to Run Locally:**

1. Clone the repository:
   ```bash
   git clone https://github.com/AhmedYounis20/AAA.ERP.git
   ```

2. Navigate to the project directory:
   ```bash
   cd AAA.ERP
   ```

3. Restore dependencies:
   ```bash
   dotnet restore
   ```

4. Set up the SQL Server database and configure the connection string in `appsettings.json`.

5. Build and run the application:
   ```bash
   dotnet run
   ```

   The API will be available at `https://localhost:5001` (or the configured port).

### **Testing the APIs**

You can use **Postman** or **Swagger UI** (available at `/swagger`) to interact with the APIs.
