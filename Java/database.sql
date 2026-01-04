-- Create tables for Brasil Burger application

-- Users table for authentication
CREATE TABLE IF NOT EXISTS users (
    id_user SERIAL PRIMARY KEY,
    login VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS burgers (
    id_burger SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    prix DECIMAL(10, 2) NOT NULL,
    image VARCHAR(255),
    actif BOOLEAN DEFAULT true
);

CREATE TABLE IF NOT EXISTS menus (
    id_menu SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    actif BOOLEAN DEFAULT true
);

CREATE TABLE IF NOT EXISTS complements (
    id_complement SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    prix DECIMAL(10, 2) NOT NULL,
    actif BOOLEAN DEFAULT true
);

-- Sample data
INSERT INTO users (login, password) VALUES 
('admin', 'admin123'),
('user', 'user123');

INSERT INTO burgers (nom, prix, image, actif) VALUES 
('Burger Classic', 8.50, 'classic.jpg', true),
('Burger Cheese', 9.00, 'cheese.jpg', true);

INSERT INTO menus (nom, actif) VALUES 
('Menu Enfant', true),
('Menu Maxi', true);

INSERT INTO complements (nom, prix, actif) VALUES 
('Frites', 3.50, true),
('Boisson', 2.50, true);
