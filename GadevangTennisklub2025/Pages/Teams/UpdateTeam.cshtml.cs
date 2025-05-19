using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
namespace GadevangTennisklub2025.Pages.Teams
{
    public class UpdateTeamModel : PageModel
    {
        private readonly ITeamService _teamService;
        private readonly ICoachService _coachService;
        private readonly IMembershipService _membershipService;



        [BindProperty] public int Id { get; set; }
        [BindProperty] public string Name { get; set; }
        [BindProperty] public string MembershipType { get; set; }
        [BindProperty] public int DayOfWeek { get; set; }
        [BindProperty] public TimeOnly TimeOfDay { get; set; }
        [BindProperty] public double Length { get; set; }
        [BindProperty] public int MinMember { get; set; }
        [BindProperty] public int MaxMember { get; set; }
        [BindProperty] public string Description { get; set; }
        [BindProperty] public int? TrainerId { get; set; }
        [BindProperty] public Coach? Coach { get; set; }
        public List<SelectListItem> TrainerOptions { get; set; }
        public List<SelectListItem> MembershipOptions { get; set; }

        public UpdateTeamModel(ITeamService teamService, ICoachService coachService, IMembershipService membershipService)
        {
            _teamService = teamService;
            _coachService = coachService;
            _membershipService = membershipService;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            var team = await _teamService.GetTeamFromIdAsync(id);
            if (team == null) return NotFound();

            Id = team.Id;
            Name = team.Name;
            MembershipType = team.MembershipType;
            DayOfWeek = team.DayOfWeek;
            TimeOfDay = team.TimeOfDay;
            Length = team.Length;
            MinMember = team.AttendeeRange[0];
            MaxMember = team.AttendeeRange[1];
            Description = team.Description;
            Coach? TempTrainer = await _coachService.GetCoachByTeamIdAsync(Id);
            TrainerId = (TempTrainer==null?null:TempTrainer.Coach_Id);
            Coach = team.Trainer;

            var coaches = await _coachService.GetAllCoachesAsync();
            TrainerOptions = coaches.Select(c => new SelectListItem
            {
                Value = c.Coach_Id.ToString(),
                Text = c.Name
            }).ToList();

            var memberships = await _membershipService.GetAllMembershipsAsync();
            MembershipOptions = memberships.Select(m => new SelectListItem
            {
                Value = m.MembershipType,
                Text = m.MembershipType
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (TrainerId == null)
            {
                ModelState.AddModelError(string.Empty, "A coach must be selected.");
                return Page();
            }

            var selectedCoach = await _coachService.GetCoachByIdAsync(TrainerId.Value);
            if (selectedCoach == null)
            {
                ModelState.AddModelError(string.Empty, "Selected coach was not found.");
                return Page();
            }

            var attendees = new List<Models.Member>(); // Optional: Fetch if needed
            var attendeeRange = new[] { MinMember, MaxMember };
            

            var updatedTeam = new Team(Id, Name, MembershipType, selectedCoach, DayOfWeek, TimeOfDay, Length, attendeeRange, attendees, Description);
            await _teamService.UpdateTeamAsync(updatedTeam, Id);

            return RedirectToPage("ShowTeam");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await _teamService.DeleteTeamAsync(Id);
            return RedirectToPage("ShowTeam");
        }

        public IActionResult OnPostUpdateAttendeeListId(int AttendeeListTeamId)
        {
            return RedirectToPage("UpdateAttendeeList", new { AttendeeListTeamId });
        }

    }
}
