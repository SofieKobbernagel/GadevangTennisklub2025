using System.ComponentModel.DataAnnotations;

namespace GadevangTennisklub2025.Models.ViewModels
{
    public class RegisterMemberViewModel
    {
        /// <summary>
        /// ViewModel til registrering af nye medlemmer i klubben.
        /// Indeholder et medlem og et felt til accept af betingelser,
        /// som kræves for registrering, men ikke gemmes i databasen.
        /// </summary>
        public Member Member { get; set; }

        [Required(ErrorMessage = "Du skal acceptere betingelserne")]
        public bool AcceptTerms { get; set; }
    }
}
