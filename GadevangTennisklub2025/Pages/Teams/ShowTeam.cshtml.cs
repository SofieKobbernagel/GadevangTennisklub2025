using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using GadevangTennisklub2025.Services;
namespace GadevangTennisklub2025.Pages.Teams
{
    public class ShowTeamModel : PageModel
    {
        private readonly ITeamService _teamService;

        public bool isAdmin { get; set; } = false;
        public bool isLoggedIn { get; set; } = false;
        public List<string> MembershipTypes { get; set; } = new();

        public List<Team> ListOfTeams { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchType { get; set; } = "ID";

        public List<Team>? SearchList { get; set; }

        public ShowTeamModel(ITeamService teamService)
        {
            _teamService = teamService;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            // Get user info
            if (HttpContext.Session.GetString("IsAdmin") is string admin && bool.TryParse(admin, out var isAdminParsed))
                isAdmin = isAdminParsed;

            if (HttpContext.Session.GetString("Member_Id") != null)
                isLoggedIn = true;

            // Get search results if any
            if (!string.IsNullOrWhiteSpace(Search))
            {
                TeamService teamService = new TeamService(); // ideally inject this
                SearchList = await teamService.Search(SearchType, Search);
            }

            // Always get team list and membership types
            ListOfTeams = await _teamService.GetAllTeamsAsync();
            MembershipTypes = ListOfTeams
                .Select(t => t.MembershipType)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .ToList();
            return Page();
        }



        public IActionResult OnPostEdit(int ID)
        {
            return RedirectToPage("UpdateTeam", new { ID });
        }

        public IActionResult OnPostAttendTeam(int ATTENDID)
        {
            return RedirectToPage("AttendTeam", new { ATTENDID });
        }

        public IActionResult OnPostCreate()
        {
            return RedirectToPage("CreateTeam");
        }

        public IActionResult OnPostAttendedTeam()
        {
            return RedirectToPage("AttendedTeam");
        }
    }
}
