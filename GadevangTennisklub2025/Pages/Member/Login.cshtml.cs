using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Member
{
    public class LoginModel : PageModel
    {
        private readonly IMemberService _memberService;

        // BindProperty binder form-data til LoginViewModel (brugernavn og password)
        [BindProperty]
        public LoginViewModel LoginMember { get; set; }

        // Besked til brugeren, fx fejl ved login
        public string Message { get; set; }

        public LoginModel(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // Gemmer en returnUrl i session, hvis brugeren kommer fra en anden side så de kan sendes tilbage efter login
        public void OnGet(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) &&
                !returnUrl.Contains("/Member/Login", StringComparison.OrdinalIgnoreCase))
            {
                HttpContext.Session.SetString("ReturnUrl", returnUrl);
            }
        }

        // Logout-metode der rydder session og sender brugeren til forsiden
        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }

        // POST-metode til login-forsøg
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
            // Hvis model ikke er valid, vis siden igen med fejl
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Verificer brugeren via service (synkron kald via .Result)
            Models.Member loginUser = _memberService.VerifyMember(LoginMember.Username, LoginMember.Password).Result;
            if (loginUser != null)
            {
                // Gem brugerdata i session
                HttpContext.Session.SetString("Member_Id", loginUser.Member_Id.ToString());
                HttpContext.Session.SetString("Email", loginUser.Email);
                HttpContext.Session.SetString("IsAdmin", loginUser.IsAdmin ? "true" : "false");

                // Hent og fjern returnUrl fra session (for at undgå redirect loop)
                string returnUrl = HttpContext.Session.GetString("ReturnUrl");
                HttpContext.Session.Remove("ReturnUrl"); //Fjerner "ReturnUrl" til at rense Cache.

                // Redirect til returnUrl hvis det er gyldigt, ellers til forsiden
                if (!string.IsNullOrEmpty(returnUrl) && !returnUrl.Contains("/Member/Login", StringComparison.OrdinalIgnoreCase))
                {
                    return Redirect(returnUrl);
                }

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

