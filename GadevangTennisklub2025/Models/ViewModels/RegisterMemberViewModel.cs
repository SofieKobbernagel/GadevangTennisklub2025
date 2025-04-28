using System.ComponentModel.DataAnnotations;

namespace GadevangTennisklub2025.Models.ViewModels
{
    public class RegisterMemberViewModel
    {
        public Member Member { get; set; }

        [Required(ErrorMessage = "Du skal acceptere betingelserne")]
        public bool AcceptTerms { get; set; }
    }
}
