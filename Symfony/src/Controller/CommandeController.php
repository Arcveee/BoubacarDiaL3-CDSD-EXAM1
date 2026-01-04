<?php

namespace App\Controller;

use App\Entity\Commande;
use App\Repository\CommandeRepository;
use App\Repository\ZoneRepository;
use Doctrine\ORM\EntityManagerInterface;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Routing\Attribute\Route;

#[Route('/commandes')]
class CommandeController extends AbstractController
{
    #[Route('/', name: 'app_commande_index', methods: ['GET'])]
    public function index(CommandeRepository $commandeRepository, Request $request, \App\Repository\ZoneRepository $zoneRepository): Response
    {
        $etat = $request->query->get('etat');
        $zone = $request->query->get('zone');
        $client = $request->query->get('client');
        $page = max(1, (int)$request->query->get('page', 1));
        $pageSize = 20;

        $queryBuilder = $commandeRepository->createQueryBuilder('c')
            ->leftJoin('c.zone', 'z')
            ->leftJoin('c.client', 'cl')
            ->orderBy('c.createdAt', 'DESC');

        if ($etat) {
            $queryBuilder->andWhere('c.etat = :etat')
                ->setParameter('etat', $etat);
        }

        if ($zone) {
            $queryBuilder->andWhere('z.id = :zone')
                ->setParameter('zone', $zone);
        }

        if ($client) {
            $queryBuilder->andWhere('cl.prenom LIKE :client OR cl.nom LIKE :client')
                ->setParameter('client', '%' . $client . '%');
        }

        // Count total results (clone query builder to avoid modifying original)
        $countQb = clone $queryBuilder;
        $total = (int) $countQb->select('COUNT(c.id)')->resetDQLPart('orderBy')->getQuery()->getSingleScalarResult();

        $commandes = $queryBuilder
            ->setFirstResult(($page - 1) * $pageSize)
            ->setMaxResults($pageSize)
            ->getQuery()
            ->getResult();

        $totalPages = (int) ceil($total / $pageSize);

        $zones = $zoneRepository->findAll();

        return $this->render('commande/index.html.twig', [
            'commandes' => $commandes,
            'zones' => $zones,
            'filters' => [
                'etat' => $etat,
                'zone' => $zone,
                'client' => $client,
            ],
            'pagination' => [
                'page' => $page,
                'pageSize' => $pageSize,
                'total' => $total,
                'totalPages' => $totalPages,
            ],
        ]);
    }

    #[Route('/{id}', name: 'app_commande_show', methods: ['GET'])]
    public function show(Commande $commande, \App\Service\CommandeService $commandeService): Response
    {
        return $this->render('commande/show.html.twig', [
            'commande' => $commande,
            'paye' => $commandeService->isPaye($commande),
            'mode_paiement' => $commandeService->getModePaiement($commande),
        ]);
    }

    #[Route('/{id}/changer-etat', name: 'app_commande_change_etat', methods: ['POST'])]
    public function changeEtat(Request $request, Commande $commande, EntityManagerInterface $entityManager): Response
    {
        $nouvelEtat = $request->request->get('etat');
        
        if (!$commande->canBeModified()) {
            $this->addFlash('error', 'Cette commande ne peut plus être modifiée.');
            return $this->redirectToRoute('app_commande_show', ['id' => $commande->getId()]);
        }

        $etatsValides = [
            Commande::ETAT_EN_COURS,
            Commande::ETAT_VALIDEE,
            Commande::ETAT_PREPAREE,
            Commande::ETAT_TERMINEE,
        ];

        if (in_array($nouvelEtat, $etatsValides)) {
            $commande->setEtat($nouvelEtat);
            $entityManager->flush();
            
            $this->addFlash('success', 'État de la commande mis à jour avec succès !');
        } else {
            $this->addFlash('error', 'État invalide.');
        }

        return $this->redirectToRoute('app_commande_show', ['id' => $commande->getId()]);
    }

    #[Route('/{id}/annuler', name: 'app_commande_cancel', methods: ['POST'])]
    public function cancel(Commande $commande, EntityManagerInterface $entityManager): Response
    {
        if (!$commande->canBeModified()) {
            $this->addFlash('error', 'Cette commande ne peut plus être annulée.');
            return $this->redirectToRoute('app_commande_show', ['id' => $commande->getId()]);
        }

        $commande->setEtat(Commande::ETAT_ANNULEE);
        $entityManager->flush();

        $this->addFlash('success', 'Commande annulée avec succès !');
        return $this->redirectToRoute('app_commande_index');
    }
}