CREATE TABLE zones (
    id_zone SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    prix_livraison DECIMAL(10,2) NOT NULL
);

CREATE TABLE quartiers (
    id_quartier SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    id_zone INT NOT NULL REFERENCES zones(id_zone) ON DELETE CASCADE
);

CREATE TABLE clients (
    id_client SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    prenom VARCHAR(100) NOT NULL,
    telephone VARCHAR(20) UNIQUE NOT NULL,
    email VARCHAR(150) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL
);

CREATE TABLE burgers (
    id_burger SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    prix DECIMAL(10,2) NOT NULL,
    image VARCHAR(255),
    actif BOOLEAN DEFAULT true
);

CREATE TABLE complements (
    id_complement SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    prix DECIMAL(10,2) NOT NULL,
    image VARCHAR(255),
    actif BOOLEAN DEFAULT true
);

CREATE TABLE menus (
    id_menu SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    image VARCHAR(255),
    actif BOOLEAN DEFAULT true
);

CREATE TABLE menus_burgers (
    id_menu INT NOT NULL REFERENCES menus(id_menu) ON DELETE CASCADE,
    id_burger INT NOT NULL REFERENCES burgers(id_burger) ON DELETE CASCADE,
    PRIMARY KEY (id_menu, id_burger)
);

CREATE TABLE commandes (
    id_commande SERIAL PRIMARY KEY,
    id_client INT NOT NULL REFERENCES clients(id_client) ON DELETE CASCADE,
    date_commande TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    total DECIMAL(10,2) NOT NULL,
    mode_consommation VARCHAR(20) CHECK (mode_consommation IN ('SUR_PLACE','EMPORTE','LIVRAISON')),
    adresse_livraison VARCHAR(255),
    id_zone INT REFERENCES zones(id_zone),
    etat_commande VARCHAR(20) CHECK (etat_commande IN ('EN_COURS','VALIDEE','PREPAREE','TERMINEE','ANNULEE')) DEFAULT 'EN_COURS'
);

CREATE TABLE lignes_commande (
    id_ligne SERIAL PRIMARY KEY,
    id_commande INT NOT NULL REFERENCES commandes(id_commande) ON DELETE CASCADE,
    type_produit VARCHAR(20) CHECK (type_produit IN ('BURGER','MENU','COMPLEMENT')),
    id_produit INT NOT NULL,
    quantite INT NOT NULL,
    sous_total DECIMAL(10,2) NOT NULL
);

CREATE TABLE paiements (
    id_paiement SERIAL PRIMARY KEY,
    id_commande INT UNIQUE NOT NULL REFERENCES commandes(id_commande) ON DELETE CASCADE,
    date_paiement TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    montant DECIMAL(10,2) NOT NULL,
    mode_paiement VARCHAR(20) CHECK (mode_paiement IN ('WAVE','OM'))
);

ALTER TABLE commandes
    ADD CONSTRAINT livraison_zone CHECK (
        (mode_consommation <> 'LIVRAISON') OR (id_zone IS NOT NULL)
    );

INSERT INTO zones (nom, prix_livraison) VALUES
('Dakar Plateau', 1000),
('Parcelles Assainies', 800),
('Pikine', 700),
('Guédiawaye', 700),
('Medina', 900),
('Fann Hock', 900);

INSERT INTO quartiers (nom, id_zone) VALUES
('Plateau', 1),
('Sandaga', 1),
('Colobane', 2),
('Unité 15', 3),
('Unité 26', 3),
('Guinaw Rails', 4),
('Sam Notaire', 4),
('Golf Sud', 4),
('Fann Résidence', 6);

INSERT INTO burgers (nom, prix, image, actif) VALUES
('Burger Dibi', 3700, 'https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=400', true),
('Cheese Burger', 3500, 'https://images.unsplash.com/photo-1572802419224-296b0aeee0d9?w=400', true),
('Double Cheese Burger', 4200, 'https://images.unsplash.com/photo-1553979459-d2229ba7433b?w=400', true),
('Burger Poulet', 3200, 'https://images.unsplash.com/photo-1606755962773-d324e0a13086?w=400', true),
('Burger Simple', 3000, 'https://images.unsplash.com/photo-1550547660-d9450f859349?w=400', true),
('Croque Monsieur Burger', 3800, 'https://images.unsplash.com/photo-1594212699903-ec8a3eca50f5?w=400', true);

INSERT INTO complements (nom, prix, image, actif) VALUES
('Frites classiques', 800, 'frites_classiques.jpg', true),
('Frites alloco', 900, 'frites_alloco.jpg', true),
('Jus Bissap', 700, 'jus_bissap.jpg', true),
('Jus Bouye', 700, 'jus_bouye.jpg', true),
('Soda 33cl', 600, 'soda_33cl.jpg', true),
('Café Touba', 500, 'cafe_touba.jpg', true);

INSERT INTO menus (nom, image, actif) VALUES
('Menu Dakar Samba', 'https://images.unsplash.com/photo-1585238342024-78d387f4a707?w=400', true),
('Menu Sénégal', 'https://images.unsplash.com/photo-1571091718767-18b5b1457add?w=400', true),
('Menu Brasil Burger', 'https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=400', true),
('Menu Teranga', 'https://images.unsplash.com/photo-1586190848861-99aa4a171e90?w=400', true);

INSERT INTO menus_burgers (id_menu, id_burger) VALUES
(1, 1),
(1, 4),
(2, 2),
(2, 3),
(3, 5),
(3, 6),
(4, 1),
(4, 2);

INSERT INTO clients (nom, prenom, telephone, email, password) VALUES
('Ndiaye', 'Fatou', '770000001', 'fatou.ndiaye@gmail.com', 'fatou123'),
('Diop', 'Moussa', '770000002', 'moussa.diop@gmail.com', 'moussa123'),
('Sarr', 'Awa', '770000003', 'awa.sarr@gmail.com', 'awa123'),
('Fall', 'Cheikh', '770000004', 'cheikh.fall@gmail.com', 'cheikh123'),
('Ba', 'Khady', '770000005', 'khady.ba@gmail.com', 'khady123');
