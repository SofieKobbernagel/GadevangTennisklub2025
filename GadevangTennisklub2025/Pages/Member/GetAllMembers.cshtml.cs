using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Pages.Member
{
    public class GetAllMembersModel : PageModel
    {
        private IMemberService _memberService;
        public bool IsAdmin { get; set; } = false;
        [BindProperty(SupportsGet = true)] public string FilterCriteria { get; set; }
        [BindProperty(SupportsGet = true)] public string SortBy { get; set; }
        [BindProperty(SupportsGet = true)] public string SortOrder { get; set; }


        public List<Models.Member> Members { get; set; }

        public GetAllMembersModel(IMemberService userService)
        {
            _memberService = userService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "true")
            {
                return RedirectToPage("/Index");
            }

            try
            {
                Members = await _memberService.GetAllMembersAsync();

                if (Members == null)
                    return RedirectToPage("Index");


                if (!string.IsNullOrWhiteSpace(FilterCriteria))
                {
                    string criteria = FilterCriteria.ToLower();
                    Members = Members.Where(m =>
                        (!string.IsNullOrEmpty(m.Name) && m.Name.ToLower().Contains(criteria)) ||
                        (!string.IsNullOrEmpty(m.Email) && m.Email.ToLower().Contains(criteria)) ||
                        (!string.IsNullOrEmpty(m.Phone) && m.Phone.ToLower().Contains(criteria))
                    ).ToList();
                }


                if (!string.IsNullOrEmpty(SortBy))
                {
                    Func<Models.Member, object> keySelector = SortBy switch
                    {
                        "Name" => m => m.Name,
                        "Email" => m => m.Email,
                        "Phone" => m => m.Phone,
                        "Age" => m => m.Age
                    };

                    if (SortOrder == "Descending")
                        Members = Members.OrderByDescending(keySelector).ToList();
                    else
                        Members = Members.OrderBy(keySelector).ToList();
                }

                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return RedirectToPage("Error");
            }
        }

    }
}
