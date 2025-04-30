using System.ComponentModel.DataAnnotations;

namespace GadevangTennisklub2025.Models
{
    public class Member
    {
       
        [Display(Name = "Fødselsdag")]
        [Required(ErrorMessage = "Du skal angive din fødselsdag")]
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
        public string Address { get; set; }

        [Display(Name = "Hjemmekommune")]
        [Required(ErrorMessage = "Du skal angive din kommune")]
        public string Municipality { get; set; }

        [Display(Name = "Navn")]
        [Required(ErrorMessage = "Du skal angive dit fulde navn")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Der skal gives en email")]
        [RegularExpression(@".+@.+\.(com|dk)", ErrorMessage = "Email skal slutte på .com eller .dk.")]
        public string Email { get; set; }

        public int BookingsLeft { get; set; }

        [Display(Name = "brugernavn")]
        [Required(ErrorMessage = "Du skal angive et brugernavn")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Brugernavn skal være mellem 3 og 20 karakterer.")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Brugernavn må kun indeholde bogstaver, tal, - eller _.")]
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
        }
    }
}
