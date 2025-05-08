using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Services;


namespace GadevangTennisklub2025.Pages.Teams
{
    public class AttendedTeamModel : PageModel
    {
        // This page should show the list of all the courses
        // Show the Courses title, description, and the number of places left in that course   maxNumOfAttendees - Attendees.Count
        #region Instance Fields
        private ITeamService _teamService;


        #endregion

        #region Properties
        public bool isAdmin { get; set; } = false;
        public List<Models.Team> ListOfAttendedTeams { get; private set; }
        private TimeOnly temp = new TimeOnly(23, 50);

        public Models.Member SelectedMember { get; set; }
        //Console.WriteLine("endTime: "+(temp));



        #endregion

        #region Constructors
        public AttendedTeamModel(ITeamService teamService)
        {
            _teamService = teamService;
            Console.WriteLine("TimeSlot: " + temp.AddHours(1.12));
        }
        #endregion

        #region Methods
        public IActionResult OnPostEdit(int ID)
        {
            Console.WriteLine("ShowTeam/OnPostEdit here and id = " + ID);
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

        public async Task<IActionResult> OnGetAsync()
        {
            SelectedMember = _teamService.MemberById(int.Parse(HttpContext.Session.GetString("Member_Id")));
            if (HttpContext.Session.GetString("IsAdmin") != null && bool.Parse(HttpContext.Session.GetString("IsAdmin")) == true)
            {
                isAdmin = true;
            }
            //Console.WriteLine("Teams/ShowTeam/OnGetAsync  timeslot is: "+(.TimeOfDay.Add(TimeSpan.FromHours(item.Length))));
            if (ListOfAttendedTeams == null)
            {
                ListOfAttendedTeams = await _teamService.GetAllAttendedTeamsAsync(SelectedMember);
                Thread.Sleep(1000);
            }
            Console.WriteLine("Team/ShowTeam/OnGetAsync is done");
            return Page();
        }
        #endregion
    }
}
