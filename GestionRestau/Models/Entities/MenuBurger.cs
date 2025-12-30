using System.ComponentModel.DataAnnotations.Schema;

namespace BrasilBurger.Web.Models.Entities
{
    [Table("menus_burgers")]
    public class MenuBurger
    {
        [Column("id_menu")]
        public int IdMenu { get; set; }

        [Column("id_burger")]
        public int IdBurger { get; set; }

        // Navigation properties
        [ForeignKey("IdMenu")]
        public virtual Menu Menu { get; set; } = null!;

        [ForeignKey("IdBurger")]
        public virtual Burger Burger { get; set; } = null!;
    }
}
