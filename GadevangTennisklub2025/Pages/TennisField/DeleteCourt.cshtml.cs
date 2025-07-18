using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.TennisField
{
    public class DeleteCourtModel : PageModel
    {
        private IBookingServiceAsync _bookingService;
        private ICourtService _courtService;
        [BindProperty]
        public Models.TennisField Court { get; set; }
        [BindProperty] public bool Confirm { get; set; }
        public string MessageError { get; set; }
        public DeleteCourtModel(ICourtService courtService, IBookingServiceAsync bookingService)
        {
            _bookingService = bookingService;
            _courtService = courtService;
        }
        public async Task<IActionResult> OnGetAsync(int deleteId)
        {
            Court = await _courtService.GetCourtFromIdAsync(deleteId);
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "true")
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Confirm == false)
            {
                MessageError = $"Husk at klikke p� konfirmation";
                return Page();
            }

            var allBookings = await _bookingService.GetAllBookings();

            bool courtHasBookings = allBookings.Any(b => b.Court_Id == Court.CourtId);

            if (courtHasBookings)
            {
                MessageError = $"Kan ikke slette banen, da den har eksisterende bookinger.";
                return Page();
            }

            await _courtService.DeleteCourtAsync(Court.CourtId);
            return RedirectToPage("ShowCourts");
        }
    }
}
