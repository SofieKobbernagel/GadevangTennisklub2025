using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models;



namespace GadevangTennisklub2025.Pages.Team
{
    public class ShowTeamModel : PageModel
    {
        // This page should show the list of all the courses
        // Show the Courses title, description, and the number of places left in that course   maxNumOfAttendees - Attendees.Count
        #region Instance Fields
       // private TeamRepository _TeamRepo;

        #endregion

        #region Properties
        public List<Team> ListOfTeams { get; private set; }
        public Team team { get; set; }

        #endregion

        #region Constructors
        public ShowTeamModel(/*TeamRepository teamRepository*/)
        {
            //_TeamRepo = teamRepository;
            //ListOfTeams = _TeamRepo.GetAll();
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
