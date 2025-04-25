using System.ComponentModel.DataAnnotations;

namespace GadevangTennisklub2025.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Brugernavn er påkrævet")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Kodeord er påkrævet")]
        public string Password { get; set; }
    }
}
