<?php

namespace App\Entity;

use App\Repository\CommandeRepository;
use Doctrine\Common\Collections\ArrayCollection;
use Doctrine\Common\Collections\Collection;
use Doctrine\DBAL\Types\Types;
use Doctrine\ORM\Mapping as ORM;

#[ORM\Entity(repositoryClass: CommandeRepository::class)]
#[ORM\Table(name: 'commandes')]
class Commande
{
    public const ETAT_EN_COURS = 'EN_COURS';
    public const ETAT_VALIDEE = 'VALIDEE';
    public const ETAT_PREPAREE = 'PREPAREE';
    public const ETAT_TERMINEE = 'TERMINEE';
    public const ETAT_ANNULEE = 'ANNULEE';

    public const MODE_SUR_PLACE = 'SUR_PLACE';
    public const MODE_EMPORTER = 'EMPORTER';
    public const MODE_LIVRAISON = 'LIVRAISON';

    public const PAIEMENT_WAVE = 'WAVE';
    public const PAIEMENT_OM = 'OM';
    public const PAIEMENT_ESPECES = 'ESPECES';

    #[ORM\Id]
    #[ORM\GeneratedValue]
    #[ORM\Column(name: 'id_commande')]
    private ?int $id = null;

    #[ORM\Column(name: 'adresse_livraison', type: Types::TEXT, nullable: true)]
    private ?string $adresseLivraison = null;

    #[ORM\Column(name: 'mode_consommation', length: 50)]
    private ?string $modeConsommation = null;

    #[ORM\Column(name: 'etat_commande', length: 50)]
    private ?string $etat = self::ETAT_EN_COURS;

    private ?bool $paye = false;
    private ?string $modePaiement = null;

    #[ORM\Column(name: 'total', type: Types::DECIMAL, precision: 10, scale: 2)]
    private ?string $montantTotal = null;

    #[ORM\Column(name: 'date_commande', type: Types::DATETIME_IMMUTABLE, nullable: true)]
    private ?\DateTimeImmutable $createdAt = null;

    #[ORM\ManyToOne(inversedBy: 'commandes')]
    #[ORM\JoinColumn(name: 'id_zone', referencedColumnName: 'id_zone', nullable: true)]
    private ?Zone $zone = null;

    #[ORM\ManyToOne(inversedBy: 'commandes')]
    #[ORM\JoinColumn(name: 'id_client', referencedColumnName: 'id_client', nullable: true)]
    private ?Client $client = null;

    #[ORM\OneToMany(targetEntity: CommandeItem::class, mappedBy: 'commande', cascade: ['persist', 'remove'])]
    private Collection $commandeItems;

    public function __construct()
    {
        $this->commandeItems = new ArrayCollection();
        $this->createdAt = new \DateTimeImmutable();
    }

    public function getId(): ?int
    {
        return $this->id;
    }

    public function getNumeroCommande(): string
    {
        return $this->id ? (string) $this->id : '';
    }

    public function getAdresseLivraison(): ?string
    {
        return $this->adresseLivraison;
    }

    public function setAdresseLivraison(?string $adresseLivraison): static
    {
        $this->adresseLivraison = $adresseLivraison;
        return $this;
    }

    public function getModeConsommation(): ?string
    {
        return $this->modeConsommation;
    }

    public function setModeConsommation(string $modeConsommation): static
    {
        $this->modeConsommation = $modeConsommation;
        return $this;
    }

    public function getEtat(): ?string
    {
        return $this->etat;
    }

    public function setEtat(string $etat): static
    {
        $this->etat = $etat;
        return $this;
    }

    public function getPaye(): bool
    {
        return false;
    }

    public function getModePaiement(): ?string
    {
        return null;
    }

    public function getMontantTotal(): ?string
    {
        return $this->montantTotal;
    }

    public function setMontantTotal(string $montantTotal): static
    {
        $this->montantTotal = $montantTotal;
        return $this;
    }

    public function getCreatedAt(): ?\DateTimeImmutable
    {
        return $this->createdAt;
    }

    public function setCreatedAt(\DateTimeImmutable $createdAt): static
    {
        $this->createdAt = $createdAt;
        return $this;
    }

    public function getUpdatedAt(): ?\DateTimeImmutable
    {
        return null;
    }

    public function getZone(): ?Zone
    {
        return $this->zone;
    }

    public function setZone(?Zone $zone): static
    {
        $this->zone = $zone;
        return $this;
    }

    public function getClient(): ?Client
    {
        return $this->client;
    }

    public function setClient(?Client $client): static
    {
        $this->client = $client;
        return $this;
    }

    public function getCommandeItems(): Collection
    {
        return $this->commandeItems;
    }

    public function addCommandeItem(CommandeItem $commandeItem): static
    {
        if (!$this->commandeItems->contains($commandeItem)) {
            $this->commandeItems->add($commandeItem);
            $commandeItem->setCommande($this);
        }
        return $this;
    }

    public function removeCommandeItem(CommandeItem $commandeItem): static
    {
        if ($this->commandeItems->removeElement($commandeItem)) {
            if ($commandeItem->getCommande() === $this) {
                $commandeItem->setCommande(null);
            }
        }
        return $this;
    }

    public function canBeModified(): bool
    {
        return !in_array($this->etat, [self::ETAT_TERMINEE, self::ETAT_ANNULEE]);
    }

    public function getEtatBadgeClass(): string
    {
        return match($this->etat) {
            self::ETAT_EN_COURS => 'bg-warning',
            self::ETAT_VALIDEE => 'bg-info',
            self::ETAT_PREPAREE => 'bg-primary',
            self::ETAT_TERMINEE => 'bg-success',
            self::ETAT_ANNULEE => 'bg-danger',
            default => 'bg-secondary'
        };
    }

    public function getNomClient(): ?string
    {
        return $this->client ? $this->client->getPrenom() . ' ' . $this->client->getNom() : 'Client inconnu';
    }

    public function getTelephoneClient(): ?string
    {
        return $this->client ? $this->client->getTelephone() : null;
    }

    public function __get(string $name)
    {
        $lc = strtolower($name);
        return match ($lc) {
            'numerocommande', 'numero_commande', 'numero' => $this->getNumeroCommande(),
            'nomclient', 'nom_client' => $this->getNomClient(),
            'telephoneclient', 'telephone_client' => $this->getTelephoneClient(),
            'paye' => $this->getPaye(),
            'modepaiement', 'mode_paiement' => $this->getModePaiement(),
            default => null,
        };
    }

    public function __call(string $name, array $arguments)
    {
        $lc = strtolower($name);
        if (str_starts_with($lc, 'get') || str_starts_with($lc, 'is')) {
            $prop = preg_replace('/^(get|is)/', '', $lc);
        } else {
            $prop = $lc;
        }

        return match ($prop) {
            'numerocommande', 'numero_commande', 'numero' => $this->getNumeroCommande(),
            'nomclient', 'nom_client' => $this->getNomClient(),
            'telephoneclient', 'telephone_client' => $this->getTelephoneClient(),
            'paye' => $this->getPaye(),
            'modepaiement', 'mode_paiement' => $this->getModePaiement(),
            default => null,
        };
    }

    public function isPaye(): bool
    {
        return $this->getPaye();
    }
}
