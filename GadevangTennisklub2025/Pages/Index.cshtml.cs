using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace GadevangTennisklub2025.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public Calendar Calendar;

        public static int scuffedMonth;
        public static int scuffedYear;
        public static int scuffedWeek;
        public static string PrevPage = "Index";
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            scuffedMonth=DateTime.Now.Month;
            scuffedYear=DateTime.Now.Year;
            PrevPage = "Index";
        }

        public void OnPostMod() { Console.WriteLine("joke"); }
        public void OnPost() { Console.WriteLine("jo"); }
    }
}
