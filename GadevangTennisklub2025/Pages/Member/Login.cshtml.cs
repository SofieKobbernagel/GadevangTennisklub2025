using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Member
{
    public class LoginModel : PageModel
    {
        private readonly IMemberService _memberService;

        [BindProperty]
        public LoginViewModel LoginMember { get; set; }

        public string Message { get; set; }

        public LoginModel(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }

        public IActionResult OnPost()
        {
            foreach (var kvp in ModelState)
            {
                Console.WriteLine($"Key: {kvp.Key}, IsValid: {kvp.Value.ValidationState}");
                foreach (var error in kvp.Value.Errors)
                {
                    Console.WriteLine($"  Error: {error.ErrorMessage}");
                }
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var loginUser = _memberService.VerifyMember(LoginMember.Username, LoginMember.Password);
            if (loginUser != null)
            {
                HttpContext.Session.SetString("Member_Id", loginUser.Member_Id.ToString());
                HttpContext.Session.SetString("Email", loginUser.Email);
                HttpContext.Session.SetString("IsAdmin", loginUser.IsAdmin ? "true" : "false");

                return RedirectToPage("/Index");
            }
            else
            {
                Message = "Ugyldig brugernavn eller kodeord";
                return Page();
            }
        }
    }
}

