using System.ComponentModel.DataAnnotations;

namespace GadevangTennisklub2025.Models
{
    public class Member
    {
        private int _count = 0;

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
        public string Phone { get; set; }

        [Display(Name = "Postnummer")]
        [Required(ErrorMessage = "Du skal angive et postnummer")]
        public string PostalCode { get; set; }

        [Display(Name = "Køn")]
        [Required(ErrorMessage = "Du skal angive dit køn")]
        public string Gender { get; set; }

        [Display(Name = "Medlems Id")]
        public int Member_Id { get; set; }

        [Display(Name = "Adresse")]
        [Required(ErrorMessage = "Du skal angive din adresse")]
        public string Address { get; set; }

        [Display(Name = "Navn")]
        [Required(ErrorMessage = "Du skal angive dit fulde navn")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Der skal gives en email")]
        public string Email { get; set; }

        public int BookingsLeft { get; set; }

        [Display(Name = "brugernavn")]
        [Required(ErrorMessage = "Du skal angive et brugernavn")]
        public string Username { get; set; }

        [Display(Name = "Kodeord")]
        [Required(ErrorMessage = "Du skal angive et kodeord")]
        private string _password;

        public string Password
        {
            get { return _password; }
            private set { _password = value; }
        }


        [Display(Name = "Is Admin")]
        public bool IsAdmin { get; set; }

        public bool NewsSubscriber { get; set; }

       
      
        public string? ProfileImagePath { get; set; }




        public Member(string username, string name, DateOnly birthday, string membertype, string city, string phone, string postalcode, string gender, string address, string email, string password)
        {
            Member_Id = _count++;
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
        }
       
        
    }
}
