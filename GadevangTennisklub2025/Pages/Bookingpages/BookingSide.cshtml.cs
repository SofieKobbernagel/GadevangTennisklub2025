using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GadevangTennisklub2025.Pages.Bookingpages
{
    public class BookingSideModel : PageModel
    {
        public IBookingServiceAsync bookingService { get; set; }
        public IMemberService memberService { get; set; }
        public IRelationshipsServicesAsync relationshipsService { get; set; }
        public IEventServiceAsync eventService { get; set; }
        public ITeamService teamService { get; set; }
        [BindProperty]
        public Booking b {get;set;}
        [BindProperty]
        public int type { get; set; }
        public List<SelectListItem> ints { get; set; }
        public List<SelectListItem> intse { get; set; }
        public List<SelectListItem> intst { get; set; }
        [BindProperty]
        public string Message{ get; set; }
        [BindProperty]
        public string me { get; set; }
        [BindProperty]
        public string ev { get; set; }
        [BindProperty]
        public string te { get; set; }

        public BookingSideModel(IBookingServiceAsync IBSA, IMemberService IMS, IRelationshipsServicesAsync IRSA, ITeamService ITS, IEventServiceAsync IESA) 
        {
            ints = new List<SelectListItem>();
            intse = new List<SelectListItem>();
            intst = new List<SelectListItem>();
            bookingService = IBSA;
            memberService = IMS;
            relationshipsService = IRSA;
            teamService = ITS;
            eventService = IESA;
        }

        public async Task OnGet(DateTime date, int hour, int Court)
        {
           
            DateTime t = new DateTime(date.Year,date.Month,date.Day,hour,0,0);
            b = new Booking(1, t, t.AddHours(1), Court, null, null);
            await LoadList();
        }
        async Task LoadList()
        {
            ints.Add(new SelectListItem("vælg spiller","-1"));
            ints.Add(new SelectListItem("boldMaskine", "0"));
            foreach (var item in await memberService.GetAllMembersAsync())
            {
                if(item.Member_Id!=int.Parse(HttpContext.Session.GetString("Member_Id")))ints.Add(new SelectListItem(item.Name, Convert.ToString(item.Member_Id)));
            }
            intse.Add(new SelectListItem("vælg Event", "-1"));
            foreach (var item in await eventService.GetEventsAsync())
            {
                intse.Add(new SelectListItem(item.Title, Convert.ToString(item.Id)));
            }
            intst.Add(new SelectListItem("vælg Team", "-1"));
            foreach (var item in await teamService.GetAllTeamsAsync())
            {
                intst.Add(new SelectListItem(item.Name, Convert.ToString(item.Id)));
            }

        }

        public async Task<IActionResult> OnPostAdmin() 
        {
            await LoadList();
            if (type == 0 ) 
            {
                if (int.Parse(me) == -1) 
                {
                    Message = "Vælg en Modstander";
                    return null;
                }
                if (! await relationshipsService.MemberAvailible(int.Parse(me),b.Start,b.End) || bookingService.GetBookingsByUser(int.Parse(me)).Result.Count ==4) 
                {
                   
                    Message = bookingService.GetBookingsByUser(int.Parse(me)).Result.Count == 4? "Medlem har 4 timer":"Medlem er optager";
                    return null;
                }
                if (!await relationshipsService.MemberAvailible(int.Parse(HttpContext.Session.GetString("Member_Id")), b.Start, b.End) || bookingService.GetBookingsByUser(int.Parse(HttpContext.Session.GetString("Member_Id"))).Result.Count == 4)
                {
                    Message = bookingService.GetBookingsByUser(int.Parse(HttpContext.Session.GetString("Member_Id"))).Result.Count == 4 ? "du har 4 timer" : "du er optager";
                    return null;
                }
                await bookingService.CreateBooking(b);
                await relationshipsService.BookingMemberRelation(int.Parse(me),bookingService.GetAllBookings().Result.Last().Id);
                await relationshipsService.BookingMemberRelation(int.Parse(HttpContext.Session.GetString("Member_Id")), bookingService.GetAllBookings().Result.Last().Id);
                return RedirectToPage("MakeBooking2");   
            }
            if (type == 1)
            {
                if (int.Parse(ev) == -1)
                {
                    Message = "Vælg et event";
                    return null;
                }
                await bookingService.CreateEventBooking(b,int.Parse(ev));
                return RedirectToPage("MakeBooking2");
            }
            if (type == 2)
            {
                if (int.Parse(te) == -1)
                {
                    Message = "Vælg et hold";
                    return null;
                }
                await bookingService.CreateTeamBooking(b, int.Parse(te));
                return RedirectToPage("MakeBooking2");
            }


            return null;
        }

        public async Task<IActionResult> OnPostUser()
        {
            await LoadList();
                if (int.Parse(me) == -1)
                {
                    Message = "Vælg en Modstander";
                    return null;
                }
                if (!await relationshipsService.MemberAvailible(int.Parse(me), b.Start, b.End) || bookingService.GetBookingsByUser(int.Parse(me)).Result.Count == 4)
                {
                    Message = "Medlem ikke tilgængelig";
                    return null;
                }
                if (!await relationshipsService.MemberAvailible(int.Parse(HttpContext.Session.GetString("Member_Id")), b.Start, b.End) || bookingService.GetBookingsByUser(int.Parse(HttpContext.Session.GetString("Member_Id"))).Result.Count == 4)
                {
                    Message = "du er ikke tilgængelig";
                    return null;
                }
                await bookingService.CreateBooking(b);
                await relationshipsService.BookingMemberRelation(int.Parse(me), bookingService.GetAllBookings().Result.Last().Id);
                await relationshipsService.BookingMemberRelation(int.Parse(HttpContext.Session.GetString("Member_Id")), bookingService.GetAllBookings().Result.Last().Id);
                return RedirectToPage("MakeBooking2");
            
        }

    }
}
