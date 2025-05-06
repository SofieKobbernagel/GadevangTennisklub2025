using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.About
{
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
            var isAdmin = HttpContext.Session.GetString("IsAdmin");

            IsAdmin = isAdmin == "true";

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
