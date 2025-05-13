using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Teams
{
    public class UpdateAttendeeListModel : PageModel
    {
        private ITeamService _teamService;
        private List<Team> Teams = new List<Team>();
        #region Properties
        [BindProperty] // Two way binding
        public Team tEAM { get; set; }
        public List<Models.Member> Attendees { get; set; }
        public bool isAdmin { get; set; } = false;

        public UpdateAttendeeListModel(ITeamService teamService) // dependency injection
        {
            _teamService = teamService; // parameter overført 
            //Attendees =  _teamService.GetAttendeesAsync(tEAM.Id).Result;
        }




        public async Task<IActionResult> OnPostFjern(int memId, int teamId)
        {
            tEAM = await _teamService.GetTeamFromIdAsync(teamId);
            Models.Member SelectedMember = _teamService.MemberById(memId);

            await _teamService.LeaveTeamAsync(tEAM, SelectedMember);

            TempData["SuccessMessage"] = $"{SelectedMember.Name} er nu afmeldt {tEAM.Name}!";
            return RedirectToPage("UpdateAttendeeList", new { AttendeeListTeamId = teamId });
        }

        public async Task<IActionResult> OnGet(int AttendeeListTeamId)
        {
      
            tEAM = await _teamService.GetTeamFromIdAsync(AttendeeListTeamId);

            Attendees = await _teamService.GetAttendeesAsync(AttendeeListTeamId);
             
            foreach(Models.Member mem in Attendees)
            {
                Console.WriteLine(mem.Name);
            }

            if (HttpContext.Session.GetString("IsAdmin") != null && bool.Parse(HttpContext.Session.GetString("IsAdmin")) == true)
            {
                isAdmin = true;
            }

            return Page();
        }
        #endregion
    }
}