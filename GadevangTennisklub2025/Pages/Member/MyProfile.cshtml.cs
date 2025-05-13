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

        public MyProfileModel(IMemberService memberService, IWebHostEnvironment environment, IRelationshipsServicesAsync relService)
        {
            _memberService = memberService;
            _environment = environment;
            _relService = relService;
        }

        [BindProperty]
        public GadevangTennisklub2025.Models.Member Member { get; set; }

        [BindProperty]
        public List<Booking?> Bookings { get; set; } = new List<Booking?>();

        [BindProperty]
        public List<Event?> Events { get; set; } = new List<Event?>();

        [BindProperty]
        public List<BookingViewModel> BookingsWithCourtsAndPartners { get; set; } = new();

        [BindProperty]
        public List<EventViewModel> EventWithTitleAndDate { get; set; } = new();

        [BindProperty]
        public IFormFile ProfileImage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!int.TryParse(HttpContext.Session.GetString("Member_Id"), out int member_Id))
                return RedirectToPage("/Login");

            Member = await _memberService.GetMemberById(member_Id);
            Bookings = await _relService.GetBookingsByMemberId(member_Id);
            Events = await _relService.GetEventsByMemberId(member_Id);

            foreach (var booking in Bookings)
            {
                string courtName = (await _relService.GetTennisFieldById(booking.Court_Id)).Name;
                string partnerName = await _relService.GetBookingPartnerName(member_Id, booking.Id);

                BookingsWithCourtsAndPartners.Add(new BookingViewModel
                {
                    Booking = booking,
                    CourtName = courtName,
                    PartnerName = partnerName
                });
            }
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!int.TryParse(HttpContext.Session.GetString("Member_Id"), out int member_Id))
                return RedirectToPage("/Login");

            var existingMember = await _memberService.GetMemberById(member_Id);

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

            await _memberService.UpdateMemberAsync(existingMember, existingMember.Member_Id);
            return RedirectToPage("/Users/MyProfile");
        }

    }

}

