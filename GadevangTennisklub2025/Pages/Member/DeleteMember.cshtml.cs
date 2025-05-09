using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Member
{
    public class DeleteMemberModel : PageModel
    {
        private IMemberService _memberService;
        public DeleteMemberModel(IMemberService memberService)
        {
            _memberService = memberService;
        }
        [BindProperty]
        public Models.Member Member { get; set; }

        public async Task<IActionResult> OnGetAsync(int member_Id)
        {
            try
            {
                Member = await _memberService.GetMemberById(member_Id);
                if (Member == null)
                    return RedirectToPage("MyProfile");
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return RedirectToPage("Error");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (Member == null || Member.Member_Id == null)
                {
                    ModelState.AddModelError("", "ID mangler.");
                    return Page();
                }

                Models.Member deletedMember = await _memberService.DeleteMemberAsync(Member.Member_Id);
                TempData["SuccessMessage"] = "Din profil er blevet slettet";
                if (deletedMember == null)
                    return RedirectToPage("/Index");
                else
                {
                    HttpContext.Session.Clear();
                }

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Der skete en fejl da brugeren skulle slettes: " + ex.Message);
                return Page();
            }
        }
    }
}
