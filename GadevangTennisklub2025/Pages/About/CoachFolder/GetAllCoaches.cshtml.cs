using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.About
{
    // PageModel til visning af alle tr�nere p� en oversigtsside.
    // Tjekker ogs�, om brugeren er admin for at styre rettigheder.
    public class GetAllCoachesModel : PageModel
    {
        private ICoachService _coachService;
        public bool IsAdmin { get; set; } = false;


        public List<Models.Coach> Coaches { get; set; }

        public GetAllCoachesModel(ICoachService coachService)
        {
            _coachService = coachService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            //Tjekker om bruger er admin
            var isAdmin = HttpContext.Session.GetString("IsAdmin");

            IsAdmin = isAdmin == "true"; // Bruges i visningen til at afg�re om admin-funktioner skal vises
            //Henter alle tr�nere fra database via service
            try
            {
                Coaches = await _coachService.GetAllCoachesAsync();
    
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return RedirectToPage("Error");
            }

        }
    }
}
