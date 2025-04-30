using System.ComponentModel.DataAnnotations;

namespace GadevangTennisklub2025.Models
{
    public class Membership
    {
        [Required]
        public string MembershipType { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Rights { get; set; } = string.Empty;

        public Membership(string type, decimal price, string rights)
        {
            MembershipType = type;
            Price = price;
            Rights = rights;
        }
    }
}
