using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BrasilBurger.Web.Models.Enums;

namespace BrasilBurger.Web.Models.Entities
{
    [Table("commandes")]
    public class Commande
    {
        [Key]
        [Column("id_commande")]
        public int IdCommande { get; set; }

        [Column("date_commande")]
        public DateTime DateCommande { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("total", TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        [MaxLength(255)]
        [Column("adresse_livraison")]
        public string? AdresseLivraison { get; set; }

        [Column("id_client")]
        public int IdClient { get; set; }

        [Column("mode_consommation")]
        public ModeConsommation ModeConsommation { get; set; }

        [Column("etat_commande")]
        public EtatCommande EtatCommande { get; set; } = EtatCommande.EN_COURS;

        [Column("id_zone")]
        public int? IdZone { get; set; }

        // Navigation properties
        [ForeignKey("IdClient")]
        public virtual Client Client { get; set; } = null!;

        [ForeignKey("IdZone")]
        public virtual Zone? Zone { get; set; }

        public virtual ICollection<LigneCommande> LignesCommande { get; set; } = new List<LigneCommande>();
        public virtual Paiement? Paiement { get; set; }
    }
}
