using BrasilBurger.Web.Models.Enums;
using System.Collections.Generic;

namespace BrasilBurger.Web.Models.ViewModels
{
    public class CommandeViewModel
    {
        public int IdCommande { get; set; }
        public DateTime DateCommande { get; set; }
        public decimal Total { get; set; }
        public ModeConsommation ModeConsommation { get; set; }
        public EtatCommande EtatCommande { get; set; }
        public string? AdresseLivraison { get; set; }
        public string? NomZone { get; set; }
        public string NomClient { get; set; } = string.Empty;
        public List<LigneCommandeViewModel> Lignes { get; set; } = new();
    }
}
