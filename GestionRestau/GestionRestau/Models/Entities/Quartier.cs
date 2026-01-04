using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrasilBurger.Web.Models.Entities
{
    [Table("quartiers")]
    public class Quartier
    {
        [Key]
        [Column("id_quartier")]
        public int IdQuartier { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nom")]
        public string Nom { get; set; } = string.Empty;

        [Column("id_zone")]
        public int IdZone { get; set; }

        // Navigation property
        [ForeignKey("IdZone")]
        public virtual Zone Zone { get; set; } = null!;
    }
}
