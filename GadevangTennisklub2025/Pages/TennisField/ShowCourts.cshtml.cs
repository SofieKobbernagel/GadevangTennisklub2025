using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GadevangTennisklub2025.Pages.TennisField
{
    public class ShowCourtsModel : PageModel
    {
        public ICourtService _courtService;

        public List<Models.TennisField> Courts { get; set; }

        public ShowCourtsModel(ICourtService courtService)
        {
            _courtService = courtService;
        }

        public async Task OnGetAsync()
        {
            Courts = await _courtService.GetAllCourtsAsync();
        }
    }
}
