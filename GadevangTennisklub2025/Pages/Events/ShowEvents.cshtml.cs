using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace GadevangTennisklub2025.Pages.Events
{
    public class ShowEventsModel : PageModel
    {
        public List<Event> EventList;
        
        public IEventServiceAsync EventService;
        public int month { get;  set ; }

        public int year { get; set; }
        public ShowEventsModel(IEventServiceAsync IESA) 
        { EventService = IESA; }

        public async Task OnGet()
        {
            month = IndexModel.scuffedMonth;
            year = IndexModel.scuffedYear;
            EventList = await EventService.GetEventsAsync();

        }

        public async Task<IActionResult> OnPostForward() 
        {
            if (IndexModel.scuffedMonth == 12) { IndexModel.scuffedMonth = 1; IndexModel.scuffedYear++; }
            else { IndexModel.scuffedMonth++; }
            return RedirectToPage("ShowEvents");
        }
        public async Task<IActionResult> OnPostBackwards() 
        {
            if (IndexModel.scuffedMonth == 1) { IndexModel.scuffedMonth = 12; IndexModel.scuffedYear--; }
            else { IndexModel.scuffedMonth--; }
          
            
            return RedirectToPage("ShowEvents"); 
        }
    }
}
