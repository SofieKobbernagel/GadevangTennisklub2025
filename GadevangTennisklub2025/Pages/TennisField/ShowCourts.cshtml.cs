using GadevangTennisklub2025.Helper;
using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GadevangTennisklub2025.Pages.TennisField
{
    public class ShowCourtsModel : PageModel
    {
        public ICourtService _courtService;

        [BindProperty(SupportsGet = true)] public string FilterCriteria { get; set; }
        [BindProperty(SupportsGet = true)] public string SortBy { get; set; }
        [BindProperty(SupportsGet = true)] public string SortOrder { get; set; }

        public List<Models.TennisField> Courts { get; set; }

        public List<SelectListItem> SelectList { get; set; }

        public ShowCourtsModel(ICourtService courtService)
        {
            SortOrder = "CourtId";
            Courts = new List<Models.TennisField>();
            _courtService = courtService;
            SelectList = new List<SelectListItem>();
        }

        async Task LoadList()
        {
            foreach (var item in await _courtService.GetAllCourtsAsync())
            {
                SelectList.Add(new SelectListItem(item.Type, Convert.ToString(item.CourtId)));
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await LoadList();
                Courts = await _courtService.GetAllCourtsAsync();
                if (!string.IsNullOrWhiteSpace(FilterCriteria))
                {
                    string criteria = FilterCriteria.ToLower();
                    Courts = Courts.Where(m =>
                        (!string.IsNullOrEmpty(m.Name) && m.Name.ToLower().Contains(criteria)) ||
                        (!string.IsNullOrEmpty(m.Type) && m.Type.ToLower().Contains(criteria))
                    ).ToList();
                }
                if (Courts == null)
                    return RedirectToPage("Index");
                if (SortBy == "Name") { Courts.Sort(new CourtNameCompare()); }
                if (SortBy == "Type") { Courts.Sort(); }
                if (SortOrder == "Descending") { Courts.Reverse(); }
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return RedirectToPage("Pages/Error");
            }
        }
    }
}
