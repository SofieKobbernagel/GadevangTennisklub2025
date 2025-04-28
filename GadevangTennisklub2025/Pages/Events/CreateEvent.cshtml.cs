using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Events
{
    public class CreateEventModel : PageModel
    {
        public IEventServiceAsync EventServicesAsync;
        [BindProperty]
        public Event ev { get; set; }
        public CreateEventModel(IEventServiceAsync IESA)
        {
            EventServicesAsync = IESA;
            ev = new Event();
        }
        public async void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                await EventServicesAsync.CreateEventAsync(ev);
                return Redirect("ShowEvents");
            }

            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;

                return null;

            }

        }
    }
}
