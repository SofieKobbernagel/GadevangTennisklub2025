using GadevangTennisklub2025.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace GadevangTennisklub2025.Models
{
    public class Coach
    {
        [Display(Name = "Tlf")]
        [Required(ErrorMessage = "Du skal angive et telefonnummer")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Telefonnummer skal være præcis 8 cifre og kun tal.")]
        public string Phone { get; set; }


        [Display(Name = "Træner Id")]
        public int Coach_Id { get; set; }

        [Display(Name = "Navn")]
        [Required(ErrorMessage = "Du skal angive dit fulde navn")]
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ\- ]{2,}$", ErrorMessage = "Dit navn skal være mindst 2 tegn og må kun indeholde bogstaver, bindestreg eller mellemrum.")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Der skal gives en email")]
        [RegularExpression(@".+@.+\.(com|dk)", ErrorMessage = "Email skal slutte på .com eller .dk.")]
        public string Email { get; set; }

        [Display(Name = "Løn")]
        public decimal Sallary { get; set; }

        [Display(Name = "Kontrakt")]
        public string ContractFileRoute { get; set; }

        [Display(Name = "Adresse")]
        [Required(ErrorMessage = "Du skal angive din adresse")]
        [RegularExpression(@"^(?=.*[a-zA-ZæøåÆØÅ]{2,})(?=.*\d)(?=.*\s)[a-zA-ZæøåÆØÅ0-9\s\.\,\-]{4,100}$",
        ErrorMessage = "Adressen skal indeholde mindst to bogstaver, et mellemrum og et tal, og må kun indeholde bogstaver, tal, punktum, komma eller bindestreg.")]
        public string Address { get; set; }

        [Display(Name = "Profilbillede")]
        public string? ProfileImagePath { get; set; }

        public Coach()
        {

        }

        public Coach(string name, string phone, string email, int coachId, string address, string? profileImagePath, string contractFileRoute)
        {

            Name = name;
            Phone = phone;
            Email = email;
            Coach_Id = coachId;
            Address = address;
            ProfileImagePath = profileImagePath;
            ContractFileRoute = contractFileRoute;
        }
    }
}



