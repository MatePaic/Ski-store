# Ski & Snowboard E-Commerce Store

Welcome to the Ski & Snowboard E-Commerce Store! This project is a fully functional online store built using a modern tech stack, including **.NET WebAPI** for the backend and **React** for the frontend. The application allows users to browse, search, and purchase ski and snowboard equipment, manage their shopping cart, and securely checkout using Stripe.

---

## Features

### Backend (.NET WebAPI)
- **Repository, Unit of Work, and Specification Patterns**: Implemented to ensure clean, maintainable, and scalable code.
- **Entity Framework Core**: Used for database operations, including querying and updating data.
- **ASP.NET Identity**: Handles user authentication, login, and registration.
- **Automapper**: Simplifies data shaping and mapping between entities and DTOs.
- **Stripe Integration**: Secure payment processing with support for 3D Secure (EU standards).
- **Discount Coupons**: Users can apply promo codes for discounts during checkout.
- **Paging, Sorting, Searching, and Filtering**: Enhanced user experience with dynamic data handling.

### Frontend (React)
- **React Router**: Enables seamless navigation between pages in the single-page application.
- **Material Design**: A modern and responsive UI built using Material-UI components.
- **Reusable Form Components**: Built using **React Hook Form** for efficient form management.
- **Shopping Cart**: Users can add/remove items and create orders from the cart.
- **Order Management**: Users can view their order history and track orders.

### DevOps
- **CI/CD Workflows**: Automated build, test, and deployment pipelines using **GitHub Actions**.
- **Azure Deployment**: The application is hosted on **Microsoft Azure** for scalability and reliability.

---

## Technologies Used

### Backend
- **.NET Core WebAPI**
- **Entity Framework Core**
- **ASP.NET Identity**
- **Automapper**
- **Stripe API**

### Frontend
- **React**
- **React Router**
- **Material-UI**
- **React Hook Form**

### DevOps
- **GitHub Actions**
- **Microsoft Azure**

---

## Getting Started

### Prerequisites
- **.NET SDK** (version 6.0 or later)
- **Node.js** (version 16 or later)
- **SQL Server** (or another database supported by Entity Framework Core)
- **Stripe Account** (for payment processing)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/ski-snowboard-store.git
2. Restore dependencies:
   ```bash
   dotnet restore
3. Update the database connection string in appsettings.json
4. Stripe API Keys: Add your Stripe publishable and secret keys to the backend configuration.
5. Navigate to the frontend folder and install dependencies:
   ```bash
   cd ../client
   npm install
6. Start the backend and frontend servers:
   ```bash
   ###backend
   cd ../API
   dotnet run
   
   ###frontend
   cd ../client
   npm run dev
