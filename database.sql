DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'mode_consommmation_enum') THEN
        CREATE TYPE mode_consommmation_enum AS ENUM ('SUR_PLACE', 'EMPORTER', 'LIVRAISON');
    END IF;
END$$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'etat_commande_enum') THEN
        CREATE TYPE etat_commande_enum AS ENUM ('EN_COURS', 'VALIDEE', 'PREPAREE', 'TERMINEE', 'ANNULEE');
    END IF;
END$$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'mode_paiement_enum') THEN
        CREATE TYPE mode_paiement_enum AS ENUM ('WAVE', 'OM');
    END IF;
END$$;

CREATE TABLE IF NOT EXISTS clients (
    id_client SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    prenom VARCHAR(100) NOT NULL,
    telephone VARCHAR(30) NOT NULL UNIQUE,
    email VARCHAR(150) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS gestionnaires (
    id_gestionnaire SERIAL PRIMARY KEY,
    login VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS livreurs (
    id_livreur SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    prenom VARCHAR(100) NOT NULL,
    telephone VARCHAR(30) NOT NULL UNIQUE,
    actif BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS zones (
    id_zone SERIAL PRIMARY KEY,
    nom VARCHAR(150) NOT NULL,
    prix_livraison NUMERIC(10,2) NOT NULL
);

CREATE TABLE IF NOT EXISTS quartiers (
    id_quartier SERIAL PRIMARY KEY,
    nom VARCHAR(150) NOT NULL,
    id_zone INTEGER NOT NULL REFERENCES zones(id_zone)
);

CREATE TABLE IF NOT EXISTS burgers (
    id_burger SERIAL PRIMARY KEY,
    nom VARCHAR(150) NOT NULL,
    prix NUMERIC(10,2) NOT NULL,
    image VARCHAR(255),
    actif BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS complements (
    id_complement SERIAL PRIMARY KEY,
    nom VARCHAR(150) NOT NULL,
    prix NUMERIC(10,2) NOT NULL,
    image VARCHAR(255),
    actif BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS menus (
    id_menu SERIAL PRIMARY KEY,
    nom VARCHAR(150) NOT NULL,
    image VARCHAR(255),
    actif BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS menus_burgers (
    id_menu INTEGER NOT NULL REFERENCES menus(id_menu) ON DELETE CASCADE,
    id_burger INTEGER NOT NULL REFERENCES burgers(id_burger),
    PRIMARY KEY (id_menu, id_burger)
);

CREATE TABLE IF NOT EXISTS commandes (
    id_commande SERIAL PRIMARY KEY,
    date_commande TIMESTAMP NOT NULL DEFAULT NOW(),
    total NUMERIC(10,2),
    adresse_livraison TEXT,
    mode_consommation mode_consommmation_enum NOT NULL,
    etat_commande etat_commande_enum NOT NULL DEFAULT 'EN_COURS',
    id_client INTEGER NOT NULL REFERENCES clients(id_client),
    id_zone INTEGER REFERENCES zones(id_zone),
    id_livreur INTEGER REFERENCES livreurs(id_livreur),
    CONSTRAINT chk_zone_livraison CHECK (mode_consommation <> 'LIVRAISON' OR id_zone IS NOT NULL)
);

CREATE TABLE IF NOT EXISTS lignes_commandes (
    id_ligne SERIAL PRIMARY KEY,
    id_commande INTEGER NOT NULL REFERENCES commandes(id_commande) ON DELETE CASCADE,
    quantite INTEGER NOT NULL,
    id_burger INTEGER REFERENCES burgers(id_burger),
    id_menu INTEGER REFERENCES menus(id_menu),
    id_complement INTEGER REFERENCES complements(id_complement),
    CONSTRAINT chk_produit_unique CHECK (((id_burger IS NOT NULL)::int + (id_menu IS NOT NULL)::int + (id_complement IS NOT NULL)::int) = 1)
);

CREATE TABLE IF NOT EXISTS paiements (
    id_paiement SERIAL PRIMARY KEY,
    date_paiement TIMESTAMP NOT NULL DEFAULT NOW(),
    montant NUMERIC(10,2) NOT NULL,
    mode_paiement mode_paiement_enum NOT NULL,
    id_commande INTEGER NOT NULL REFERENCES commandes(id_commande) ON DELETE CASCADE,
    CONSTRAINT paiement_unique_par_commande UNIQUE (id_commande)
);

INSERT INTO gestionnaires (login, password) VALUES
('admin', 'admin123')
ON CONFLICT (login) DO NOTHING;

INSERT INTO zones (nom, prix_livraison) VALUES
('Dakar Plateau', 1000),
('Parcelles Assainies', 800),
('Pikine', 700),
('Guediawaye', 700)
ON CONFLICT (nom) DO NOTHING;

INSERT INTO quartiers (nom, id_zone) VALUES
('Plateau', 1),
('Medina', 1),
('Fann Hock', 1),
('Unité 15', 2),
('Unité 26', 2),
('Guinaw Rails', 3),
('Sam Notaire', 4),
('Golf Sud', 4)
ON CONFLICT DO NOTHING;

INSERT INTO burgers (nom, prix, image, actif) VALUES
('Yassa Burger', 3500, 'yassa_burger.jpg', true),
('Thiéboudienne Burger', 3800, 'thieb_burger.jpg', true),
('Mafe Burger', 3600, 'mafe_burger.jpg', true),
('Poulet Braisé Burger', 3200, 'poulet_braise_burger.jpg', true)
ON CONFLICT (nom) DO NOTHING;

INSERT INTO complements (nom, prix, image, actif) VALUES
('Frites classiques', 800, 'frites_classiques.jpg', true),
('Frites alloco', 900, 'frites_alloco.jpg', true),
('Jus Bissap', 700, 'jus_bissap.jpg', true),
('Jus Bouye', 700, 'jus_bouye.jpg', true),
('Soda 33cl', 600, 'soda_33cl.jpg', true)
ON CONFLICT (nom) DO NOTHING;

INSERT INTO menus (nom, image, actif) VALUES
('Menu Dakar', 'menu_dakar.jpg', true),
('Menu Sénégal', 'menu_senegal.jpg', true),
('Menu Brasil Burger', 'menu_brasil_burger.jpg', true)
ON CONFLICT (nom) DO NOTHING;

INSERT INTO menus_burgers (id_menu, id_burger) VALUES
(1, 1),
(1, 4),
(2, 2),
(2, 3),
(3, 1),
(3, 2)
ON CONFLICT (id_menu, id_burger) DO NOTHING;

INSERT INTO clients (nom, prenom, telephone, email, password) VALUES
('Ndiaye', 'Fatou', '770000001', 'fatou.ndiaye@example.com', 'fatou123'),
('Diop', 'Moussa', '770000002', 'moussa.diop@example.com', 'moussa123'),
('Sarr', 'Awa', '770000003', 'awa.sarr@example.com', 'awa123'),
('Fall', 'Cheikh', '770000004', 'cheikh.fall@example.com', 'cheikh123')
ON CONFLICT (telephone) DO NOTHING;

INSERT INTO commandes (date_commande, total, adresse_livraison, mode_consommation, etat_commande, id_client, id_zone) VALUES
(NOW(), NULL, 'Plateau, Dakar', 'SUR_PLACE', 'EN_COURS', 1, NULL),
(NOW(), NULL, 'Parcelles Unité 15', 'LIVRAISON', 'EN_COURS', 2, 2),
(NOW(), NULL, 'Medina Dakar', 'EMPORTER', 'EN_COURS', 3, NULL);

INSERT INTO lignes_commandes (id_commande, quantite, id_burger, id_menu, id_complement) VALUES
(1, 1, 1, NULL, NULL),
(1, 1, NULL, 1, NULL),
(2, 2, NULL, 2, NULL),
(2, 2, NULL, NULL, 3),
(2, 1, NULL, NULL, 2),
(3, 1, 4, NULL, NULL);

UPDATE commandes c
SET total = sub.total_calc
FROM (
    SELECT lc.id_commande,
           SUM(
               CASE
                   WHEN lc.id_burger IS NOT NULL THEN b.prix * lc.quantite
                   WHEN lc.id_menu IS NOT NULL THEN (
                       SELECT COALESCE(SUM(bm.prix),0)
                       FROM menus_burgers mb2
                       JOIN burgers bm ON bm.id_burger = mb2.id_burger
                       WHERE mb2.id_menu = lc.id_menu
                   ) * lc.quantite
                   WHEN lc.id_complement IS NOT NULL THEN co.prix * lc.quantite
                   ELSE 0
               END
           ) + COALESCE(z.prix_livraison,0) AS total_calc
    FROM lignes_commandes lc
    LEFT JOIN burgers b ON b.id_burger = lc.id_burger
    LEFT JOIN menus m ON m.id_menu = lc.id_menu
    LEFT JOIN complements co ON co.id_complement = lc.id_complement
    LEFT JOIN commandes c2 ON c2.id_commande = lc.id_commande
    LEFT JOIN zones z ON z.id_zone = c2.id_zone AND c2.mode_consommation = 'LIVRAISON'
    GROUP BY lc.id_commande, z.prix_livraison
) sub
WHERE c.id_commande = sub.id_commande;

INSERT INTO paiements (date_paiement, montant, mode_paiement, id_commande) VALUES
(NOW(), (SELECT total FROM commandes WHERE id_commande = 1), 'WAVE', 1)
ON CONFLICT (id_commande) DO NOTHING;

INSERT INTO paiements (date_paiement, montant, mode_paiement, id_commande) VALUES
(NOW(), (SELECT total FROM commandes WHERE id_commande = 2), 'OM', 2)
ON CONFLICT (id_commande) DO NOTHING;

-- Synced inserts from Neon: updated menu and seed data
INSERT INTO zones (nom, prix_livraison) VALUES
('Dakar Plateau', 1000),
('Parcelles Assainies', 800),
('Pikine', 700),
('Guediawaye', 700);

INSERT INTO quartiers (nom, id_zone) VALUES
('Plateau', 1),
('Medina', 1),
('Fann Hock', 1),
('Unité 15', 2),
('Unité 26', 2),
('Guinaw Rails', 3),
('Sam Notaire', 4),
('Golf Sud', 4);

INSERT INTO burgers (nom, prix, image, actif) VALUES
('Dibi Burger', 3500, 'yassa_burger.jpg', true),
('Cheese Burger', 3800, 'thieb_burger.jpg', true),
('Double Cheese Burger', 3600, 'mafe_burger.jpg', true),
('Burger Poulet', 3200, 'poulet_braise_burger.jpg', true);

INSERT INTO complements (nom, prix, image, actif) VALUES
('Frites classiques', 800, 'frites_classiques.jpg', true),
('Frites alloco', 900, 'frites_alloco.jpg', true),
('Jus Bissap', 700, 'jus_bissap.jpg', true),
('Jus Bouye', 700, 'jus_bouye.jpg', true),
('Soda 33cl', 600, 'soda_33cl.jpg', true);

INSERT INTO menus (nom, image, actif) VALUES
('Menu Dakar Samba', 'menu_dakar.jpg', true),
('Menu Casamance', 'menu_senegal.jpg', true),
('Menu Brasil Burger', 'menu_brasil_burger.jpg', true);

INSERT INTO menus_burgers (id_menu, id_burger) VALUES
(1, 1),
(1, 4),
(2, 2),
(2, 3),
(3, 1),
(3, 2);

INSERT INTO clients (nom, prenom, telephone, email, password) VALUES
('Ndiaye', 'Fatou', '770000001', 'fatou.ndiaye@gmail.com', 'fatou123'),
('Diop', 'Moussa', '770000002', 'moussa.diop@gmail.com', 'moussa123'),
('Sarr', 'Awa', '770000003', 'awa.sarr@gmail.com', 'awa123'),
('Fall', 'Cheikh', '770000004', 'cheikh.fall@gmail.com', 'cheikh123');


