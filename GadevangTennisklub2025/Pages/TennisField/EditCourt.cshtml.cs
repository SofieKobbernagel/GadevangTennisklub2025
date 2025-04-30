using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.TennisField
{
    public class EditCourtModel : PageModel
    {
        private ICourtService _courtService;
        [BindProperty]
        public Models.TennisField Court { get; set; }
        public EditCourtModel(ICourtService courtService)
        {
            _courtService = courtService;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Court = await _courtService.GetCourtFromIdAsync(id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _courtService.UpdateCourtAsync(Court);
            return RedirectToPage("ShowCourts");
        }
    }
}
