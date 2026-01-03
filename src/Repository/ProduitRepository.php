<?php

namespace App\Repository;

use App\Entity\Produit;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

class ProduitRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Produit::class);
    }

    public function findActiveProducts(): array
    {
        try {
            return $this->createQueryBuilder('p')
                ->andWhere('p.actif = :actif')
                ->setParameter('actif', true)
                ->orderBy('p.nom', 'ASC')
                ->getQuery()
                ->getResult();
        } catch (\Doctrine\DBAL\Exception $e) {
            return [];
        }
    }

    public function findByType(string $type): array
    {
        try {
            return $this->createQueryBuilder('p')
                ->andWhere('p.type = :type')
                ->andWhere('p.actif = :actif')
                ->setParameter('type', $type)
                ->setParameter('actif', true)
                ->orderBy('p.nom', 'ASC')
                ->getQuery()
                ->getResult();
        } catch (\Doctrine\DBAL\Exception $e) {
            return [];
        }
    }

    public function getTopSellingProducts(int $limit = 5, ?\DateTime $date = null): array
    {
        try {
            $conn = $this->getEntityManager()->getConnection();
            
            if ($date) {
                $dateStart = $date->format('Y-m-d 00:00:00');
                $dateEnd = $date->format('Y-m-d 23:59:59');
                $sql = "SELECT b.id_burger, b.nom, b.prix, b.image, SUM(l.quantite) as total_vendu 
                        FROM burgers b 
                        LEFT JOIN lignes_commande l ON b.id_burger = l.id_produit AND l.type_produit = 'BURGER'
                        LEFT JOIN commandes c ON l.id_commande = c.id_commande
                        WHERE c.etat_commande = 'TERMINEE' AND c.date_commande BETWEEN :dateStart AND :dateEnd
                        GROUP BY b.id_burger, b.nom, b.prix, b.image
                        ORDER BY total_vendu DESC
                        LIMIT :limit";
                $stmt = $conn->prepare($sql);
                $stmt->bindValue('dateStart', $dateStart);
                $stmt->bindValue('dateEnd', $dateEnd);
            } else {
                $sql = "SELECT b.id_burger, b.nom, b.prix, b.image, SUM(l.quantite) as total_vendu 
                        FROM burgers b 
                        LEFT JOIN lignes_commande l ON b.id_burger = l.id_produit AND l.type_produit = 'BURGER'
                        LEFT JOIN commandes c ON l.id_commande = c.id_commande
                        WHERE c.etat_commande = 'TERMINEE'
                        GROUP BY b.id_burger, b.nom, b.prix, b.image
                        ORDER BY total_vendu DESC
                        LIMIT :limit";
                $stmt = $conn->prepare($sql);
            }
            
            $stmt->bindValue('limit', $limit);
            return $stmt->executeQuery()->fetchAllAssociative();
        } catch (\Exception $e) {
            return [];
        }
    }
}