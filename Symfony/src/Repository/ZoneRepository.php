<?php

namespace App\Repository;

use App\Entity\Zone;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

class ZoneRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Zone::class);
    }

    public function findActiveZones(): array
    {
        // The `actif` column may not exist in the database for `zones`.
        // Return all zones ordered by name to avoid runtime exceptions.
        return $this->createQueryBuilder('z')
            ->orderBy('z.nom', 'ASC')
            ->getQuery()
            ->getResult();
    }
}