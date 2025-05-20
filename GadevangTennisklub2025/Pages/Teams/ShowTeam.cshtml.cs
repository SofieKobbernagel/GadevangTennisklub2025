using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GadevangTennisklub2025.Pages.Teams
{
    public class ShowTeamModel : PageModel
    {
        private readonly ITeamService _teamService;
        private readonly ICoachService _coachService;

        public ShowTeamModel(ITeamService teamService, ICoachService coachService)
        {
            _teamService = teamService;
            _coachService = coachService;
        }

        public bool isAdmin { get; set; }
        public bool isLoggedIn { get; set; }

        public List<Team> ListOfTeams { get; set; } = new();
        public List<string> MembershipTypes { get; set; } = new();
        public List<string> Coaches { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchType { get; set; } = "Name";

        public List<Team>? SearchList { get; set; }

        public async Task OnGetAsync()
        {
            // Session flags
            if (HttpContext.Session.GetString("IsAdmin") is string admin && bool.TryParse(admin, out var isAdminParsed))
                isAdmin = isAdminParsed;
            isLoggedIn = HttpContext.Session.GetString("Member_Id") != null;

            // Get full team list
            ListOfTeams = await _teamService.GetAllTeamsAsync();

            // Populate dropdown options
            MembershipTypes = ListOfTeams
                .Select(t => t.MembershipType)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .ToList();

            Coaches = (await _coachService.GetAllCoachesAsync())
                .Select(c => c.Name)
                .Distinct()
                .ToList();

            // If searching, run search
            if (!string.IsNullOrWhiteSpace(Search))
            {
                SearchList = await _teamService.Search(SearchType, Search);
            }
        }

        public IActionResult OnPostEdit(int ID)=> RedirectToPage("UpdateTeam", new { ID });
        public IActionResult OnPostAttendTeam(int ATTENDID) => RedirectToPage("AttendTeam", new { ATTENDID });
        public IActionResult OnPostCreate() => RedirectToPage("CreateTeam");
        public IActionResult OnPostAttendedTeam() => RedirectToPage("AttendedTeam");
    }
}
