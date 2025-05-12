using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
namespace GadevangTennisklub2025.Pages.Teams
{
    public class AttendTeamModel : PageModel
    {
        #region Instance Fields

        private readonly ITeamService _TeamServ;
        private readonly IMemberService _MemberServ;

        #endregion

        #region Properties

        public Models.Member SelectedMember { get; set; }
        public List<Models.Member> Members { get; set; }
        public List<Team> Teams { get; set; }


        #endregion

        #region Constructors

        // Use dependency injection for ITeamService and IMemberService
        public AttendTeamModel(ITeamService teamService, IMemberService memberService)
        {
            _TeamServ = teamService;
            _MemberServ = memberService;
            Teams = _TeamServ.GetAllTeamsAsync().Result;
            Members = _MemberServ.GetAllMembersAsync().Result;
           
           

        }

        #endregion

        #region Methods

        public async Task<List<Team>> NonEnteredTeamsByMember(Models.Member member)
        {
            List<Team> list = new List<Team>();
            List<Team> attended = await _TeamServ.GetAllAttendedTeamsAsync(member.Member_Id);
            string attId = "";
            foreach(Team team in attended)
            {
                attId += "," + team.Id;
            }


            for (int i = 0; i < Teams.Count; i++)
            {

                if (attId.Contains(""+Teams[i].Id)==false)
                {
                    list.Add(Teams[i]);
                }
            }
            return list;
        }

        public List<Models.Member> SortedMembers()
        {
            List<Models.Member> list = new List<Models.Member>();
            list = Members;
            list.Remove(SelectedMember);
            list.Prepend(SelectedMember);
            return list;
        }

        public IActionResult OnGet()
        {
            Teams = _TeamServ.GetAllTeamsAsync().Result;
            Members = _MemberServ.GetAllMembersAsync().Result;

                SelectedMember = _TeamServ.MemberById(int.Parse(HttpContext.Session.GetString("Member_Id")));
                Console.WriteLine($"SelectedMember is {SelectedMember.Name}");
            return Page();
        }


        public async Task<IActionResult> OnPostAttendTeam(int id, int memberId)
        {
            var member = await _MemberServ.GetMemberById(memberId);
            var team = await _TeamServ.GetTeamFromIdAsync(id);
            await _TeamServ.AttendTeamAsync(team, member);
            return RedirectToPage("ShowTeam");
        }

        public IActionResult OnPostSelectMemberButton(int id)
        {
            SelectedMember = (Models.Member)_MemberServ.GetMemberById(id).Result;
            //Console.WriteLine($"Selected Member = {SelectedMember.Name}");
            if (SelectedMember == null)
            {
                Console.WriteLine("SelectedMember is NULL");
            }

            return Page();
        }

        #endregion
    }
}