# Brasil Burger - Restaurant Management System

Java console application using MVC pattern with PostgreSQL database.

## Setup Instructions

### 1. Database Configuration
- Create a PostgreSQL database on Neon (https://neon.tech)
- Run the `database.sql` script to create tables
- Update `src/config/DatabaseConfig.java` with your connection details:
  - JDBC URL
  - Username
  - Password

### 2. Build & Run
```bash
# Compile
javac -cp "lib/*" -d bin src/**/*.java src/*.java

# Run
java -cp "bin;lib/*" Main
```

Or use Maven:
```bash
mvn clean compile
mvn exec:java -Dexec.mainClass="Main"
```

## Project Structure (MVC)
```
src/
├── Main.java                    # Entry point
├── config/
│   └── DatabaseConfig.java      # HikariCP connection pool
├── models/
│   ├── Burger.java              # Entity
│   ├── Menu.java                # Entity
│   ├── Complement.java          # Entity
│   └── dao/
│       ├── BurgerDAO.java       # Data access
│       ├── MenuDAO.java         # Data access
│       └── ComplementDAO.java   # Data access
├── views/
│   ├── MainView.java            # Main menu display
│   ├── BurgerView.java          # Burger display
│   ├── MenuView.java            # Menu display
│   └── ComplementView.java      # Complement display
├── controllers/
│   ├── MainController.java      # Main logic
│   ├── BurgerController.java    # Burger CRUD logic
│   ├── MenuController.java      # Menu CRUD logic
│   └── ComplementController.java # Complement CRUD logic
└── utils/
    └── ConsoleUtils.java        # Console helpers
```

## Features
- CRUD operations for Burgers, Menus, and Complements
- PostgreSQL with HikariCP connection pooling
- Prepared statements (SQL injection prevention)
- Console-based user interface
- Input validation and error handling

## Dependencies
- PostgreSQL JDBC Driver 42.7.1
- HikariCP 5.0.1
