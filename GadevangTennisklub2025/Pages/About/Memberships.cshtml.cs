using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.About
{
    public class MembershipsModel : PageModel
    {
        private readonly IMembershipService _membershipService;

        [BindProperty]
        public List<Membership> Memberships { get; set; } = new();
        public MembershipsModel(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Memberships = await _membershipService.GetAllMembershipsAsync();
            return Page();
        }
    }
}
