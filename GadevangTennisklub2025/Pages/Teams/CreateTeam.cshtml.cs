using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Interfaces;

namespace GadevangTennisklub2025.Pages.Teams
{
    public class CreateTeamModel : PageModel
    {
        private ITeamService _teamService;
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
        public int DayOfWeek {  get; set; }
        [BindProperty]
        public TimeOnly TimeOfDay {  get; set; }
        [BindProperty]
        public int Length {  get; set; }
        [BindProperty]
        public int[] AttendeeRange {  get; set; }
        [BindProperty]
        public List<Models.Member> Attendees {  get; set; }
        [BindProperty]
        public Models.Member Master {  get; set; }
        [BindProperty]
        public string Description {  get; set; }
        #endregion
        #region constructor
        public CreateTeamModel(ITeamService teamService) // dependency injection
        {
            _teamService = teamService; // parameter overført 
        }
        #endregion


        public IActionResult OnGet()
        {
            return Page();
        }
        public IActionResult OnPost(int id)
        {
            Team team = new Team(Id, Name, MembershipType, DayOfWeek,TimeOfDay,Length, AttendeeRange,Attendees,Master, Description);
            _teamService.CreateTeamAsync(team);
            return RedirectToPage("ShowTeam");
        }
        public IActionResult OnPostCancel()
        {
            return RedirectToPage("ShowTeam");
        }
    }
}


