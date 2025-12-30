using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BrasilBurger.Web.Models.Enums;

namespace BrasilBurger.Web.Models.Entities
{
    [Table("lignes_commande")]
    public class LigneCommande
    {
        [Key]
        [Column("id_ligne")]
        public int IdLigne { get; set; }

        [Required]
        [Column("quantite")]
        public int Quantite { get; set; }

        [Column("id_commande")]
        public int IdCommande { get; set; }

        [Column("type_produit")]
        public TypeProduit TypeProduit { get; set; }

        [Column("id_produit")]
        public int IdProduit { get; set; }

        [Column("sous_total", TypeName = "decimal(10,2)")]
        public decimal SousTotal { get; set; }
    }
}
