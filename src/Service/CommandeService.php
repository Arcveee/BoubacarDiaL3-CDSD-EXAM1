<?php

namespace App\Service;

use App\Entity\Commande;
use Doctrine\DBAL\Connection;

class CommandeService
{
    public function __construct(private Connection $connection)
    {
    }

    public function isPaye(Commande $commande): bool
    {
        $sql = "SELECT COUNT(*) FROM paiements WHERE id_commande = :id";
        $stmt = $this->connection->prepare($sql);
        $stmt->bindValue('id', $commande->getId());
        $count = $stmt->executeQuery()->fetchOne();
        return $count > 0;
    }

    public function getModePaiement(Commande $commande): ?string
    {
        $sql = "SELECT mode_paiement FROM paiements WHERE id_commande = :id LIMIT 1";
        $stmt = $this->connection->prepare($sql);
        $stmt->bindValue('id', $commande->getId());
        return $stmt->executeQuery()->fetchOne() ?: null;
    }
}
