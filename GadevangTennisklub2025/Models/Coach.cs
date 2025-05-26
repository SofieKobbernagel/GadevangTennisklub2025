using GadevangTennisklub2025.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace GadevangTennisklub2025.Models
{
    // Repræsenterer en træner i Gadevang Tennisklub med kontaktinfo og kontraktdata.
    public class Coach
    {
        [Display(Name = "Tlf")]
        [Required(ErrorMessage = "Du skal angive et telefonnummer")]
        // Skal være præcis 8 tal uden mellemrum eller tegn.
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Telefonnummer skal være præcis 8 cifre og kun tal.")]
        public string Phone { get; set; }


        [Display(Name = "Træner Id")]
        public int Coach_Id { get; set; }

        [Display(Name = "Navn")]
        [Required(ErrorMessage = "Du skal angive dit fulde navn")]
        // Navn skal være mindst 2 tegn og kun bogstaver, bindestreg eller mellemrum – ingen tal eller specialtegn.
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ\- ]{2,}$", ErrorMessage = "Dit navn skal være mindst 2 tegn og må kun indeholde bogstaver, bindestreg eller mellemrum.")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Der skal gives en email")]
        [RegularExpression(@".+@.+\.(com|dk)", ErrorMessage = "Email skal slutte på .com eller .dk.")]
        public string Email { get; set; }

        [Display(Name = "Løn")]
        public decimal Salary { get; set; }

        [Display(Name = "Kontrakt")]
        public string? ContractFilePath { get; set; }

        [Display(Name = "Adresse")]
        [Required(ErrorMessage = "Du skal angive din adresse")]
        // Adressen skal indeholde bogstaver, tal og mellemrum – bruges til at validere typiske danske adresser.
        [RegularExpression(@"^(?=.*[a-zA-ZæøåÆØÅ]{2,})(?=.*\d)(?=.*\s)[a-zA-ZæøåÆØÅ0-9\s\.\,\-]{4,100}$",
        ErrorMessage = "Adressen skal indeholde mindst to bogstaver, et mellemrum og et tal, og må kun indeholde bogstaver, tal, punktum, komma eller bindestreg.")]
        public string Address { get; set; }

        [Display(Name = "By")]
        [Required(ErrorMessage = "Du skal angive din by")]
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ\- ]{2,}$", ErrorMessage = "Navnet på din by må kun indeholde bogstaver, - og mellemrum")]
        public string City { get; set; }

        [Display(Name = "Postnummer")]
        [Required(ErrorMessage = "Du skal angive et postnummer")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Postnummer skal være præcis 4 cifre.")]
        public string PostalCode { get; set; }

        [Display(Name = "Profilbillede")]
        public string? ProfileImagePath { get; set; }

        // Tom konstruktør kræves til modellens brug i fx databinding og frameworks (f.eks. Entity Framework).
        // Uden en parameterløs konstruktør kan fx Entity Framework og ASP.NET ikke fungere korrekt med vores modeller.
        public Coach()
        {

        }

        // Brugerdefineret konstruktør til oprettelse af træner med alle relevante felter.
        public Coach(string name, string phone, string email, int coachId, string address, string? profileImagePath, string? contractFileRoute, string city, string postalCode, decimal salary)
        {

            Name = name;
            Phone = phone;
            Email = email;
            Coach_Id = coachId;
            Address = address;
            ProfileImagePath = profileImagePath;
            ContractFilePath = contractFileRoute;
            City = city;
            PostalCode = postalCode;
            Salary = salary;
        }
  
    }
}



