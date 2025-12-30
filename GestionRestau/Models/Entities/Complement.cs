using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrasilBurger.Web.Models.Entities
{
    [Table("complements")]
    public class Complement
    {
        [Key]
        [Column("id_complement")]
        public int IdComplement { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nom")]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [Column("prix", TypeName = "decimal(10,2)")]
        public decimal Prix { get; set; }

        [MaxLength(255)]
        [Column("image")]
        public string? Image { get; set; }

        [Column("actif")]
        public bool Actif { get; set; } = true;
    }
}
