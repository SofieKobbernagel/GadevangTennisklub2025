using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Member
{
    public class MyProfileModel : PageModel
    {
        private readonly IMemberService _memberService;
        private readonly IWebHostEnvironment _environment;

        public MyProfileModel(IMemberService memberService, IWebHostEnvironment environment)
        {
            _memberService = memberService;
            _environment = environment;
        }

        [BindProperty]
        public GadevangTennisklub2025.Models.Member Member { get; set; }

        [BindProperty]
        public IFormFile UploadImage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!int.TryParse(HttpContext.Session.GetString("Member_Id"), out int member_Id))
                return RedirectToPage("/Login");

            Member = await _memberService.GetMemberById(member_Id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!int.TryParse(HttpContext.Session.GetString("Member_Id"), out int member_Id))
                return RedirectToPage("/Login");

            var existingMember = await _memberService.GetMemberById(member_Id);

            if (UploadImage != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{UploadImage.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadImage.CopyToAsync(fileStream);
                }

                existingMember.ProfileImagePath = "/uploads/" + fileName;
            }

            await _memberService.UpdateMemberAsync(existingMember, existingMember.Member_Id);
            return RedirectToPage("/Users/MyProfile");
        }

    }

}

