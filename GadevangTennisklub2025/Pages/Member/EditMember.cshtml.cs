using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Member
{
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
        [BindProperty]
        public RegisterMemberViewModel RegisterModel { get; set; }
        [BindProperty]
        public IFormFile? ProfileImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int member_Id)
        {
            Memberships = await _membershipService.GetAllMembershipsAsync();
            try
            {
                MemberObject = await _memberService.GetMemberById(member_Id);
                if (MemberObject == null)
                    return RedirectToPage("Index");
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
            var existingMember = await _memberService.GetMemberById(MemberObject.Member_Id);

            if (string.IsNullOrEmpty(MemberObject.Password))
            {
                MemberObject.Password = existingMember.Password;
            }

            if (!ModelState.IsValid)
            {
                Memberships = await _membershipService.GetAllMembershipsAsync();
                // Debugging: Log model state errors
                foreach (var state in ModelState)
                {
                    Console.WriteLine($"Key: {state.Key}");
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

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
                // Behold eksisterende billede
                MemberObject.ProfileImagePath = existingMember.ProfileImagePath;
            }



            await _memberService.UpdateMemberAsync(MemberObject, MemberObject.Member_Id);
            TempData["SuccessMessage"] = "Brugeroplysninger er opdateret.";
            return RedirectToPage("MyProfile");




        }
    }
}
