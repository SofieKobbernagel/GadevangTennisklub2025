using GadevangTennisklub2025.Models.Validation;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace GadevangTennisklub2025.Models
{
    public class Member
    {
       
        [Display(Name = "Fødselsdag")]
        [Required(ErrorMessage = "Du skal angive din fødselsdag")]
        [ValidBirthday]
        public DateOnly Birthday { get; set; }

        public int Age
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                int age = today.Year - Birthday.Year;
                if (Birthday > today.AddYears(-age))
                {
                    age--;
                }

                return age;
            }
        }

        public double Price;

        [Display(Name = "Medlemstyper")]
        [Required(ErrorMessage = "Du skal vælge en medlemstype")]
        public string MemberType { get; set; }

        [Display(Name = "By")]
        [Required(ErrorMessage = "Du skal angive din by")]
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ\- ]{2,}$", ErrorMessage = "Navnet på din by må kun indeholde bogstaver, - og mellemrum")]
        public string City { get; set; }

        [Display(Name = "Tlf")]
        [Required(ErrorMessage = "Du skal angive et telefonnummer")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Telefonnummer skal være præcis 8 cifre og kun tal.")]
        public string Phone { get; set; }

        [Display(Name = "Postnummer")]
        [Required(ErrorMessage = "Du skal angive et postnummer")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Postnummer skal være præcis 4 cifre.")]
        public string PostalCode { get; set; }

        [Display(Name = "Køn")]
        [Required(ErrorMessage = "Du skal angive dit køn")]
        public string Gender { get; set; }

        [Display(Name = "Medlems Id")]
        public int Member_Id { get; set; }

        [Display(Name = "Adresse")]
        [Required(ErrorMessage = "Du skal angive din adresse")]
        //(?=.*[a-zA-ZæøåÆØÅ]{2,}) betyder at der skal være mindst to bogstaver et sted
        //(?=.*\d) betyder at der skal være mindst et tal et sted
        //(?=.*\s) betyder at der skal være mindst et mellemrum et sted
        //[a-zA-ZæøåÆØÅ0-9\s\.\,\-]{4,100} betyder at selve indholdet kun må indeholde tilladte tegn og mellem 4 og 100 tegn langt
        //(?=...) er en positive lookahead. Den kigger fremad i strengen og kræver, at noget findes et sted, men forbruger ikke tegnene(dvs.matcher ikke indholdet direkte)
        //. betyder = hvilket som helst enkelt tegn (undtagen linjeskift) og * betyder "0 eller flere gentagelser"
        [RegularExpression(@"^(?=.*[a-zA-ZæøåÆØÅ]{2,})(?=.*\d)(?=.*\s)[a-zA-ZæøåÆØÅ0-9\s\.\,\-]{4,100}$",
        ErrorMessage = "Adressen skal indeholde mindst to bogstaver, et mellemrum og et tal, og må kun indeholde bogstaver, tal, punktum, komma eller bindestreg.")]
        public string Address { get; set; }

        [Display(Name = "Hjemmekommune")]
        [Required(ErrorMessage = "Du skal angive din kommune")]
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ]{3,}$", ErrorMessage = "Navnet på din kommune må kun indeholde bogstaver og skal være på mindst 3 tegn.")]
        public string Municipality { get; set; }

        [Display(Name = "Navn")]
        [Required(ErrorMessage = "Du skal angive dit fulde navn")]
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ\- ]{2,}$", ErrorMessage = "Dit navn skal være mindst 2 tegn og må kun indeholde bogstaver, bindestreg eller mellemrum.")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Der skal gives en email")]
        [RegularExpression(@".+@.+\.(com|dk)", ErrorMessage = "Email skal slutte på .com eller .dk.")]
        public string Email { get; set; }

        public int BookingsLeft { get; set; }

        [Display(Name = "brugernavn")]
        [Required(ErrorMessage = "Du skal angive et brugernavn")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Brugernavn skal være mellem 3 og 20 karakterer.")]
        [RegularExpression(@"^[a-zA-Z0-9_æøåÆØÅ\- ]+$", ErrorMessage = "Brugernavn må kun indeholde bogstaver, tal, - eller _.")]
        public string Username { get; set; }

        [Display(Name = "Samtykke til offentliggørelse af billeder")]
        [Required(ErrorMessage = "Vælg venligst en mulighed for billedtilladelse")]
        public string PictureConsent { get; set; }

        [Display(Name = "Kodeord")]
        [Required(ErrorMessage = "Du skal angive et kodeord")]
        public string Password { get; set; }



        [Display(Name = "Er Admin")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Tilmeldt nyhedsbrev")]
        public bool NewsSubscriber { get; set; } = false;

        [Display(Name = "Evt. Anden TLF (valgfri)")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Telefonnummer skal være præcis 8 cifre og kun tal.")]
        public string? OtherPhone { get; set; }

        [Display(Name = "Profilbillede")]
        public string? ProfileImagePath { get; set; }


        public Member()
        {

        }

        public Member(string username, string name, DateOnly birthday, string membertype, string city, string phone, string postalcode, string gender, string address, string email, string password, string municipality, string consent, int memberId)
        {
            IsAdmin = false;
            Name = name;
            Birthday = birthday;
            MemberType = membertype;
            City = city;
            Phone = phone;
            PostalCode = postalcode;
            Gender = gender;
            Address = address;
            Email = email;
            Password = password;
            Username = username;
            Municipality = municipality;
            PictureConsent = consent;
            Member_Id = memberId;
            BookingsLeft = 4;

        }
        public Member(string username, string name, DateOnly birthday, string membertype, string city, string phone, string postalcode, string gender, string address, string email, string password, string municipality, string consent)
        {
            IsAdmin = false;
            Name = name;
            Birthday = birthday;
            MemberType = membertype;
            City = city;
            Phone = phone;
            PostalCode = postalcode;
            Gender = gender;
            Address = address;
            Email = email;
            Password = password;
            Username = username;
            Municipality = municipality;
            PictureConsent = consent;
            BookingsLeft = 4;
        }

        public Member(int id) 
        {
            Member_Id = id;
        }
    }
}
