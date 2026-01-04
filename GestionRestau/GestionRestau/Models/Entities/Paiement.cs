using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BrasilBurger.Web.Models.Enums;

namespace BrasilBurger.Web.Models.Entities
{
    [Table("paiements")]
    public class Paiement
    {
        [Key]
        [Column("id_paiement")]
        public int IdPaiement { get; set; }

        [Column("date_paiement")]
        public DateTime DatePaiement { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("montant", TypeName = "decimal(10,2)")]
        public decimal Montant { get; set; }

        [Column("mode_paiement")]
        public ModePaiement ModePaiement { get; set; }

        [Column("id_commande")]
        public int IdCommande { get; set; }

        // Navigation property
        [ForeignKey("IdCommande")]
        public virtual Commande Commande { get; set; } = null!;
    }
}
