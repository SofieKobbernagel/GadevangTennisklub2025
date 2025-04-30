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

        public IRelationshipsServicesAsync relationshipsService { get; set; }

        [BindProperty]
        public Booking bo { get; set; }
        public List<SelectListItem> ints { get; set; }

        [BindProperty]
        public string me { get; set; }
        [BindProperty]
        public string Message { get; set; }

        public MakeBookingModel(IBookingServiceAsync IBSA, IMemberService IMS, IRelationshipsServicesAsync IRSA) 
        {
            bookingService = IBSA;
            memberService = IMS;
            relationshipsService = IRSA;
            ints= new List<SelectListItem>();
            Message = "";
        }
        public async Task OnGet()
        {

           await  LoadList();
        }
        async Task LoadList() 
        {
            foreach (var item in await memberService.GetAllMembersAsync())
            {
                ints.Add(new SelectListItem(item.Name, Convert.ToString(item.Member_Id)));
            }
            ints.Add(new SelectListItem("boldMaskine", "0"));
        }

        public async Task<IActionResult> OnPost() 
        {
            await LoadList();
            if (!bool.Parse(HttpContext.Session.GetString("IsAdmin")))
            {

                bo.End = bo.Start.AddHours(1);  
                
                
               
            }
            if (!await relationshipsService.MemberAvailible(int.Parse(me),bo.Start,bo.End)) 
            {
                Message = "medlem optaget";
                return null;
            }
            if (!await relationshipsService.CourtAvailible(bo.Court_Id, bo.Start, bo.End))
            {
                Message = "Bane optaget";
                return null;
            }
            if (!me.Equals("0"))
            {
                await memberService.SubtrackHour(int.Parse(me));
            }
           
            
            await bookingService.CreateBooking(bo);
            return RedirectToPage("/Index");
        }
    }
}
