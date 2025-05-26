using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GadevangTennisklub2025.Models.ViewModels;

namespace GadevangTennisklub2025.Pages.Member
{
    public class MyProfileModel : PageModel
    {
        private readonly IMemberService _memberService;
        private readonly IWebHostEnvironment _environment;
        private readonly IRelationshipsServicesAsync _relService;
        private readonly ICourtService _courtService;
        private readonly IBookingServiceAsync _bookingService;

        public MyProfileModel(IMemberService memberService, IWebHostEnvironment environment, IRelationshipsServicesAsync relService, ICourtService courtService, IBookingServiceAsync bookingService)
        {
            _memberService = memberService;
            _environment = environment;
            _relService = relService;
            _courtService = courtService;
            _bookingService = bookingService;
        }

        [BindProperty]
        public GadevangTennisklub2025.Models.Member Member { get; set; }

        [BindProperty]
        public List<Booking?> Bookings { get; set; } = new List<Booking?>();

        [BindProperty]
        public List<Event?> Events { get; set; } = new List<Event?>();

        // ViewModels til at samle booking info med bane- og partnernavne
        [BindProperty]
        public List<BookingViewModel> BookingsWithCourtsAndPartners { get; set; } = new();

        // ViewModels til at samle event info med titel og dato
        [BindProperty]
        public List<EventViewModel> EventWithTitleAndDate { get; set; } = new();

        [BindProperty]
        public IFormFile ProfileImage { get; set; }

        // Henter data til profilsiden
        public async Task<IActionResult> OnGetAsync()
        {
            // Hent brugerens ID fra session, ellers send til login
            if (!int.TryParse(HttpContext.Session.GetString("Member_Id"), out int member_Id))
                return RedirectToPage("/Login");

            // Hent medlem, bookings og events for den aktuelle bruger
            Member = await _memberService.GetMemberById(member_Id);
            Bookings = await _relService.GetBookingsByMemberId(member_Id);
            Events = await _relService.GetEventsByMemberId(member_Id);

            // For hver booking hent bane- og partnernavn, saml i viewmodel
            foreach (var booking in Bookings)
            {
                string courtName = (await _courtService.GetCourtFromIdAsync(booking.Court_Id)).Name;
                string partnerName = await _relService.GetBookingPartnerName(member_Id, booking.Id);

                BookingsWithCourtsAndPartners.Add(new BookingViewModel
                {
                    Booking = booking,
                    CourtName = courtName,
                    PartnerName = partnerName
                });
            }

            // For hver event lav et viewmodel med titel og dato
            foreach (var e in Events)
            {
                string title = e.Title;
                DateTime dato = e.Date;
                int id = e.Id;

                EventWithTitleAndDate.Add(new EventViewModel
                {
                    Title = title,
                    DateAndTime = dato,
                    EventId = id
                });
            }
            return Page();
        }

        // Opdater profil inkl. evt. upload af profilbillede
        public async Task<IActionResult> OnPostAsync()
        {
            if (!int.TryParse(HttpContext.Session.GetString("Member_Id"), out int member_Id))
                return RedirectToPage("/Login");

            var existingMember = await _memberService.GetMemberById(member_Id);
            // Hvis der er uploadet et billede, gem det i /uploads-mappen og opdater stien
            if (ProfileImage != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{ProfileImage.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(fileStream);
                }

                existingMember.ProfileImagePath = "/uploads/" + fileName;
            }
            // Opdater medlem i databasen
            await _memberService.UpdateMemberAsync(existingMember, existingMember.Member_Id);
            // Redirect til profilsiden igen
            return RedirectToPage("/Users/MyProfile");
        }

        // Slet en booking tilknyttet medlemmet
        public async Task<IActionResult> OnPostDeleteBookingAsync(int booking_Id)
        {
            int memberId = int.Parse(HttpContext.Session.GetString("Member_Id"));
            List<Booking> list = await _bookingService.GetBookingsByUser(memberId);
            Booking bookingToDelete = list.Find(i=>i.Id == booking_Id);
            await _bookingService.DeleteBooking(bookingToDelete);
            return RedirectToPage();
        }

        // Afmeld medlem fra et event
        public async Task<IActionResult> OnPostRemoveFromEventAsync(int eventId)
        {
            int memberId = int.Parse(HttpContext.Session.GetString("Member_Id"));
            await _relService.SignOffEvent(eventId, memberId);
            TempData["SuccessMessage"] = "Du er nu afmeldt eventet.";
            return RedirectToPage();
        }

    }

}

