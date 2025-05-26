using System.ComponentModel.DataAnnotations;

namespace GadevangTennisklub2025.Models.Validation
{
    public class ValidBirthdayAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validerer at ens fødselsdato er inden for et gyldigt datointerval.
        /// Sikrer, at datoen ikke ligger i fremtiden og ikke er mere end 120 år tilbage i tiden.
        /// </summary>
        /// <param name="value">Værdien der skal valideres (DateOnly der repræsenterer en fødselsdato).</param>
        /// <param name="validationContext">Kontekstinformation om valideringsoperationen.</param>
        /// <returns>
        /// Et ValidationResult der angiver, om valideringen er gyldig.
        /// ValidationResult er en indbygget del af .NET's valideringsinfrastruktur, som bruges sammen med dataannotations
        /// Returnerer en valideringsfejl, hvis datoen er i fremtiden eller mere end 120 år gammel og ellers returneres succes.
        /// </returns>

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
