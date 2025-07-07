# 🛠️ C# SQLite CRUD Console App

This is a simple C# console application that demonstrates how to perform basic **CRUD operations** (Create, Read, Update, and Delete) using a local **SQLite database**. It is designed as an educational project for beginners who want to learn how to:

- Connect to a SQLite database in C#
- Create a table if it doesn't already exist
- Insert new records (products with name and price)
- Display all existing records
- Update product prices by name
- Delete products by name

### 📦 Features
- Uses `System.Data.SQLite` for database access
- Full CRUD support with safe, parameterized SQL queries
- Console-based menu for interactive testing
- Simple and lightweight — no external libraries or frameworks required

### 📁 Table Schema
CREATE TABLE AllProducts (
    Name VARCHAR(20),
    Price INT
);

🚀 How to Run
  1. Clone the repository
  2. Open the project in Visual Studio or any C# IDE
  3.Build and run the application
  4.Follow the on-screen menu to perform CRUD operations

✅ Example Menu
1. Create   ||   2. ReadAll   ||   3. Update   ||   4. Delete   ||   0. Exit
