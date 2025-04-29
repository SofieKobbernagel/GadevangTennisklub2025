using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.TennisField
{
    public class CreateCourtModel : PageModel
    {
        private ICourtService _courtService;

        [BindProperty]
        public Models.TennisField Court { get; set; }
        public string ErrorMessage { get; set; }

        public CreateCourtModel(ICourtService courtService)
        {
            _courtService = courtService;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }
            try //try-catch for at fange exceptions.
            {
                await _courtService.CreateCourtAsync(Court);
                return RedirectToPage("ShowCourts");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            return Page();
        }
    }
}
