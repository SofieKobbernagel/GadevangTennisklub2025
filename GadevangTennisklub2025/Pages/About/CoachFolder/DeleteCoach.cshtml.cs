using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.About.CoachFolder
{
    public class DeleteCoachModel : PageModel
    {
        private ICoachService _CoachService;
        public DeleteCoachModel(ICoachService coachService)
        {
            _CoachService = coachService;
        }
        [BindProperty]
        public Models.Coach Coach { get; set; }

        public async Task<IActionResult> OnGetAsync(int coach_Id)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");

            if (isAdmin != "true")
            {
                return RedirectToPage("/Pages/Index");
            }
            try
            {
                Coach = await _CoachService.GetCoachByIdAsync(coach_Id);
                if (Coach == null)
                    return RedirectToPage("MyProfile");
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
            try
            {
                if (Coach == null || Coach.Coach_Id == null)
                {
                    ModelState.AddModelError("", "ID mangler.");
                    return Page();
                }

                Models.Coach deletedCoach = await _CoachService.DeleteCoachAsync(Coach.Coach_Id);
                TempData["SuccessMessage"] = "Træneren er blevet slettet";
                if (deletedCoach == null)
                    return RedirectToPage("/GetAllCoaches");
                else
                {
                    HttpContext.Session.Clear();
                }

                return RedirectToPage("/About/CoachFolder/GetAllCoaches");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Der skete en fejl da træneren skulle slettes: " + ex.Message);
                return Page();
            }
        }
    }
}
