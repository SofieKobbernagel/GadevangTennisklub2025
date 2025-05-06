using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GadevangTennisklub2025.Pages.TennisField
{
    public class EditCourtModel : PageModel
    {
        private ICourtService _courtService;
        private ICourtTypeService _courtTypeService;

        [BindProperty]
        public Models.TennisField Court { get; set; }
        public List<SelectListItem> SelectListCourts { get; set; }
        public EditCourtModel(ICourtService courtService, ICourtTypeService courtTypeService)
        {
            _courtService = courtService;
            _courtTypeService = courtTypeService;
            SelectListCourts = new List<SelectListItem>();
        }
        async Task LoadList()
        {
            foreach (var item in await _courtTypeService.GetAllCourtsAsync())
            {
                SelectListCourts.Add(new SelectListItem(item.Type, Convert.ToString(item.Type)));
            }
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            await LoadList();
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
