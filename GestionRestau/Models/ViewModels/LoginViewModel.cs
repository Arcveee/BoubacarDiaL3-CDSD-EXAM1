using System.ComponentModel.DataAnnotations;

namespace BrasilBurger.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Le login est requis")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
