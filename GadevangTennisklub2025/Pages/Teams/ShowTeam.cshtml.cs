using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Services;


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
        public bool isAdmin { get; set; } = false;
        public List<Team> ListOfTeams { get; set; } = new(); // prevents null

            


        #endregion

        #region Constructors
        public ShowTeamModel(ITeamService teamService)
        {
            _teamService = teamService;
           
            ListOfTeams =  _teamService.GetAllTeamsAsync().Result;
        }
        #endregion

        #region Methods
        public IActionResult OnPostEdit(int ID)
        {
            Console.WriteLine("ShowTeam/OnPostEdit here and id = "+ ID );
            return RedirectToPage("UpdateTeam", new { ID });
        }

        public IActionResult OnPostAttendTeam(int ATTENDID)
        {
            Console.WriteLine("ShowTeam/OnPostAttend just ran");
            return RedirectToPage("AttendTeam", new { ATTENDID });
        }

        public IActionResult OnPostCreate()
        {
            Console.WriteLine("ShowTeam/OnPostCreate just ran");
            return RedirectToPage("CreateTeam");
        }
        public IActionResult OnPostAttendedTeam()
        {
            Console.WriteLine("ShowTeam/OnPostAttendedTeam just ran");
            return RedirectToPage("AttendedTeam");
        }
        
        public async Task OnGetAsync()
        {
            ListOfTeams = await _teamService.GetAllTeamsAsync();
            if (HttpContext.Session.GetString("IsAdmin")!=null && bool.Parse(HttpContext.Session.GetString("IsAdmin"))==true) 
            { 
                isAdmin = true;
                
            }

            Console.WriteLine("Team/ShowTeam/OnGetAsync is done");
        }
        #endregion
    }
}
