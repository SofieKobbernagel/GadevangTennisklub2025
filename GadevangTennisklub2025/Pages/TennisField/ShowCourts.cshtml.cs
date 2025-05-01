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

        public List<Models.TennisField> Courts { get; set; }

        public List<SelectListItem> SelectList { get; set; }

        public ShowCourtsModel(ICourtService courtService)
        {
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
                if (Courts == null)
                    return RedirectToPage("Index");
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
