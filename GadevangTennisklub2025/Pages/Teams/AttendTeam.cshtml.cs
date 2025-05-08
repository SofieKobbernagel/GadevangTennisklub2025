using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Services;

namespace GadevangTennisklub2025.Pages.Teams
{
    public class AttendTeamModel : PageModel
    {
            #region Instance Fields
            
            private TeamService _TeamServ;
            private MemberService _MemberServ;
            #endregion

            #region Properties
            public TeamService Teamrep { get; set; }

            public List<Models.Member> Members { get; set; }
            public List<Team> Teams { get; set; }
            #endregion

            #region Constructors
            public AttendTeamModel(TeamService teamService, MemberService memberService)
            {
                Console.WriteLine("AttendTeam.cshtml.cs is here");
                _TeamServ = teamService;
                _MemberServ = memberService;
                Teams = _TeamServ.GetAllTeamsAsync().Result;
                Members = _MemberServ.GetAllMembersAsync().Result;
                Console.WriteLine($" SelectedMember is {_TeamServ.SelectedMember}");
                if (_TeamServ.SelectedMember == null)
                {
                    _TeamServ.SelectedMember = (Models.Member)_MemberServ.GetMemberById(0).Result;
                    Console.WriteLine("(AttendedTeam.cshtml.cs)  SelectedMember was null, changed it to Kurt");
                }
            }

            #endregion

            #region Methods
            public List<Team> NonEnteredTeamsByMember(Models.Member member)
            {
               List<Team> list = new List<Team>();
               List<Team> attended = new List<Team>();
               attended=_TeamServ.GetAllAttendedTeamsAsync(member).Result;
               for(int i = 0; i < Teams.Count; i++)
               {
                if (attended.Contains(Teams[i]) == false)
                {
                    list.Add(Teams[i]);
                }
               }
               Console.WriteLine($"the first in the list of non entered Teams is: {list[0]}");
               return list;
            }

            public List<Models.Member> SortedMembers()
            {
                List<Models.Member> list = new List<Models.Member>();
                list = Members;
                list.Remove(_TeamServ.SelectedMember);
                list.Prepend(_TeamServ.SelectedMember);
                return list;
            }

            public void OnGet()
            {
                if (_TeamServ.SelectedMember == null)
                {
                _TeamServ.SelectedMember = Members[0];
                }
            }



            public IActionResult OnPostAttendTeam(int Id)
            {
                
                Console.WriteLine($"selectedMember is {_TeamServ.SelectedMember}");
            _TeamServ.GetTeamFromIdAsync(Id).Result.AttendTeam(_TeamServ.SelectedMember);

                return RedirectToPage("AttendTeam");
            }

            public IActionResult OnPostSelectMemberButton(int id)
            {

                _TeamServ.SelectedMember = (Models.Member)_MemberServ.GetMemberById(id).Result;
                Console.WriteLine($"Selected Member = {_TeamServ.SelectedMember}");
                if (_TeamServ.SelectedMember == null)
                {
                    Console.WriteLine("SelectedMember is NULL"); // Debugging: Log null issues
                }

                return Page();
            }
            #endregion
        }
    }

