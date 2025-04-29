using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Member
{
    public class GetMemberModel : PageModel
    {
        private readonly IMemberService _memberService;
     

        public GetMemberModel(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [BindProperty]
        public GadevangTennisklub2025.Models.Member Member { get; set; }


        public async Task<IActionResult> OnGetAsync(int member_Id)
        {
            Member = await _memberService.GetMemberById(member_Id);

            if (Member == null)
            {
                return RedirectToPage("/Error");
            }

            return Page();
        }

    }
}
