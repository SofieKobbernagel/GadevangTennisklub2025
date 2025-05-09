using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Member
{
    public class CreateMemberModel : PageModel
    {
        private IMemberService _memberService;
        private readonly IMembershipService _membershipService;

        [BindProperty]
        public List<Membership> Memberships { get; set; } = new();
        [BindProperty]
        public RegisterMemberViewModel RegisterModel { get; set; }

        [BindProperty]
        public IFormFile? ProfileImage { get; set; }

        public CreateMemberModel(IMemberService memberService, IMembershipService membershipService)
        {
            _memberService = memberService;
            _membershipService = membershipService;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            Memberships = await _membershipService.GetAllMembershipsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Memberships = await _membershipService.GetAllMembershipsAsync();
                    return Page();
                }

                bool isUnique = await _memberService.IsUsernameUnique(RegisterModel.Member.Username);
                if (!isUnique)
                {
                    ModelState.AddModelError("RegisterModel.Member.Username", "Brugernavnet er taget, v�lg venligst et andet.");
                    Memberships = await _membershipService.GetAllMembershipsAsync();
                    return Page();
                }

                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    // Generate a unique filename (so people don't overwrite each other�s pictures)
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);

                    // Build the path to /images/ProfilePictures/
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ProfilePictures", fileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(stream);
                    }

                    // Save the relative path to the database
                    RegisterModel.Member.ProfileImagePath = "/images/ProfilePictures/" + fileName;
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
