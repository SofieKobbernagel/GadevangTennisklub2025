using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models.ViewModels;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Services;

namespace GadevangTennisklub2025.Pages.About
{
    /// <summary>
    /// PageModel til oprettelse af ny træner i systemet.
    /// Håndterer både upload af profilbillede og kontraktfil.
    /// Kun administratorer har adgang til siden.
    /// </summary>
    public class CreateCoachModel : PageModel
    {
        // Service til oprettelse af coach i databasen
        private ICoachService _coachService;
        // Giver adgang til miljøoplysninger, fx root-path til filhåndtering
        private readonly IWebHostEnvironment _env;

        // Træner-objektet der udfyldes via formularen
        [BindProperty]
        public Coach Coach { get; set; }
        // Uploadet profilbillede (valgfrit)
        [BindProperty]
        public IFormFile? ProfileImage { get; set; }
        // Brugeren skal uploade en kontraktfil, men egenskaben er nullable fordi filen først bindes under formularindsendelse
        [BindProperty]
        public IFormFile? ContractFile { get; set; }

        public CreateCoachModel(ICoachService coachService, IWebHostEnvironment env)
        {
            _coachService = coachService;
            _env = env;
        }
        public bool IsAdmin { get; set; } = false;

        // Tjekker om brugeren er admin. Hvis ikke, omdirigeres de til forsiden.
        public async Task<IActionResult> OnGetAsync()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "true")
            {
                return RedirectToPage("/Index");
            }
            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Fjern ContractFilePath fra ModelState da den sættes manuelt (ikke via formular)
                ModelState.Remove("Coach.ContractFilePath");
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                // Hvis brugeren har uploadet et profilbillede
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    // Generer et unikt filnavn (så man ikke overskriver hinandens billeder)
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);

                    // Opretter sti til /images/ProfilePictures/
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ProfilePictures", fileName);

                    // Gemmer filen i ProfilePictures-mappen
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(stream);
                    }

                    // Gemmer derefter stien i databasen
                    Coach.ProfileImagePath = "/images/ProfilePictures/" + fileName;
                }

                // Hvis en kontraktfil er uploadet
                if (ContractFile != null && ContractFile.Length > 0)
                {
                    // Gem den i /wwwroot/Contracts med unikt filnavn
                    var contractFolderPath = Path.Combine(_env.WebRootPath, "Contracts");

                    var contractFileName = Guid.NewGuid().ToString() + Path.GetExtension(ContractFile.FileName);
                    var contractFilePath = Path.Combine(contractFolderPath, contractFileName);

                    using (var stream = new FileStream(contractFilePath, FileMode.Create))
                    {
                        await ContractFile.CopyToAsync(stream);
                    }

                    // Gem stien i databasen
                    Coach.ContractFilePath = "/Contracts/" + contractFileName;
                }

                // Forsøger at oprette træneren via coachService
                bool success = await _coachService.CreateCoachAsync(Coach);

                // Viser besked ved succes og redirecter, ellers vis fejl
                if (success)
                {
                    TempData["SuccessMessage"] = "Træner oprettet succesfuldt!";
                    return RedirectToPage("/About/CoachFolder/GetAllCoaches");

                }
                else
                {
                    ModelState.AddModelError("", "Could not create the coach. Please try again.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the coach: " + ex.Message);
                return Page();
            }
        }
    }
}
