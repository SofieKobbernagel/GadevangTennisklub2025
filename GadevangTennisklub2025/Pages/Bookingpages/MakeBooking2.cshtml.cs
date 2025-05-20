 using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Globalization;

namespace GadevangTennisklub2025.Pages.Bookingpages
{
    public class MakeBooking2Model : PageModel
    {
        public IBookingServiceAsync bookingService { get; set; }
        public IRelationshipsServicesAsync relationshipsServices { get; set; } 
        public IMemberService memberService { get; set; }
        public Dictionary<int, Dictionary<int,Dictionary<int,int>>> BookingType { get; set; }
        public Dictionary<int, Dictionary<int, Dictionary<int, string[]>>> Players { get; set; }
        public  int weekFromNow { get; set; }

        public MakeBooking2Model(IBookingServiceAsync IBSA ,IRelationshipsServicesAsync IRSA, IMemberService IMS) 
        {
            memberService = IMS;
            relationshipsServices = IRSA;
            bookingService = IBSA;
            BookingType=new Dictionary<int,Dictionary<int, Dictionary<int, int>>>();
            Players = new Dictionary<int, Dictionary<int, Dictionary<int, string[]>>>();
        }
        public async void OnGet()
        {
            weekFromNow = IndexModel.scuffedWeek;
            for (int z=1;z<8;z++) 
            {
                BookingType.Add(z, new Dictionary<int, Dictionary<int, int>>());
                Players.Add(z, new Dictionary<int, Dictionary<int, string[]>>());
                for (int i = 1; i < 4; i++)
                {
                    BookingType[z].Add(i, new Dictionary<int, int>());
                    Players[z].Add(i, new Dictionary<int, string[]>());
                    for (int j = 1; j < 15; j++)
                    {
                        if (weekFromNow == 0)
                        {
                            if ((DateTime.Now.Hour >= j + 7 && ((int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek) == z) || z < ((int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek))
                            {
                                BookingType[z][i].Add(j, 4);
                                Players[z][i].Add(j, new string[2]);
                            }
                            else
                            {
                                BookingType[z][i].Add(j, 0);
                                Players[z][i].Add(j, new string[2]);
                            }
                        }
                        else
                        {
                            BookingType[z][i].Add(j, 0);
                            if (weekFromNow < 0) BookingType[z][i][j]=4;
                        }
                            

                    }
                }
            }
            
            List<Booking> temp= bookingService.GetAllBookings().Result;
            foreach (Booking b in temp.FindAll(i => i.Start <= DateTime.Now)) 
            {
                if(weekFromNow==0) bookingService.DeleteBooking(b);
                temp.Remove(b);
            }


            temp.RemoveAll(i=> Math.Round((i.Start-DateTime.Now.AddDays(IndexModel.scuffedWeek*7)).TotalDays)>(7-((int)DateTime.Now.DayOfWeek==0? 7:(int)DateTime.Now.DayOfWeek)));
            temp.RemoveAll(i => Math.Round((i.Start - DateTime.Now.AddDays(IndexModel.scuffedWeek * 7)).TotalDays) < (-7 + ((int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek)));
            
            foreach (Booking booking in temp) 
            {
                if (booking.Event_Id == null && booking.Team_Id == null) 
                {
                    BookingType[(int)booking.Start.DayOfWeek == 0 ? 7 : (int)booking.Start.DayOfWeek][booking.Court_Id][booking.Start.Hour - 7] = 1;
                    string[] m;
                    m = relationshipsServices.GetBookingMembers(booking.Id).Result;
                    Players[(int)booking.Start.DayOfWeek == 0 ? 7 : (int)booking.Start.DayOfWeek][booking.Court_Id][booking.Start.Hour - 7][0] = memberService.GetMemberById(int.Parse(m[0])).Result.Name;
                    Players[(int)booking.Start.DayOfWeek == 0 ? 7 : (int)booking.Start.DayOfWeek][booking.Court_Id][booking.Start.Hour - 7][1] = memberService.GetMemberById(int.Parse(m[1])).Result.Name;
                    Console.WriteLine(Players[(int)booking.Start.DayOfWeek == 0 ? 7 : (int)booking.Start.DayOfWeek][booking.Court_Id][booking.Start.Hour - 7][0] + " " + ((int)booking.Start.DayOfWeek == 0 ? 7 : (int)booking.Start.DayOfWeek) +" " + booking.Court_Id+" "+ (booking.Start.Hour - 7));
                    Console.WriteLine(BookingType[(int)booking.Start.DayOfWeek == 0 ? 7 : (int)booking.Start.DayOfWeek][booking.Court_Id][booking.Start.Hour - 7]);
                } 
                if(booking.Event_Id==null && booking.Team_Id!=null ) BookingType[(int)booking.Start.DayOfWeek == 0 ? 7 : (int)booking.Start.DayOfWeek][booking.Court_Id][booking.Start.Hour-7] = 2;
                if(booking.Event_Id!=null && booking.Team_Id==null ) BookingType[(int)booking.Start.DayOfWeek == 0 ? 7 : (int)booking.Start.DayOfWeek][booking.Court_Id][booking.Start.Hour-7] = 3;


            }
        }

        public async Task<IActionResult> OnPostForward() 
        {
            IndexModel.scuffedWeek++;
            return RedirectToPage("MakeBooking2");
        }

        public async Task<IActionResult> OnPostBackwards()
        {
            IndexModel.scuffedWeek--;
            return RedirectToPage("MakeBooking2");
        }
    }
}
