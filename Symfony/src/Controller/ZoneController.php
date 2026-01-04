<?php

namespace App\Controller;

use App\Entity\Zone;
use App\Repository\ZoneRepository;
use Doctrine\ORM\EntityManagerInterface;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Routing\Attribute\Route;

#[Route('/zones')]
class ZoneController extends AbstractController
{
    #[Route('/', name: 'app_zone_index', methods: ['GET'])]
    public function index(ZoneRepository $zoneRepository): Response
    {
        return $this->render('zone/index.html.twig', [
            'zones' => $zoneRepository->findAll(),
        ]);
    }

    #[Route('/nouvelle', name: 'app_zone_new', methods: ['GET', 'POST'])]
    public function new(Request $request, EntityManagerInterface $entityManager): Response
    {
        $zone = new Zone();

        if ($request->isMethod('POST')) {
            $zone->setNom($request->request->get('nom'));
            $zone->setPrixLivraison($request->request->get('prix_livraison'));
            
            // Traiter les quartiers
            $quartiers = $request->request->get('quartiers', '');
            $quartiersArray = array_filter(array_map('trim', explode(',', $quartiers)));
            $zone->setQuartiersArray($quartiersArray);
            
            $zone->setActif($request->request->get('actif', false));

            $entityManager->persist($zone);
            $entityManager->flush();

            $this->addFlash('success', 'Zone créée avec succès !');
            return $this->redirectToRoute('app_zone_index');
        }

        return $this->render('zone/new.html.twig', [
            'zone' => $zone,
        ]);
    }

    #[Route('/{id}', name: 'app_zone_show', methods: ['GET'])]
    public function show(Zone $zone): Response
    {
        return $this->render('zone/show.html.twig', [
            'zone' => $zone,
        ]);
    }

    #[Route('/{id}/modifier', name: 'app_zone_edit', methods: ['GET', 'POST'])]
    public function edit(Request $request, Zone $zone, EntityManagerInterface $entityManager): Response
    {
        if ($request->isMethod('POST')) {
            $zone->setNom($request->request->get('nom'));
            $zone->setPrixLivraison($request->request->get('prix_livraison'));
            
            // Traiter les quartiers
            $quartiers = $request->request->get('quartiers', '');
            $quartiersArray = array_filter(array_map('trim', explode(',', $quartiers)));
            $zone->setQuartiersArray($quartiersArray);
            
            $zone->setActif($request->request->get('actif', false));

            $entityManager->flush();

            $this->addFlash('success', 'Zone modifiée avec succès !');
            return $this->redirectToRoute('app_zone_index');
        }

        return $this->render('zone/edit.html.twig', [
            'zone' => $zone,
        ]);
    }

    #[Route('/{id}/supprimer', name: 'app_zone_delete', methods: ['POST'])]
    public function delete(Request $request, Zone $zone, EntityManagerInterface $entityManager): Response
    {
        if ($this->isCsrfTokenValid('delete'.$zone->getId(), $request->request->get('_token'))) {
            $entityManager->remove($zone);
            $entityManager->flush();
            $this->addFlash('success', 'Zone supprimée avec succès !');
        }

        return $this->redirectToRoute('app_zone_index');
    }
}