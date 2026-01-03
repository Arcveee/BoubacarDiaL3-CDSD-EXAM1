<?php

namespace App\Controller;

use App\Entity\Produit;
use App\Repository\ProduitRepository;
use Doctrine\ORM\EntityManagerInterface;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Routing\Attribute\Route;

#[Route('/produits')]
class ProduitController extends AbstractController
{
    #[Route('/', name: 'app_produit_index', methods: ['GET'])]
    public function index(Request $request, EntityManagerInterface $em): Response
    {
        $type = $request->query->get('type');
        $conn = $em->getConnection();
        
        if ($type === 'BURGER') {
            $sql = "SELECT id_burger as id, nom, prix, image, actif, 'BURGER' as type FROM burgers WHERE actif = true ORDER BY nom ASC";
        } elseif ($type === 'MENU') {
            $sql = "SELECT m.id_menu as id, m.nom, SUM(b.prix) as prix, m.image, m.actif, 'MENU' as type 
                    FROM menus m 
                    LEFT JOIN menus_burgers mb ON m.id_menu = mb.id_menu 
                    LEFT JOIN burgers b ON mb.id_burger = b.id_burger 
                    WHERE m.actif = true 
                    GROUP BY m.id_menu, m.nom, m.image, m.actif 
                    ORDER BY m.nom ASC";
        } elseif ($type === 'COMPLEMENT') {
            $sql = "SELECT id_complement as id, nom, prix, image, actif, 'COMPLEMENT' as type FROM complements WHERE actif = true ORDER BY nom ASC";
        } else {
            $sql = "SELECT id_burger as id, nom, prix, image, actif, 'BURGER' as type FROM burgers WHERE actif = true 
                    UNION ALL 
                    SELECT m.id_menu as id, m.nom, SUM(b.prix) as prix, m.image, m.actif, 'MENU' as type 
                    FROM menus m 
                    LEFT JOIN menus_burgers mb ON m.id_menu = mb.id_menu 
                    LEFT JOIN burgers b ON mb.id_burger = b.id_burger 
                    WHERE m.actif = true 
                    GROUP BY m.id_menu, m.nom, m.image, m.actif 
                    UNION ALL 
                    SELECT id_complement as id, nom, prix, image, actif, 'COMPLEMENT' as type FROM complements WHERE actif = true 
                    ORDER BY nom ASC";
        }
        
        $produits = $conn->executeQuery($sql)->fetchAllAssociative();

        return $this->render('produit/index.html.twig', [
            'produits' => $produits,
            'current_type' => $type,
        ]);
    }

    #[Route('/nouveau', name: 'app_produit_new', methods: ['GET', 'POST'])]
    public function new(Request $request, EntityManagerInterface $entityManager): Response
    {
        $produit = new Produit();

        if ($request->isMethod('POST')) {
            $produit->setNom($request->request->get('nom'));
            $produit->setDescription($request->request->get('description'));
            $produit->setPrix($request->request->get('prix'));
            $produit->setType($request->request->get('type'));
            $produit->setImage($request->request->get('image'));
            $produit->setActif((bool)$request->request->get('actif'));

            $entityManager->persist($produit);
            $entityManager->flush();

            $this->addFlash('success', 'Produit créé avec succès !');
            return $this->redirectToRoute('app_produit_index');
        }

        return $this->render('produit/new.html.twig', [
            'produit' => $produit,
        ]);
    }

    #[Route('/{id}', name: 'app_produit_show', methods: ['GET'])]
    public function show(Produit $produit): Response
    {
        return $this->render('produit/show.html.twig', [
            'produit' => $produit,
        ]);
    }

    #[Route('/{id}/modifier', name: 'app_produit_edit', methods: ['GET', 'POST'])]
    public function edit(Request $request, Produit $produit, EntityManagerInterface $entityManager): Response
    {
        if ($request->isMethod('POST')) {
            $produit->setNom($request->request->get('nom'));
            $produit->setDescription($request->request->get('description'));
            $produit->setPrix($request->request->get('prix'));
            $produit->setType($request->request->get('type'));
            $produit->setImage($request->request->get('image'));
            $produit->setActif((bool)$request->request->get('actif'));

            $entityManager->flush();

            $this->addFlash('success', 'Produit modifié avec succès !');
            return $this->redirectToRoute('app_produit_index');
        }

        return $this->render('produit/edit.html.twig', [
            'produit' => $produit,
        ]);
    }

    #[Route('/{id}/archiver', name: 'app_produit_archive', methods: ['POST'])]
    public function archive(Produit $produit, EntityManagerInterface $entityManager): Response
    {
        $produit->setActif(false);
        $entityManager->flush();

        $this->addFlash('success', 'Produit archivé avec succès !');
        return $this->redirectToRoute('app_produit_index');
    }

    #[Route('/{id}/supprimer', name: 'app_produit_delete', methods: ['POST'])]
    public function delete(Request $request, Produit $produit, EntityManagerInterface $entityManager): Response
    {
        if ($this->isCsrfTokenValid('delete'.$produit->getId(), $request->request->get('_token'))) {
            $entityManager->remove($produit);
            $entityManager->flush();
            $this->addFlash('success', 'Produit supprimé avec succès !');
        }

        return $this->redirectToRoute('app_produit_index');
    }
}