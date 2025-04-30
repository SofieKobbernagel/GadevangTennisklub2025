using System.ComponentModel.DataAnnotations;

namespace GadevangTennisklub2025.Models.Validation
{
    public class ValidBirthdayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateOnly birthday)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                if (birthday > today || birthday < today.AddYears(-120))
                {
                    return new ValidationResult("Fødselsdag skal være i dag eller højst 120 år tilbage.");
                }
            }

            return ValidationResult.Success!;
        }
    }

}
