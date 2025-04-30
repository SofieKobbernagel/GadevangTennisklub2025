using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Pages.Member
{
    public class GetAllMembersModel : PageModel
    {
        private IMemberService _memberService;
        public bool IsAdmin { get; set; } = false;


        public List<Models.Member> Members { get; set; }

        public GetAllMembersModel(IMemberService userService)
        {
            _memberService = userService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "true")
            {
                return RedirectToPage("/Index");
            }

            try
            {
                Members = await _memberService.GetAllMembersAsync();
                if (Members == null)
                    return RedirectToPage("Index");
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return RedirectToPage("Error");
            }
        }
    }
}
