using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Member
{
    // PageModel til oprettelse af et nyt medlem i systemet.
    // Indehodler bl.a. upload af profilbillede og validering af unikt brugernavn.
    public class CreateMemberModel : PageModel
    {
        // Service til håndtering af medlemmer (oprettelse, validering mv.)
        private IMemberService _memberService;
        // Service til håndtering af medlemskaber
        private readonly IMembershipService _membershipService;

        // Indeholder listen af mulige medlemskaber til dropdown i formularen
        [BindProperty]
        public List<Membership> Memberships { get; set; } = new();
        // ViewModel som indeholder det nye medlem og sørger for at betingelser bliver accepteret
        [BindProperty]
        public RegisterMemberViewModel RegisterModel { get; set; }

        // Uploadet profilbillede fra formularen
        [BindProperty]
        public IFormFile? ProfileImage { get; set; }

        // Constructor med dependency injection af services
        public CreateMemberModel(IMemberService memberService, IMembershipService membershipService)
        {
            _memberService = memberService;
            _membershipService = membershipService;
        }

        // Henter alle medlemskaber når siden indlæses, så de kan vises i formularen
        public async Task<IActionResult> OnGetAsync()
        {
            Memberships = await _membershipService.GetAllMembershipsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Tjek om modellen er gyldig (baseret på validation fra Member klassen i models folderen)
                if (!ModelState.IsValid)
                {
                    Memberships = await _membershipService.GetAllMembershipsAsync();
                    return Page();
                }

                // Tjek om brugernavnet er unikt
                bool isUnique = await _memberService.IsUsernameUnique(RegisterModel.Member.Username);
                if (!isUnique)
                {
                    ModelState.AddModelError("RegisterModel.Member.Username", "Brugernavnet er taget, vælg venligst et andet.");
                    Memberships = await _membershipService.GetAllMembershipsAsync();
                    return Page();
                }

                // Hvis der er uploadet et profilbillede, gem det på serveren
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    // Generer unikt filnavn
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);

                    // Bygger stien hvor billedet skal gemmes
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ProfilePictures", fileName);

                    // Gem filen 
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(stream);
                    }

                    // Gem stien i databasen
                    RegisterModel.Member.ProfileImagePath = "/images/ProfilePictures/" + fileName;
                }

                // Forsøg at oprette medlemmet
                bool success = await _memberService.CreateMemberAsync(RegisterModel.Member);

                if (success)
                {
                    // Ved succes, redirect til forsiden
                    return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError("", "Could not create the member. Please try again.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the member: " + ex.Message);
                return Page();
            }
        }
    }
}
