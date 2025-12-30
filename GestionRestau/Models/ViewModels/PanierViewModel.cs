using BrasilBurger.Web.Models.Entities;
using BrasilBurger.Web.Models.Enums;
using System.Collections.Generic;

namespace BrasilBurger.Web.Models.ViewModels
{
    public class PanierViewModel
    {
        public Commande? Commande { get; set; }
        public List<LigneCommandeViewModel> Lignes { get; set; } = new();
        public decimal SousTotal { get; set; }
        public decimal FraisLivraison { get; set; }
        public decimal Total { get; set; }
        public List<Zone> ZonesDisponibles { get; set; } = new();
    }

    public class LigneCommandeViewModel
    {
        public int IdLigne { get; set; }
        public string NomProduit { get; set; } = string.Empty;
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
        public decimal SousTotal { get; set; }
        public TypeProduit TypeProduit { get; set; }
        public string? ImageUrl { get; set; }
    }
}
