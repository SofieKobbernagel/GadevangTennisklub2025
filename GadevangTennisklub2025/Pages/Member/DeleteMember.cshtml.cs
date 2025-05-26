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

        [BindProperty]
        public Models.Member LoggedInUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int member_Id)
        {
            try
            {
                //Hvis man ikke er logget ind blive man sendt til logind siden
                if (!int.TryParse(HttpContext.Session.GetString("Member_Id"), out int activeUserId))
                {
                    return RedirectToPage("Login");
                }
                // Hent data for både logget ind medlem og medlemmet der skal slettes
                // i tilfælde af at aministratoren skal slette en andens bruger
                LoggedInUser = await _memberService.GetMemberById(activeUserId);
                Member = await _memberService.GetMemberById(member_Id);
                if (Member == null)
                    // Hvis medlemmet ikke findes, redirect til egen profil
                    return RedirectToPage("MyProfile");
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return RedirectToPage("Error");
            }
        }
        //Håndter sletning af medlem
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                //Hvis man ikke er logget ind sendes man til logind side
                if (!int.TryParse(HttpContext.Session.GetString("Member_Id"), out int activeUserId))
                {
                    return RedirectToPage("Login");
                }
              
                LoggedInUser = await _memberService.GetMemberById(activeUserId);
                // Tjekker at Member er sat og har et gyldigt ID
                if (Member == null || Member.Member_Id == null)
                {
                    ModelState.AddModelError("", "ID mangler.");
                    return Page();
                }

                // Forsøg at slette medlemmet via service
                Models.Member deletedMember = await _memberService.DeleteMemberAsync(Member.Member_Id);
                TempData["SuccessMessage"] = "Din profil er blevet slettet";
                // Hvis det aktive medlem (den som er logged ind) er admin og sletter en anden bruger,
                // redirect til oversigten over medlemmer
                if (LoggedInUser.IsAdmin && Member.Member_Id != LoggedInUser.Member_Id)
                {
                    return RedirectToPage("GetAllMembers");
                }
                // Hvis sletning ikke lykkedes, redirect til startsiden
                if (deletedMember == null)
                    return RedirectToPage("/Index");
                else
                {
                    // Slet session ved sletning af egen profil (log ud)
                    HttpContext.Session.Clear();
                }

                // Redirect til startsiden efter sletning
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
