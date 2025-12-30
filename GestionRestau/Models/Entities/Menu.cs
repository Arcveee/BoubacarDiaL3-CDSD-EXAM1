using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BrasilBurger.Web.Models.Entities
{
    [Table("menus")]
    public class Menu
    {
        [Key]
        [Column("id_menu")]
        public int IdMenu { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nom")]
        public string Nom { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("image")]
        public string? Image { get; set; }

        [Column("actif")]
        public bool Actif { get; set; } = true;

        // Navigation properties
        public virtual ICollection<MenuBurger> MenuBurgers { get; set; } = new List<MenuBurger>();

        // Propriété calculée pour le prix (somme des burgers)
        [NotMapped]
        public decimal Prix => MenuBurgers?.Sum(mb => mb.Burger?.Prix ?? 0) ?? 0;
    }
}
