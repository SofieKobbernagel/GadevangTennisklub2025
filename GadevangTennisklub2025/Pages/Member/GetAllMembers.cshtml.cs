using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Pages.Member
{
    // PageModel til visning af alle medlemmer i klubben.
    // Kun i brug for administratorer, da de har adgang til alle medlemmer.
    public class GetAllMembersModel : PageModel
    {
        private IMemberService _memberService;
        public bool IsAdmin { get; set; }
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
            // Tjek om brugeren er admin, ellers redirect til forsiden
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "true")
            {
                return RedirectToPage("/Index");
            }
            IsAdmin = true;
            try
            {
                // Hent alle medlemmer fra service
                Members = await _memberService.GetAllMembersAsync();

                if (Members == null)
                    return RedirectToPage("Index");

                // Filtrering: hvis FilterCriteria er angivet, filtrer medlemmerne på navn, email eller telefon
                if (!string.IsNullOrWhiteSpace(FilterCriteria))
                {
                    string criteria = FilterCriteria.ToLower();
                    Members = Members.Where(m =>
                        (!string.IsNullOrEmpty(m.Name) && m.Name.ToLower().Contains(criteria)) ||
                        (!string.IsNullOrEmpty(m.Email) && m.Email.ToLower().Contains(criteria)) ||
                        (!string.IsNullOrEmpty(m.Phone) && m.Phone.ToLower().Contains(criteria))
                    ).ToList();
                }

                // Sortering: hvis SortBy er angivet, sortér efter det valgte felt i angivet retning
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
                // Returner siden med filtreret og sorteret liste
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
