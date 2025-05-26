using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.Intrinsics.X86;

namespace GadevangTennisklub2025.Pages.Member
{
    /// <summary>
    /// PageModel for admins til redigering af andre medlem i systemet.
    /// </summary>
    public class EditMemberModel : PageModel
    {
        private readonly IMemberService _memberService;
        private readonly IMembershipService _membershipService;


        public EditMemberModel(IMemberService memberService, IMembershipService membershipService)
        {
            _memberService = memberService;
            _membershipService = membershipService;
        }

        [BindProperty]
        public List<Membership> Memberships { get; set; } = new();
        [BindProperty]
        public Models.Member MemberObject { get; set; }

        // Profilbillede uploadet via formular (valgfrit)
        [BindProperty]
        public IFormFile? ProfileImage { get; set; }

        // Henter medlem og medlemskaber til redigeringssiden
        public async Task<IActionResult> OnGetAsync(int member_Id)
        {
            // Henter alle medlemskaber
            Memberships = await _membershipService.GetAllMembershipsAsync();
            try
            {
                // Henter medlem udra member_Id
                MemberObject = await _memberService.GetMemberById(member_Id);
                if (MemberObject == null)
                    // Hvis medlem ikke findes, redirect til forsiden
                    return RedirectToPage("Index");
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return RedirectToPage("Error");
            }
        }

        // Opdaterer medlem med data fra formularen
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Hent id på det medlem som er logget ind fra session, ellers redirect til login
                if (!int.TryParse(HttpContext.Session.GetString("Member_Id"), out int activeUserId))
                {
                    return RedirectToPage("Login");
                }
                // Hent det aktive medlem (den der er logget ind)
                var activeUser = await _memberService.GetMemberById(activeUserId);

                // Hent det medlem som skal redigeres for at bevare visse data hvis ikke opdateret
                var existingMember = await _memberService.GetMemberById(MemberObject.Member_Id);

                // passwor kan ikke ændres i formularen, der sikres at brugeren beholder eksisterende password
                if (string.IsNullOrEmpty(MemberObject.Password))
                {
                    MemberObject.Password = existingMember.Password;
                }

                // Valider ModelState - hvis ikke gyldigt, hent medlemskaber og vis siden igen
                if (!ModelState.IsValid)
                {
                    Memberships = await _membershipService.GetAllMembershipsAsync();
                    return Page();
                }

                // Håndter upload af profilbillede, hvis der er uploadet en fil
                if (ProfileImage != null)
                {
                    var uploadsFolder = Path.Combine("wwwroot", "Images", "ProfilePictures");
                    Directory.CreateDirectory(uploadsFolder);
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(stream);
                    }
                    MemberObject.ProfileImagePath = "/Images/ProfilePictures/" + fileName;
                }
                else
                {
                    // Bevar eksisterende profilbillede, hvis der ikke uploades nyt
                    MemberObject.ProfileImagePath = existingMember.ProfileImagePath;
                }

                // Bevar admin-status fra eksisterende medlem (undgå utilsigtede ændringer)
                MemberObject.IsAdmin = existingMember.IsAdmin;

                // Opdater medlem i databasen via service
                await _memberService.UpdateMemberAsync(MemberObject, MemberObject.Member_Id);
                TempData["SuccessMessage"] = "Brugeroplysninger er opdateret.";

                // Hvis den aktive bruger er admin og redigerer en anden bruger,
                // redirect til oversigt over medlemmer
                if (activeUser.IsAdmin && MemberObject.Member_Id != activeUser.Member_Id)
                {
                    return RedirectToPage("GetAllMembers");
                }
                //Ellers redirect til brugerens egen profil
                return RedirectToPage("MyProfile");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Der opstod en fejl under opdatering af bruger: " + ex.Message;
                return RedirectToPage("Error");
            }
        }
    }
}
