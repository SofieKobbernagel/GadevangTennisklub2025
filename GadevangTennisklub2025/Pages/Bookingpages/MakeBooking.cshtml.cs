using Azure.Core;
using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GadevangTennisklub2025.Pages.Bookingpages
{
    public class MakeBookingModel : PageModel
    {
        public IBookingServiceAsync bookingService { get; set; }
        public IMemberService memberService { get; set; }

        [BindProperty]
        public Booking bo { get; set; }

        public List<SelectListItem> ints { get; set; }

        [BindProperty]
        public string me { get; set; }

        public MakeBookingModel(IBookingServiceAsync IBSA, IMemberService IMS) 
        {
            bookingService = IBSA;
            memberService = IMS;
            ints= new List<SelectListItem>();
        }
        public async Task OnGet()
        {

            foreach (var item in await memberService.GetAllMembersAsync()) 
            {
                ints.Add(new SelectListItem(item.Name, Convert.ToString(item.Member_Id)) );
            }
            ints.Add(new SelectListItem("boldMaskine", "0"));
        }

        public async Task<IActionResult> OnPost() 
        {
            if (!me.Equals("0"))
            {
                await memberService.GetMemberById(int.Parse(me));
            }
            Console.WriteLine(bool.Parse(HttpContext.Session.GetString("IsAdmin")));
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
