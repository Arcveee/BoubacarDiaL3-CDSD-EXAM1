<?php

namespace App\Entity;

use App\Repository\ZoneRepository;
use Doctrine\Common\Collections\ArrayCollection;
use Doctrine\Common\Collections\Collection;
use Doctrine\DBAL\Types\Types;
use Doctrine\ORM\Mapping as ORM;

#[ORM\Entity(repositoryClass: ZoneRepository::class)]
#[ORM\Table(name: 'zones')]
class Zone
{
    #[ORM\Id]
    #[ORM\GeneratedValue]
    #[ORM\Column(name: 'id_zone')]
    private ?int $id = null;

    #[ORM\Column(name: 'nom', length: 255)]
    private ?string $nom = null;

    #[ORM\Column(name: 'prix_livraison', type: Types::DECIMAL, precision: 10, scale: 2)]
    private ?string $prixLivraison = null;

    // columns present in DB: id_zone, nom, prix_livraison

    #[ORM\OneToMany(targetEntity: Commande::class, mappedBy: 'zone')]
    private Collection $commandes;

    public function __construct()
    {
        $this->commandes = new ArrayCollection();
    }

    public function getId(): ?int
    {
        return $this->id;
    }

    public function getNom(): ?string
    {
        return $this->nom;
    }

    public function setNom(string $nom): static
    {
        $this->nom = $nom;
        return $this;
    }

    public function getPrixLivraison(): ?string
    {
        return $this->prixLivraison;
    }

    public function setPrixLivraison(string $prixLivraison): static
    {
        $this->prixLivraison = $prixLivraison;
        return $this;
    }

    // quartiers and actif columns removed to match DB schema
    // Provide transient properties to avoid template/controller errors
    private array $quartiersArray = [];
    private ?bool $actif = true;

    public function setQuartiersArray(array $quartiers): static
    {
        $this->quartiersArray = $quartiers;
        return $this;
    }

    public function getQuartiersArray(): array
    {
        return $this->quartiersArray;
    }

    public function setActif(bool $actif): static
    {
        $this->actif = $actif;
        return $this;
    }

    public function isActif(): ?bool
    {
        return $this->actif;
    }

    public function getCommandes(): Collection
    {
        return $this->commandes;
    }

    public function addCommande(Commande $commande): static
    {
        if (!$this->commandes->contains($commande)) {
            $this->commandes->add($commande);
            $commande->setZone($this);
        }
        return $this;
    }

    public function removeCommande(Commande $commande): static
    {
        if ($this->commandes->removeElement($commande)) {
            if ($commande->getZone() === $this) {
                $commande->setZone(null);
            }
        }
        return $this;
    }
}
