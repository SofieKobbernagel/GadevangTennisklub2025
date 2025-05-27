using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace GadevangTennisklub2025.Pages.TennisField
{
    public class CreateCourtModel : PageModel
    {
        private ICourtService _courtService;
        private ICourtTypeService _courtTypeService;

        [BindProperty]
        public Models.TennisField Court { get; set; }
        [BindProperty]
        public List<SelectListItem> SelectListCourts { get; set; }
        public string ErrorMessage { get; set; }

        public CreateCourtModel(ICourtService courtService, ICourtTypeService courtTypeService)
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
        public async Task<IActionResult> OnGetAsync()
        {
            await LoadList();
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "true")
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { await LoadList(); return Page(); }
            try //try-catch for at fange exceptions.
            {
                await _courtService.CreateCourtAsync(Court);
                return RedirectToPage("ShowCourts");
            }
            catch (SqlException sqlEx)
            {
                ViewData["ErrorMessage"] = sqlEx.Message;
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            
        }
    }
}
