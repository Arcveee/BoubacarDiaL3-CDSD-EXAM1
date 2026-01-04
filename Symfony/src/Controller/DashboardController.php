<?php

namespace App\Controller;

use App\Repository\CommandeRepository;
use App\Repository\ProduitRepository;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Routing\Attribute\Route;

class DashboardController extends AbstractController
{
    #[Route('/', name: 'app_dashboard')]
    public function index(CommandeRepository $commandeRepo, ProduitRepository $produitRepo, \Symfony\Component\HttpFoundation\Request $request): Response
    {
        $dateParam = $request->query->get('date');
        $selectedDate = $dateParam ? new \DateTime($dateParam) : new \DateTime();
        
        $dailyStats = $commandeRepo->getDailyStats($selectedDate);
        
        $stats = [
            'EN_COURS' => 0,
            'VALIDEE' => 0,
            'PREPAREE' => 0,
            'TERMINEE' => 0,
            'ANNULEE' => 0,
            'recette_journaliere' => 0
        ];

        foreach ($dailyStats as $stat) {
            $stats[$stat['etat']] = $stat['count'];
            if ($stat['etat'] === 'TERMINEE') {
                $stats['recette_journaliere'] = $stat['total'] ?? 0;
            }
        }

        $topProduits = $produitRepo->getTopSellingProducts(5, $selectedDate);

        return $this->render('dashboard/index.html.twig', [
            'stats' => $stats,
            'top_produits' => $topProduits,
            'selected_date' => $selectedDate->format('Y-m-d'),
        ]);
    }
}