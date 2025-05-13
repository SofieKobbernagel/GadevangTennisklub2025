using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Interfaces;

namespace GadevangTennisklub2025.Pages.Teams
{
    public class CreateTeamModel : PageModel
    {
        private ITeamService _teamService;
        private IBookingServiceAsync _bookingService;
        private List<Team> Teams = new List<Team>();
        #region Properties
        [BindProperty] // Two way binding
        public Team Team { get; set; }
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string MembershipType { get; set; }
        [BindProperty]
        public int DayOfWeek { get; set; }
        [BindProperty]
        public TimeOnly TimeOfDay { get; set; }
        [BindProperty]
        public Double Length { get; set; }
        [BindProperty]
        public int MinMembers { get; set; }
        [BindProperty]
        public int MaxMembers { get; set; }
        [BindProperty]
        public List<Models.Member> Attendees { get; set; }

        [BindProperty]
        public string Description { get; set; }
        public int[] AttendeeRange = new int[2];

        [BindProperty]
        public List<string> Messages { get; set; }
        #endregion
        
        
        #region constructor
        public CreateTeamModel(ITeamService teamService, IBookingServiceAsync IBSA) // dependency injection
        {
            _teamService = teamService; // parameter overført 
            _bookingService = IBSA;
            Messages = new List<string>();
            //AttendeeRange[0] = MinMembers;
           // AttendeeRange[1] = MaxMembers;
        }
        #endregion


        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost(int id)
        {
            AttendeeRange[0] = MinMembers;
            AttendeeRange[1] = MaxMembers;
            Team team = new Team(Id, Name, MembershipType, DayOfWeek, TimeOfDay, Length, AttendeeRange,Attendees, Description);
            await _teamService.CreateTeamAsync(team);
            List<DateTime> Temp = await _bookingService.TeamCreation(team);
            
            if (Temp.Count!=0)
            {
                foreach (DateTime d in Temp) 
                {
                    Messages.Add(d.ToString()+" dato ikke tilgængelig");
                }
                return null;
            }
            else 
            {
                TempData["SuccessMessage"] = "Holdet er oprettet og er nu synligt for medlemmer!";
                return RedirectToPage("ShowTeam");
            }

                
           
        }
        public IActionResult OnPostCancel()
        {
            return RedirectToPage("ShowTeam");
        }
    }
}


