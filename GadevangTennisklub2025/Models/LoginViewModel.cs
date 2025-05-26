using System.ComponentModel.DataAnnotations;

namespace GadevangTennisklub2025.Models
{
    public class LoginViewModel
    {
        /// <summary>
        /// ViewModel til login af brugere i Gadevang Tennisklub.
        /// Indeholder felter til brugernavn og adgangskode, som anvendes ved login.
        /// </summary>
        [Required(ErrorMessage = "Brugernavn er påkrævet")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Kodeord er påkrævet")]
        public string Password { get; set; }
    }
}
