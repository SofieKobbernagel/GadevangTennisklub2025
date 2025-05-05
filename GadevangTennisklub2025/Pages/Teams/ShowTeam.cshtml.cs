using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Interfaces;


namespace GadevangTennisklub2025.Pages.Teams
{
    public class ShowTeamModel : PageModel
    {
        // This page should show the list of all the courses
        // Show the Courses title, description, and the number of places left in that course   maxNumOfAttendees - Attendees.Count
        #region Instance Fields
       private ITeamService _teamService;
        

        #endregion

        #region Properties
        public List<Models.Team> ListOfTeams { get; private set; }
        

        #endregion

        #region Constructors
        public ShowTeamModel(ITeamService teamService)
        {
            _teamService = teamService;
        }
        #endregion

        #region Methods
        public IActionResult OnPostEdit(int ID)
        {
            Console.WriteLine("ShowTeam/OnPostEdit here and id = "+ ID );
            return RedirectToPage("UpdateTeam", new { ID });
        }

        public IActionResult OnPostAttend(int id)
        {
            return RedirectToPage("AttendTeam", new { id });
        }

        public IActionResult OnPostCreate()
        {
            Console.WriteLine("ShowTeam/OnPostCreate just ran");
            return RedirectToPage("CreateTeam");
        }

        public async Task OnGetAsync()
        {
            if (ListOfTeams == null)
            {
                ListOfTeams = await _teamService.GetAllTeamsAsync();
                Thread.Sleep(1000);
            }
            Console.WriteLine("Team/ShowTeam/OnGetAsync is done");
        }
        #endregion
    }
}
