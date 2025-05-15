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

        public string Message { get; set; }
        public int MemberCount { get { return relationshipsServicesAsync.GetEventParticipants(e.Id).Result.Count; } }
        
        public bool notadmin { get; set; }
        public EventPageModel(IEventServiceAsync IESA, IRelationshipsServicesAsync IRSA)
        {
            eventServicesAsync = IESA;
            relationshipsServicesAsync = IRSA;
           
        }
        public async Task OnGetAsync(int EventId)
        {
            notadmin = false;
            if (HttpContext.Session.GetString("Member_Id") == null || !bool.Parse(HttpContext.Session.GetString("IsAdmin")))
            {
                notadmin = true;
            }
            e =  eventServicesAsync.returnEventAsync(EventId).Result;
            
            
        }

        public async Task<IActionResult> OnPost()
        {
            try 
            {
                int t = Convert.ToInt32(HttpContext.Session.GetString("Member_Id"));
                await  relationshipsServicesAsync.EventMemberRelation(e.Id, int.Parse(HttpContext.Session.GetString("Member_Id")));
                TempData["SuccessMessage"] = "du er tilmeldt event";
                return Redirect("ShowEvents");
            }
            catch (Exception ex) 
            {
                Message = "fejl Forsøg igen";
                return null;
            }
           
        }

        public async Task<IActionResult>OnPostDelete()
        {
            try
            {
                await eventServicesAsync.DeleteEventAsync(e);
                TempData["SuccessMessage"] = "event slettet";
                return Redirect("ShowEvents");
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<IActionResult> OnPostUpdate()
        {
            try
            {
                await eventServicesAsync.UpdateEventAsync(e);
                TempData["SuccessMessage"] = "Event opdateret";
                return Redirect("ShowEvents");
            }
            catch (Exception ex)
            {
                return null;
            }

        }

       
    }
}
