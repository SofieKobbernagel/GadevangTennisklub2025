using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Interfaces;



namespace GadevangTennisklub2025.Pages.Team
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
        /*public ShowTeamModel(List<Models.Team> teamList)
        {
            _TeamRepo = teamList;
            ListOfTeams = _TeamRepo.GetAllTeamsAsync;
        }*/
        public ShowTeamModel(ITeamService teamService)
        {
            _teamService = teamService;
        }

        #endregion

        #region Methods




        public IActionResult OnPostEdit(int id)
        {
            return RedirectToPage("EditTeam", new { id });
        }


        public IActionResult OnPostSignIn()
        {
            Console.WriteLine("ShowTeam/OnPostSignIn just ran");
            return RedirectToPage("SignInTeam");
        }

        public void OnGet()
        {

        }
        #endregion
    }
}
