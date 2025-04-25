using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Member
{
    public class CreateMemberModel : PageModel
    {
        private IMemberService _memberService;

        [BindProperty]
        public RegisterMemberViewModel RegisterModel { get; set; }

        public CreateMemberModel(IMemberService memberService)
        {
            _memberService = memberService;
        }


     
        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                bool success = await _memberService.CreateMemberAsync(RegisterModel.Member);

                if (success)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError("", "Could not create the member. Please try again.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the member: " + ex.Message);
                return Page();
            }
        }
    }
}
