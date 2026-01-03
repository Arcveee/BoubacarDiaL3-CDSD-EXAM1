<?php

namespace App\Entity;

use App\Repository\CommandeItemRepository;
use Doctrine\DBAL\Types\Types;
use Doctrine\ORM\Mapping as ORM;

#[ORM\Entity(repositoryClass: CommandeItemRepository::class)]
#[ORM\Table(name: 'lignes_commande')]
class CommandeItem
{
    #[ORM\Id]
    #[ORM\GeneratedValue]
    #[ORM\Column(name: 'id_ligne')]
    private ?int $id = null;

    #[ORM\ManyToOne(inversedBy: 'commandeItems')]
    #[ORM\JoinColumn(name: 'id_commande', referencedColumnName: 'id_commande', nullable: false)]
    private ?Commande $commande = null;

    #[ORM\Column(name: 'type_produit', length: 50)]
    private ?string $typeProduit = null;

    #[ORM\Column(name: 'id_produit')]
    private ?int $idProduit = null;

    #[ORM\Column(name: 'quantite')]
    private ?int $quantite = null;

    #[ORM\Column(name: 'sous_total', type: Types::DECIMAL, precision: 10, scale: 2)]
    private ?string $sousTotal = null;

    public function getId(): ?int
    {
        return $this->id;
    }

    public function getCommande(): ?Commande
    {
        return $this->commande;
    }

    public function setCommande(?Commande $commande): static
    {
        $this->commande = $commande;
        return $this;
    }

    public function getTypeProduit(): ?string
    {
        return $this->typeProduit;
    }

    public function setTypeProduit(string $typeProduit): static
    {
        $this->typeProduit = $typeProduit;
        return $this;
    }

    public function getIdProduit(): ?int
    {
        return $this->idProduit;
    }

    public function setIdProduit(int $idProduit): static
    {
        $this->idProduit = $idProduit;
        return $this;
    }

    public function getProduit()
    {
        return null;
    }

    public function getQuantite(): ?int
    {
        return $this->quantite;
    }

    public function setQuantite(int $quantite): static
    {
        $this->quantite = $quantite;
        $this->calculateSousTotal();
        return $this;
    }

    public function getPrixUnitaire(): ?string
    {
        if ($this->quantite && $this->sousTotal) {
            return (string) ((float)$this->sousTotal / $this->quantite);
        }
        return '0';
    }

    public function getSousTotal(): ?string
    {
        return $this->sousTotal;
    }

    public function setSousTotal(string $sousTotal): static
    {
        $this->sousTotal = $sousTotal;
        return $this;
    }
}
