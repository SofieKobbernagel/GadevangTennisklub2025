using Azure.Core;
using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Bookingpages
{
    public class MakeBookingModel : PageModel
    {
        public IBookingServiceAsync bookingService { get; set; }
        public IMemberService memberService { get; set; }

        [BindProperty]
        public Booking bo { get; set; }

        public List<int> i=new List<int>();

        [BindProperty]
        public MemberPlaceHolder me { get; set; }

        public MakeBookingModel(IBookingServiceAsync IBSA, IMemberService IMS) 
        {
            bookingService = IBSA;
            memberService = IMS;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost() 
        {
            if (!bool.Parse(HttpContext.Session.GetString("IsAdmin")))
            {
                bo.End = bo.Start;
                bo.End.AddHours(1);
            }
            
            await bookingService.CreateBooking(bo);
            return RedirectToPage("Index");
        }
    }
}
