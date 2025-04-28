using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Events
{
    public class EventPageModel : PageModel
    {
        IEventServiceAsync eventServicesAsync { get; set; }
        [BindProperty]
        public Event e { get; set; }
        public EventPageModel(IEventServiceAsync IESA)
        {
            eventServicesAsync = IESA;
           
        }
        public async Task OnGetAsync(int EventId)
        {
            e = await eventServicesAsync.returnEventAsync(EventId);

        }

        public IActionResult OnPost()
        {
            return Redirect("GetAllHotels");
        }

        public IActionResult OnPostSignUp()
        {

            return RedirectToPage("/Index");
        }
    }
}
