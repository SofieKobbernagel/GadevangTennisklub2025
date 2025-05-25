using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.About.CoachFolder
{
    // PageModel til sletning af en tr�nerprofil (kun for administratorer)
    public class DeleteCoachModel : PageModel
    {
        private ICoachService _CoachService;
        // Dependency Injection af CoachService (service til h�ndtering af tr�nere)
        public DeleteCoachModel(ICoachService coachService)
        {
            _CoachService = coachService;
        }
        [BindProperty]
        public Models.Coach Coach { get; set; }

        public async Task<IActionResult> OnGetAsync(int coach_Id)
        {
            // Tjek om brugeren er admin � hvis ikke, send tilbage til forsiden
            var isAdmin = HttpContext.Session.GetString("IsAdmin");

            if (isAdmin != "true")
            {
                return RedirectToPage("/Pages/Index");
            }
            try
            {
                // Henter coach-data baseret p� coach_Id til bekr�ftelsessiden
                Coach = await _CoachService.GetCoachByIdAsync(coach_Id);
                if (Coach == null)
                    // Hvis coach ikke findes, send brugeren til siden med alle tr�nere
                    return RedirectToPage("GetAllCoaches");
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
                // Bekr�ft at Coach-objektet og Coach_Id er til stede
                if (Coach == null || Coach.Coach_Id == null)
                {
                    ModelState.AddModelError("", "ID mangler.");
                    return Page();
                }

                // Slet tr�neren via CoachService
                Models.Coach deletedCoach = await _CoachService.DeleteCoachAsync(Coach.Coach_Id);
                TempData["SuccessMessage"] = "Tr�neren er blevet slettet";
                if (deletedCoach == null)
                    // Hvis tr�ner slettes succesfuldt, vis besked og redirect til oversigten
                    return RedirectToPage("/GetAllCoaches");
                else
                {
                    // Hvis sletningen mislykkes, fjern session og redirect (n�dprocedure)
                    HttpContext.Session.Clear();
                }

                return RedirectToPage("/About/CoachFolder/GetAllCoaches");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Der skete en fejl da tr�neren skulle slettes: " + ex.Message);
                return Page();
            }
        }
    }
}
