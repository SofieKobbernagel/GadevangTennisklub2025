using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Services;
namespace GadevangTennisklub2025.Pages.Teams
{
    public class DeleteTeamModel : PageModel
    {
        public Team team { get; set; }
        private ITeamService teamService { get; set; }

        public DeleteTeamModel(ITeamService TEamService)
        {
            teamService = TEamService;
        }

        public async Task<IActionResult> OnGet(int deleteTeamId)
        {
            team = await teamService.GetTeamFromIdAsync(deleteTeamId);

            if (team == null)
            {
                TempData["ErrorMessage"] = "Team not found!";
                return RedirectToPage("ShowTeam");
            }

            return Page();
        }
        public async Task<IActionResult> OnPostDelete(int ID)
        {
            await teamService.DeleteTeamAsync(ID);
            TempData["SuccessMessage"] = "Holdet er nu slettet fra systemet!";
            return RedirectToPage("ShowTeam");
        }
        public IActionResult OnPostCancel()
       {
            return RedirectToPage("ShowTeam");
       }
    }
}
