using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GadevangTennisklub2025.Pages.Teams
{
    public class UpdateTeamModel : PageModel
    {
            private ITeamService _teamService;
            private ICoachService _coachService;
            private List<Team> Teams = new List<Team>();
            #region Properties
            [BindProperty] // Two way binding
            public Team tEAM { get; set; }
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
            public double Length { get; set; }
            [BindProperty]
            public int MinMember { get; set; }
            [BindProperty]
            public int MaxMember { get; set; }
            [BindProperty]
            public string Description { get; set; }

        [BindProperty]
        public int TrainerId { get; set; }

        public List<SelectListItem> TrainerOptions { get; set; }

        #endregion
        //        private Task<List<Hotel>> hots;
        #region constructor
        public UpdateTeamModel(ITeamService teamService, ICoachService coachService) // dependency injection
            {
            _coachService = coachService;
            _teamService = teamService; // parameter overført 
            }
            #endregion
            public List<Team> sTeams
            {
                get { return _teamService.GetAllTeamsAsync().Result; }
            }

            public async Task<IActionResult> OnGet(int ID)
            {

                Teams = sTeams;
            
                tEAM = await _teamService.GetTeamFromIdAsync(ID);
            
            Id = tEAM.Id;
                Name = tEAM.Name;
                MembershipType = tEAM.MembershipType;
                DayOfWeek = tEAM.DayOfWeek;
                TimeOfDay = tEAM.TimeOfDay;
                Length = tEAM.Length;
                MinMember = tEAM.AttendeeRange[0];
                MaxMember = tEAM.AttendeeRange[1];
                Description = tEAM.Description;

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
            int[] AttendeeRange = { MinMember, MaxMember };
            List<Models.Member> Attendees = new List<Models.Member>();
                Team team = new Team(Id, Name, MembershipType, await _coachService.GetCoachByIdAsync(TrainerId), DayOfWeek, TimeOfDay, Length, AttendeeRange, Attendees,Description);
            
                _teamService.UpdateTeamAsync(team, id);
                return RedirectToPage("ShowTeam");
            }
            public IActionResult OnPostDelete(int deleteTeamId)
            {

                return RedirectToPage("DeleteTeam", new { deleteTeamId });
            }
        
            public IActionResult OnPostUpdateAttendeeList(int AttendeeListTeamId)
            {

                return RedirectToPage("UpdateAttendeeList", new { AttendeeListTeamId });
            }
    }
    }


