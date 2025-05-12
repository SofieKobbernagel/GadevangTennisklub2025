using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Member
{
    public class MyRegistrationsModel : PageModel
    {

        private readonly IMemberService _memberService;


        public MyRegistrationsModel(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [BindProperty]
        public GadevangTennisklub2025.Models.Member Member { get; set; }

        [BindProperty]
        public IFormFile ProfileImage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!int.TryParse(HttpContext.Session.GetString("Member_Id"), out int member_Id))
                return RedirectToPage("/Login");

            Member = await _memberService.GetMemberById(member_Id);
            return Page();
        }


    }

}

