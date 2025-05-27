using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

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
            try
            {
                await LoadList();
                Court = await _courtService.GetCourtFromIdAsync(id);
                var isAdmin = HttpContext.Session.GetString("IsAdmin");
                if (isAdmin != "true")
                {
                    return RedirectToPage("/Index");
                }
                return Page();
            }
            catch (SqlException sqlEx)
            {
                ViewData["ErrorMessage"] = sqlEx.Message;
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { await LoadList(); return Page(); }
            await _courtService.UpdateCourtAsync(Court);
            return RedirectToPage("ShowCourts");
        }
    }
}
