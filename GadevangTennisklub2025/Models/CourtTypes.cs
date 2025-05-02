using System.ComponentModel.DataAnnotations;

namespace GadevangTennisklub2025.Models
{
    public class CourtTypes
    {
        [Required]
        public string Type { get; set; }

        public CourtTypes(string type)
        {
            Type = type;
        }


    }
}
