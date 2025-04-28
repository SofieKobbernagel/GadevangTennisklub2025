using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Events
{
    public class EventPageModel : PageModel
    {
        IEventServiceAsync eventServicesAsync { get; set; }
        IRelationshipsServicesAsync relationshipsServicesAsync { get; set; }
        [BindProperty]
        public Event e { get; set; }
        public EventPageModel(IEventServiceAsync IESA, IRelationshipsServicesAsync IRSA)
        {
            eventServicesAsync = IESA;
            relationshipsServicesAsync = IRSA;
           
        }
        public async Task OnGetAsync(int EventId)
        {
            e = await eventServicesAsync.returnEventAsync(EventId);

        }

        public IActionResult OnPost()
        {
            try 
            {
                int t = Convert.ToInt32(HttpContext.Session.GetString("Member_Id"));
                relationshipsServicesAsync.EventMemberRelation(e.Id, int.Parse(HttpContext.Session.GetString("Member_Id")));
                return Redirect("ShowEvents");
            }
            catch (Exception ex) 
            {
                return null;
            }
           
        }

        public IActionResult OnPostSignUp()
        {

            return RedirectToPage("/Index");
        }
    }
}
