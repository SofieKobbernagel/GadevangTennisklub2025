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
        public Dictionary<int, Dictionary<int,Dictionary<int,int>>> BookingType { get; set; }

        public MakeBooking2Model(IBookingServiceAsync IBSA) 
        {
            bookingService = IBSA;
            BookingType=new Dictionary<int,Dictionary<int, Dictionary<int, int>>>();
        }
        public async void OnGet()
        {
            for (int z=1;z<8;z++) 
            {
                BookingType.Add(z, new Dictionary<int, Dictionary<int, int>>());
                for (int i = 1; i < 4; i++)
                {
                    BookingType[z].Add(i, new Dictionary<int, int>());
                    for (int j = 1; j < 15; j++)
                    {
                        if ((DateTime.Now.Hour >= j + 7 && ((int)DateTime.Now.DayOfWeek==0? 7:(int)DateTime.Now.DayOfWeek)==z) ||  z< ((int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek))
                        {
                            BookingType[z][i].Add(j, 4); 
                        }
                        else
                        {
                            BookingType[z][i].Add(j, 0);
                        }

                    }
                }
            }
            
            List<Booking> temp= bookingService.GetAllBookings().Result;
            temp.RemoveAll(i=> i.Start<=DateTime.Now);
            temp.RemoveAll(i => (i.Start - DateTime.Now).TotalDays > (7 - (int)DateTime.Now.DayOfWeek==0? 7: (int)DateTime.Now.DayOfWeek));
            //temp.RemoveAll(i => (i.Start - DateTime.Now).TotalDays > ( 0-((int)DateTime.Now.DayOfWeek==0? 7: (int)DateTime.Now.DayOfWeek)));
           
            
            foreach (Booking booking in temp) 
            {

                if(booking.Event_Id==null && booking.Team_Id==null ) BookingType[(int)booking.Start.DayOfWeek == 0 ? 7 : (int)booking.Start.DayOfWeek][booking.Court_Id][booking.Start.Hour-7] = 1;
                if(booking.Event_Id==null && booking.Team_Id!=null ) BookingType[(int)booking.Start.DayOfWeek == 0 ? 7 : (int)booking.Start.DayOfWeek][booking.Court_Id][booking.Start.Hour-7] = 2;
                if(booking.Event_Id!=null && booking.Team_Id==null ) BookingType[(int)booking.Start.DayOfWeek == 0 ? 7 : (int)booking.Start.DayOfWeek][booking.Court_Id][booking.Start.Hour-7] = 3;


            }
        }
    }
}
