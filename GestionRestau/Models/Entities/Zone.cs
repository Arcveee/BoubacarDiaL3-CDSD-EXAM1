using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrasilBurger.Web.Models.Entities
{
    [Table("zones")]
    public class Zone
    {
        [Key]
        [Column("id_zone")]
        public int IdZone { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nom")]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [Column("prix_livraison", TypeName = "decimal(10,2)")]
        public decimal PrixLivraison { get; set; }

        // Navigation properties
        public virtual ICollection<Quartier> Quartiers { get; set; } = new List<Quartier>();
        public virtual ICollection<Commande> Commandes { get; set; } = new List<Commande>();
    }
}
