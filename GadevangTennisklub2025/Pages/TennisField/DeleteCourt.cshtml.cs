using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.TennisField
{
    public class DeleteCourtModel : PageModel
    {
        private ICourtService _courtService;
        [BindProperty]
        public Models.TennisField Court { get; set; }
        [BindProperty] public bool Confirm { get; set; }
        public string MessageError { get; set; }
        public DeleteCourtModel(ICourtService courtService)
        {
            _courtService = courtService;
        }
        public async Task<IActionResult> OnGetAsync(int deleteId)
        {
            Court = await _courtService.GetCourtFromIdAsync(deleteId);
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "true")
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Confirm == false)
            {
                MessageError = $"Husk at klikke på konfirmation";
                return Page();
            }
            await _courtService.DeleteCourtAsync(Court.CourtId);
            return RedirectToPage("ShowCourts");
        }
    }
}
