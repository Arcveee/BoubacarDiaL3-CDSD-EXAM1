<?php

namespace App\Repository;

use App\Entity\Commande;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

class CommandeRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Commande::class);
    }

    public function findByEtat(string $etat): array
    {
        return $this->createQueryBuilder('c')
            ->andWhere('c.etat = :etat')
            ->setParameter('etat', $etat)
            ->orderBy('c.createdAt', 'DESC')
            ->getQuery()
            ->getResult();
    }

    public function getDailyStats(\DateTimeInterface $date): array
    {
        $start = $date->format('Y-m-d 00:00:00');
        $end = $date->format('Y-m-d 23:59:59');

        $conn = $this->getEntityManager()->getConnection();
        $rows = [];
        try {
                        $sql = <<<'SQL'
SELECT
    COALESCE(etat_commande, 'UNKNOWN') AS etat,
    COUNT(id_commande) AS count,
    COALESCE(SUM(montant_total), 0) AS total
FROM commandes
WHERE date_commande BETWEEN :start AND :end
GROUP BY etat_commande
SQL;

                        $stmt = $conn->prepare($sql);
                        $stmt->bindValue('start', $start);
                        $stmt->bindValue('end', $end);
                        $rows = $stmt->executeQuery()->fetchAllAssociative();
                } catch (\Throwable $e) {
                        $sql = <<<'SQL'
SELECT
    COALESCE(etat_commande, 'UNKNOWN') AS etat,
    COUNT(id_commande) AS count,
    COALESCE(SUM(total), 0) AS total
FROM commandes
WHERE date_commande BETWEEN :start AND :end
GROUP BY etat_commande
SQL;

                        $stmt = $conn->prepare($sql);
                        $stmt->bindValue('start', $start);
                        $stmt->bindValue('end', $end);
                        $rows = $stmt->executeQuery()->fetchAllAssociative();
                }

        $out = [];
        foreach ($rows as $r) {
            $out[] = ['etat' => $r['etat'], 'count' => (int) $r['count'], 'total' => (string) $r['total']];
        }

        return $out;
    }

    public function getRecettesPeriode(\DateTimeInterface $debut, \DateTimeInterface $fin): string
    {
        $conn = $this->getEntityManager()->getConnection();

        try {
            $sql = 'SELECT SUM(montant) as total FROM paiements WHERE date_paiement BETWEEN :debut AND :fin';
            $stmt = $conn->prepare($sql);
            $stmt->bindValue('debut', $debut->format('Y-m-d 00:00:00'));
            $stmt->bindValue('fin', $fin->format('Y-m-d 23:59:59'));
            $result = $stmt->executeQuery()->fetchOne();
            if ($result !== null) {
                return (string) $result;
            }
        } catch (\Throwable $e) {
        }

        try {
            $sql2 = 'SELECT SUM(montant_total) as total FROM commandes WHERE date_commande BETWEEN :debut AND :fin';
            $stmt2 = $conn->prepare($sql2);
            $stmt2->bindValue('debut', $debut->format('Y-m-d 00:00:00'));
            $stmt2->bindValue('fin', $fin->format('Y-m-d 23:59:59'));
            $res2 = $stmt2->executeQuery()->fetchOne();
            if ($res2 !== null) {
                return (string) $res2;
            }
        } catch (\Throwable $e) {
        }

        try {
            $sql3 = 'SELECT SUM(total) as total FROM commandes WHERE date_commande BETWEEN :debut AND :fin';
            $stmt3 = $conn->prepare($sql3);
            $stmt3->bindValue('debut', $debut->format('Y-m-d 00:00:00'));
            $stmt3->bindValue('fin', $fin->format('Y-m-d 23:59:59'));
            $res3 = $stmt3->executeQuery()->fetchOne();
            if ($res3 !== null) {
                return (string) $res3;
            }
        } catch (\Throwable $e) {
        }

        return '0';
    }

    public function findCommandesEnRetardIds(\DateTimeInterface $limite, array $etats = ['EN_COURS', 'VALIDEE'], int $limit = 50): array
    {
        $conn = $this->getEntityManager()->getConnection();

        // Build IN placeholders
        $placeholders = [];
        $params = [];
        foreach ($etats as $i => $e) {
            $ph = ':etat' . $i;
            $placeholders[] = $ph;
            $params['etat' . $i] = $e;
        }

        $sql = 'SELECT id_commande FROM commandes WHERE etat_commande IN (' . implode(', ', $placeholders) . ') AND date_commande < :limite ORDER BY date_commande DESC LIMIT ' . (int)$limit;

        $params['limite'] = $limite->format('Y-m-d H:i:s');

        $stmt = $conn->prepare($sql);
        foreach ($params as $k => $v) {
            $stmt->bindValue($k, $v);
        }

        $rows = $stmt->executeQuery()->fetchAllAssociative();
        $ids = array_map(fn($r) => (int)$r['id_commande'], $rows);

        return $ids;
    }
}