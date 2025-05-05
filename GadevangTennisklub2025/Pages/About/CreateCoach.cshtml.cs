using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models.ViewModels;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Services;

namespace GadevangTennisklub2025.Pages.About
{
    public class CreateCoachModel : PageModel
    {
        private ICoachService _coachService;

        [BindProperty]
        public Coach Coach { get; set; }
        [BindProperty]
        public IFormFile? ProfileImage { get; set; }
        [BindProperty]
        public IFormFile? ContractFile { get; set; }

        public CreateCoachModel(ICoachService coachService)
        {
            _coachService = coachService;
        }
        public bool IsAdmin { get; set; } = false;

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
                ModelState.Remove("Coach.ContractFilePath");
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    // Generate a unique filename (so people don't overwrite each other�s pictures)
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);

                    // Build the path to /images/ProfilePictures/
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ProfilePictures", fileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(stream);
                    }

                    // Save the relative path to the database
                    Coach.ProfileImagePath = "/images/ProfilePictures/" + fileName;
                }

                if (ContractFile != null && ContractFile.Length > 0)
                {
                    // Generate a unique filename (so people don't overwrite each other�s pictures)
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ContractFile.FileName);

                    // Build the path to /images/ProfilePictures/
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Contracts", fileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ContractFile.CopyToAsync(stream);
                    }

                    // Save the relative path to the database
                    Coach.ContractFilePath = "/Contracts/" + fileName;
                }

                bool success = await _coachService.CreateCoachAsync(Coach);

                if (success)
                {
                    TempData["SuccessMessage"] = "Tr�ner oprettet succesfuldt!";
                    return RedirectToPage("/GetAllCoaches");

                }
                else
                {
                    ModelState.AddModelError("", "Could not create the coach. Please try again.");
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
