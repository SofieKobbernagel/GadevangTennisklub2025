using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.About.CoachFolder
{
    public class UpdateCoachModel : PageModel
    {
        private readonly ICoachService _coachService;



        public UpdateCoachModel(ICoachService coachService)
        {
            _coachService = coachService;
        }

        [BindProperty]
        public Models.Coach CoachObject { get; set; }
        [BindProperty]
        public IFormFile? ContractFile { get; set; }


        [BindProperty]
        public IFormFile? ProfileImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int coach_Id)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");

            if (isAdmin != "true")
            {
                return RedirectToPage("/Pages/Index");
            }
            try
            {
                CoachObject = await _coachService.GetCoachByIdAsync(coach_Id);
                if (CoachObject == null)
                    return RedirectToPage("/Index");
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return RedirectToPage("Error");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var existingCoach = await _coachService.GetCoachByIdAsync(CoachObject.Coach_Id);


            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ProfileImage != null)
            {
                var uploadsFolder = Path.Combine("wwwroot", "Images", "ProfilePictures");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }
                CoachObject.ProfileImagePath = "/Images/ProfilePictures/" + fileName;
            }
            else
            {
                // Behold eksisterende billede
                CoachObject.ProfileImagePath = existingCoach.ProfileImagePath;
            }

            if (ContractFile != null)
            {
                var contractFolder = Path.Combine("wwwroot", "Files", "Contracts");
                Directory.CreateDirectory(contractFolder);
                var contractFileName = Guid.NewGuid().ToString() + Path.GetExtension(ContractFile.FileName);
                var contractFilePath = Path.Combine(contractFolder, contractFileName);
                using (var stream = new FileStream(contractFilePath, FileMode.Create))
                {
                    await ContractFile.CopyToAsync(stream);
                }
                CoachObject.ContractFilePath = "/Contracts/" + contractFileName;
            }
            else
            {
                CoachObject.ContractFilePath = existingCoach.ContractFilePath;
            }



            await _coachService.UpdateCoachAsync(CoachObject, CoachObject.Coach_Id);
            TempData["SuccessMessage"] = "Brugeroplysninger er opdateret.";
            return RedirectToPage("GetAllCoaches");


        }
    }
}
