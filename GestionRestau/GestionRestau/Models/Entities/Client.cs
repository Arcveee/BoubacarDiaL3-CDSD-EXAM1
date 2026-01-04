using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrasilBurger.Web.Models.Entities
{
    [Table("clients")]
    public class Client
    {
        [Key]
        [Column("id_client")]
        public int IdClient { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("nom")]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("prenom")]
        public string Prenom { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column("telephone")]
        public string Telephone { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [NotMapped]
        public DateTime DateInscription { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ICollection<Commande> Commandes { get; set; } = new List<Commande>();
    }
}
