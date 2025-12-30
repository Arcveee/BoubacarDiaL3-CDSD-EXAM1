using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BrasilBurger.Web.Models.ViewModels
{
    public class ZoneViewModel
    {
        public int? IdZone { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(100)]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prix de livraison est requis")]
        [Range(0, 999999.99, ErrorMessage = "Le prix doit être positif")]
        public decimal PrixLivraison { get; set; }

        public List<string> Quartiers { get; set; } = new();

        [StringLength(100)]
        public string? NouveauQuartier { get; set; }
    }
}
