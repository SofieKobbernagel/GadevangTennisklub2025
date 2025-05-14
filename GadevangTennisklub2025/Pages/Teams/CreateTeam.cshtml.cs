using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using GadevangTennisklub2025.Services;
using System.Reflection.PortableExecutable;

namespace GadevangTennisklub2025.Pages.Teams
{
    public class CreateTeamModel : PageModel
    {
        private ITeamService _teamService;
        private IBookingServiceAsync _bookingService;
        private ICoachService _coachService;
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

        [BindProperty]
        public int TrainerId { get; set; }

        public List<SelectListItem> TrainerOptions { get; set; }
        #endregion


        #region constructor
        public CreateTeamModel(ITeamService teamService, IBookingServiceAsync IBSA, ICoachService coachService) // dependency injection
        {
            _teamService = teamService; // parameter overført 
            _bookingService = IBSA;
            _coachService = coachService;
            Messages = new List<string>();
            //AttendeeRange[0] = MinMembers;
           // AttendeeRange[1] = MaxMembers;
        }
        #endregion


        public async Task<IActionResult> OnGet()
        {
            var coaches = await _coachService.GetAllCoachesAsync();

            TrainerOptions = coaches.Select(c => new SelectListItem
            {
                Value = c.Coach_Id.ToString(),
                Text = c.Name
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            AttendeeRange[0] = MinMembers;
            AttendeeRange[1] = MaxMembers;
            Team team = new Team(Id, Name, MembershipType,await _coachService.GetCoachByIdAsync(TrainerId), DayOfWeek, TimeOfDay, Length, AttendeeRange,Attendees, Description);
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


